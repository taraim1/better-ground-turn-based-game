using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardManager : Singletone<CardManager>
{
    [SerializeField] CardsSO cardsSO;
    [SerializeField] Transform card_spawnpoint;
    [SerializeField] Transform left_card_transform;
    [SerializeField] Transform right_card_transform;
    [SerializeField] Transform diactivated_card_transform;
    [SerializeField] Transform highlighted_card_transform;
    [SerializeField] Transform enemy_card_transform;
    [SerializeField] Transform enemy_card_highlighted_transform;

    // 스킬카드 코드
    public enum skillcard_code 
    { 
        simple_attack,
        simple_defend,
        simple_dodge
    }

    // 캐릭터들의 덱이 랜덤으로 섞여 들어가는 버퍼
    List<List<Cards>> cards_buffer = new List<List<Cards>>();

    public GameObject card_prefab;

    // 현재 보는 중인 카드패의 인덱스, 안 보는 중이면 -1
    public int active_index;

    // 현재 강조 중인 카드
    public card highlighted_card;

    // 카드 코드 주면 카드 데이터 줌
    public Cards get_card_by_code(skillcard_code code) 
    {
        return cardsSO.cards[(int)code];
    }

    // index번째 캐릭터의 덱 버퍼에서 첫 카드 뽑기
    public Cards PopCard(int index) 
    {
        if (cards_buffer[index].Count == 0) Setup_cardBuffer();

        Cards card = cards_buffer[index][0];
        cards_buffer[index].RemoveAt(0);
        return card;
    }

    void Setup_cardBuffer() // 버퍼 초기화
    { 
        cards_buffer.Clear();

        // 버퍼에 카드들 추가
        for (int i = 0; i < BattleManager.instance.hand_data.Count; i++) 
        {
            List<Cards> temp = new List<Cards>();

            // 파티의 캐릭터마다의 덱에서 코드를 얻어서 카드 데이터를 불러옴
            for (int j = 0; j < BattleManager.instance.playable_characters[i].GetComponent<Character>().deck.Length; j++) 
            {
                // 스크립터블 오브젝트에서 데이터를 뽑아옴
                Cards tempCard = cardsSO.cards[ (int)BattleManager.instance.playable_characters[i].GetComponent<Character>().deck[j] ];
                temp.Add(tempCard);
                
            }

            cards_buffer.Add(temp);
        }

        // 덱 버퍼 섞기
        for (int i = 0; i < cards_buffer.Count; i++) 
        {
            for (int j = 0; j < cards_buffer[i].Count; j++)
            {
                int rand = UnityEngine.Random.Range(0, cards_buffer[i].Count);
                Cards temp = cards_buffer[i][j];
                cards_buffer[i][j] = cards_buffer[i][rand];
                cards_buffer[i][rand] = temp;
            }
        }

        
    }

    public void Summon_card(int index) // 카드 생성 후 index번째의 패에 추가
    {
     
        var cardObj = Instantiate(card_prefab, card_spawnpoint.position, Quaternion.identity);
        if (index != active_index)
        {
            cardObj.transform.position = diactivated_card_transform.position;
        }
        card card = cardObj.GetComponent<card>();
        // index번째 캐릭터의 덱에서 카드를 뽑아옴
        card.Setup(PopCard(index), index);
        card.owner = BattleManager.instance.playable_characters[index];
        BattleManager.instance.hand_data[index].Add(card);

        Set_origin_order(index);
        Aline_cards(index);
    }

    public GameObject Summon_enemy_card(skillcard_code code, GameObject owner) // 적 카드 생성해서 리턴
    {

        var cardObj = Instantiate(card_prefab, enemy_card_transform.position, Quaternion.identity);
        card card = cardObj.GetComponent<card>();
        card.owner = owner;
        card.isEnemyCard = true;
        card.originPRS = new PRS(enemy_card_transform.position, enemy_card_transform.rotation, Vector3.one * 1.5f);
        card.Setup(cardsSO.cards[(int)code], 0);

        return cardObj;
    }

    public void Destroy_card(card card) // 카드 파괴
    {
        // 적 카드면
        if (card.isEnemyCard)
        {
            // 적 카드 리스트에서 카드 오브젝트 삭제
            EnemyAI enemyAI = card.owner.GetComponent<EnemyAI>();
            enemyAI.using_skill_Objects.Remove(card.gameObject);

            // 스킬카드 슬롯 삭제
            foreach (GameObject slot in enemyAI.skill_slots) 
            {
                if (slot.GetComponent<enemy_skillCard_slot>().enemy_Obj == card.owner) 
                {
                    Destroy(slot);
                }
            }

        }
        // 플레이어 카드면
        else 
        {
            BattleManager.instance.hand_data[card.owner.GetComponent<Character_Obj>().Character_index].Remove(card);
        }

        // 카드 오브젝트 삭제
        if (card.running_drag != null) 
        {
            card.StopCoroutine(card.running_drag);
        }
        Destroy(card.gameObject);

        // 카드 정렬
        CardManager.instance.Aline_cards(CardManager.instance.active_index);
    }

    // 적 카드를 강조
    public void highlight_enemy_card(GameObject obj) 
    {
        card card = obj.GetComponent<card>();
        PRS prs = new PRS(enemy_card_highlighted_transform.position, enemy_card_highlighted_transform.rotation, Vector3.one * 2.2f);
        card.MoveTransform(prs, true, 0.4f);
        card.state = card.current_mode.highlighted_enemy_card;

    }


    public void Set_origin_order(int index) // 카드 orderInLayer설정
    {
        if (index == -1) { return; }

        for (int i = 0; i < BattleManager.instance.hand_data[index].Count; i++) 
        {
            BattleManager.instance.hand_data[index][i].GetComponent<element_order>().Set_origin_order(i);
        }
    }



    public void Aline_cards(int index) // index번째 패의 카드 위치, 회전, 스케일, 순서 등 정렬 
    {
        // 현재 보고 있는 패가 없을 때의 active_index
        if (index == -1) 
        {
            return;
        }

        List<PRS> origin_cards_PRS;
        origin_cards_PRS = set_card_alignment(left_card_transform, right_card_transform, BattleManager.instance.hand_data[index].Count, 0.5f, Vector3.one * 1.8f, index);

        // 드래그 중인 카드가 있는지 검사
        bool isdragging = false;
        for (int j = 0; j < BattleManager.instance.hand_data[index].Count; j++)
        {
            if (BattleManager.instance.hand_data[index][j].state == card.current_mode.dragging)
            {
                isdragging = true;
            }
        }

        for (int i = 0; i < BattleManager.instance.hand_data[index].Count; i++) 
        {
            card targetCard = BattleManager.instance.hand_data[index][i];

            targetCard.originPRS = origin_cards_PRS[i];
            
            // 카드 겹침 때문에 보정값 넣어줌
            targetCard.originPRS.pos.z -= targetCard.GetComponent<element_order>().Get_order()/100f;

            // 드래그 중인 카드가 있다면 드래그 중이 아닌 카드들을 살짝 아래로 내림
            if (isdragging) 
            {
                if (targetCard.state != card.current_mode.dragging)
                {
                    targetCard.originPRS.pos.y -= 2f;
                }
                else 
                {
                    targetCard.originPRS.pos.y -= 0.3f;
                }
            }

            // 활성화된 카드면
            if (index == active_index)
            {

                // 하이라이트된 카드면
                if (targetCard == highlighted_card)
                {
                    // 드래그 중인 카드가 없다면
                    if (!isdragging) 
                    {
                        targetCard.MoveTransform(new PRS(new Vector3(highlighted_card_transform.position.x, highlighted_card_transform.position.y, targetCard.originPRS.pos.z), highlighted_card_transform.rotation, Vector3.one * 2.2f), true, 0.2f);
                    }
                    
                    continue;
                }

                // 일반 카드면

                targetCard.MoveTransform(targetCard.originPRS, true, 0.3f);

                          
            }
            // 비활성화 카드면 밑으로 내려감
            else 
            {
                targetCard.MoveTransform(targetCard.originPRS, false, 0f);
            }

        }

     
    }

    List<PRS> set_card_alignment(Transform leftTr, Transform rightTr, int CardCount, float height, Vector3 scale, int index) // 카드들의 PRS값 리스트를 계산해 반환
    {
        float[] objLerps = new float[CardCount];
        List<PRS> results = new List<PRS>(CardCount);

        // 활성화중인 패가 아닌 경우
        if (index != active_index) 
        {
            for (int i = 0; i < CardCount; i++) 
            {
                results.Add(new PRS(diactivated_card_transform.position, diactivated_card_transform.rotation, scale));
            }
            return results;
        }

        // 간격조정
        switch (CardCount) 
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.25f, 0.75f }; break;
            case 3: objLerps = new float[] { -0.5f, 0.5f, 1f }; break;
            default:
                float interval = 1f / (CardCount - 1);
                for (int i = 0; i < CardCount; i++) 
                {
                    objLerps[i] = interval * i;
                }
                break;
        }

        // 위치 계산
        for (int i = 0; i < CardCount; i++) 
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Quaternion.identity;
            if (CardCount >= 4) 
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                targetPos.y += (curve-0.5f);
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }

            results.Add(new PRS(targetPos, targetRot, scale));
        }

        return results;
    }


    public void Change_active_hand(int index) // index번째 패 활성화
    {
        active_index = index;
        for (int i = 0; i < BattleManager.instance.hand_data.Count; i++) 
        {
                Aline_cards(i);
        }
    }

    public void Setup_all() // 처음 세팅
    {
        Setup_cardBuffer();
        active_index = -1;
        highlighted_card = null;
    }




}
