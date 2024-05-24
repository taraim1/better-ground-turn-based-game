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

    // ��ųī�� �ڵ�
    public enum skillcard_code 
    { 
        simple_attack,
        simple_defend,
        simple_dodge
    }

    // ĳ���͵��� ���� �������� ���� ���� ����
    List<List<Cards>> cards_buffer = new List<List<Cards>>();

    public GameObject card_prefab;

    // ���� ���� ���� ī������ �ε���, �� ���� ���̸� -1
    public int active_index;

    // ���� ���� ���� ī��
    public card highlighted_card;

    // ī�� �ڵ� �ָ� ī�� ������ ��
    public Cards get_card_by_code(skillcard_code code) 
    {
        return cardsSO.cards[(int)code];
    }

    // index��° ĳ������ �� ���ۿ��� ù ī�� �̱�
    public Cards PopCard(int index) 
    {
        if (cards_buffer[index].Count == 0) Setup_cardBuffer();

        Cards card = cards_buffer[index][0];
        cards_buffer[index].RemoveAt(0);
        return card;
    }

    void Setup_cardBuffer() // ���� �ʱ�ȭ
    { 
        cards_buffer.Clear();

        // ���ۿ� ī��� �߰�
        for (int i = 0; i < BattleManager.instance.hand_data.Count; i++) 
        {
            List<Cards> temp = new List<Cards>();

            // ��Ƽ�� ĳ���͸����� ������ �ڵ带 �� ī�� �����͸� �ҷ���
            for (int j = 0; j < BattleManager.instance.playable_characters[i].GetComponent<Character>().deck.Length; j++) 
            {
                // ��ũ���ͺ� ������Ʈ���� �����͸� �̾ƿ�
                Cards tempCard = cardsSO.cards[ (int)BattleManager.instance.playable_characters[i].GetComponent<Character>().deck[j] ];
                temp.Add(tempCard);
                
            }

            cards_buffer.Add(temp);
        }

        // �� ���� ����
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

    public void Summon_card(int index) // ī�� ���� �� index��°�� �п� �߰�
    {
     
        var cardObj = Instantiate(card_prefab, card_spawnpoint.position, Quaternion.identity);
        if (index != active_index)
        {
            cardObj.transform.position = diactivated_card_transform.position;
        }
        card card = cardObj.GetComponent<card>();
        // index��° ĳ������ ������ ī�带 �̾ƿ�
        card.Setup(PopCard(index), index);
        card.owner = BattleManager.instance.playable_characters[index];
        BattleManager.instance.hand_data[index].Add(card);

        Set_origin_order(index);
        Aline_cards(index);
    }

    public GameObject Summon_enemy_card(skillcard_code code, GameObject owner) // �� ī�� �����ؼ� ����
    {

        var cardObj = Instantiate(card_prefab, enemy_card_transform.position, Quaternion.identity);
        card card = cardObj.GetComponent<card>();
        card.owner = owner;
        card.isEnemyCard = true;
        card.originPRS = new PRS(enemy_card_transform.position, enemy_card_transform.rotation, Vector3.one * 1.5f);
        card.Setup(cardsSO.cards[(int)code], 0);

        return cardObj;
    }

    public void Destroy_card(card card) // ī�� �ı�
    {
        // �� ī���
        if (card.isEnemyCard)
        {
            // �� ī�� ����Ʈ���� ī�� ������Ʈ ����
            EnemyAI enemyAI = card.owner.GetComponent<EnemyAI>();
            enemyAI.using_skill_Objects.Remove(card.gameObject);

            // ��ųī�� ���� ����
            foreach (GameObject slot in enemyAI.skill_slots) 
            {
                if (slot.GetComponent<enemy_skillCard_slot>().enemy_Obj == card.owner) 
                {
                    Destroy(slot);
                }
            }

        }
        // �÷��̾� ī���
        else 
        {
            BattleManager.instance.hand_data[card.owner.GetComponent<Character_Obj>().Character_index].Remove(card);
        }

        // ī�� ������Ʈ ����
        if (card.running_drag != null) 
        {
            card.StopCoroutine(card.running_drag);
        }
        Destroy(card.gameObject);

        // ī�� ����
        CardManager.instance.Aline_cards(CardManager.instance.active_index);
    }

    // �� ī�带 ����
    public void highlight_enemy_card(GameObject obj) 
    {
        card card = obj.GetComponent<card>();
        PRS prs = new PRS(enemy_card_highlighted_transform.position, enemy_card_highlighted_transform.rotation, Vector3.one * 2.2f);
        card.MoveTransform(prs, true, 0.4f);
        card.state = card.current_mode.highlighted_enemy_card;

    }


    public void Set_origin_order(int index) // ī�� orderInLayer����
    {
        if (index == -1) { return; }

        for (int i = 0; i < BattleManager.instance.hand_data[index].Count; i++) 
        {
            BattleManager.instance.hand_data[index][i].GetComponent<element_order>().Set_origin_order(i);
        }
    }



    public void Aline_cards(int index) // index��° ���� ī�� ��ġ, ȸ��, ������, ���� �� ���� 
    {
        // ���� ���� �ִ� �а� ���� ���� active_index
        if (index == -1) 
        {
            return;
        }

        List<PRS> origin_cards_PRS;
        origin_cards_PRS = set_card_alignment(left_card_transform, right_card_transform, BattleManager.instance.hand_data[index].Count, 0.5f, Vector3.one * 1.8f, index);

        // �巡�� ���� ī�尡 �ִ��� �˻�
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
            
            // ī�� ��ħ ������ ������ �־���
            targetCard.originPRS.pos.z -= targetCard.GetComponent<element_order>().Get_order()/100f;

            // �巡�� ���� ī�尡 �ִٸ� �巡�� ���� �ƴ� ī����� ��¦ �Ʒ��� ����
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

            // Ȱ��ȭ�� ī���
            if (index == active_index)
            {

                // ���̶���Ʈ�� ī���
                if (targetCard == highlighted_card)
                {
                    // �巡�� ���� ī�尡 ���ٸ�
                    if (!isdragging) 
                    {
                        targetCard.MoveTransform(new PRS(new Vector3(highlighted_card_transform.position.x, highlighted_card_transform.position.y, targetCard.originPRS.pos.z), highlighted_card_transform.rotation, Vector3.one * 2.2f), true, 0.2f);
                    }
                    
                    continue;
                }

                // �Ϲ� ī���

                targetCard.MoveTransform(targetCard.originPRS, true, 0.3f);

                          
            }
            // ��Ȱ��ȭ ī��� ������ ������
            else 
            {
                targetCard.MoveTransform(targetCard.originPRS, false, 0f);
            }

        }

     
    }

    List<PRS> set_card_alignment(Transform leftTr, Transform rightTr, int CardCount, float height, Vector3 scale, int index) // ī����� PRS�� ����Ʈ�� ����� ��ȯ
    {
        float[] objLerps = new float[CardCount];
        List<PRS> results = new List<PRS>(CardCount);

        // Ȱ��ȭ���� �а� �ƴ� ���
        if (index != active_index) 
        {
            for (int i = 0; i < CardCount; i++) 
            {
                results.Add(new PRS(diactivated_card_transform.position, diactivated_card_transform.rotation, scale));
            }
            return results;
        }

        // ��������
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

        // ��ġ ���
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


    public void Change_active_hand(int index) // index��° �� Ȱ��ȭ
    {
        active_index = index;
        for (int i = 0; i < BattleManager.instance.hand_data.Count; i++) 
        {
                Aline_cards(i);
        }
    }

    public void Setup_all() // ó�� ����
    {
        Setup_cardBuffer();
        active_index = -1;
        highlighted_card = null;
    }




}
