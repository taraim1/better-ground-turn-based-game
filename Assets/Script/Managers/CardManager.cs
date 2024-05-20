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

    // 현재 클릭한 카드
    public card highlighted_card;

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
            for (int j = 0; j < BattleManager.instance.playable_character_data[i].deck.Length; j++) 
            {
                // 스크립터블 오브젝트에서 데이터를 뽑아옴
                Cards tempCard = cardsSO.cards[ (int) BattleManager.instance.playable_character_data[i].deck[j] ];
                temp.Add(tempCard);
                
            }

            cards_buffer.Add(temp);
        }

        // 덱 버퍼 섞기
        for (int i = 0; i < cards_buffer.Count; i++) 
        {
            for (int j = 0; j < cards_buffer[i].Count; j++)
            {
                int rand = Random.Range(0, cards_buffer[i].Count);
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
        BattleManager.instance.hand_data[index].Add(card);

        Set_origin_order(index);
        Aline_cards(index);
    } 

    void Set_origin_order(int index) // 카드 orderInLayer설정
    {
        for (int i = 0; i < BattleManager.instance.hand_data[index].Count; i++) 
        {
            BattleManager.instance.hand_data[index][i].GetComponent<element_order>().Set_origin_order(i);
        }
    }

    public void Aline_cards(int index) // 카드 위치, 회전, 스케일, 순서 등 정렬 
    { 
        List<PRS> origin_cards_PRS = new List<PRS>();
        origin_cards_PRS = set_card_alignment(left_card_transform, right_card_transform, BattleManager.instance.hand_data[index].Count, 0.5f, Vector3.one * 1.5f, index);

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

            // 드래그 중인 카드가 있다면 카드를 살짝 아래로 내림
            if (isdragging) 
            {
                targetCard.originPRS.pos.y -= 2;
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
                        targetCard.MoveTransform(new PRS(new Vector3(highlighted_card_transform.position.x, highlighted_card_transform.position.y, targetCard.originPRS.pos.z), highlighted_card_transform.rotation, Vector3.one * 2f), true, 0.2f);
                    }
                    
                    continue;
                }

                // 일반 카드면
                if (isdragging)
                {
                    targetCard.MoveTransform(targetCard.originPRS, true, 0.4f);
                }
                else 
                {
                    targetCard.MoveTransform(targetCard.originPRS, true, 0.5f);
                }
                          
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

    public void Setup_all() 
    {
        Setup_cardBuffer();
        active_index = -1;
        highlighted_card = null;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Battle") 
        {
            Destroy(this.gameObject);
        }
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


}
