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

    // ���� Ŭ���� ī��
    public card highlighted_card;

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
            for (int j = 0; j < BattleManager.instance.playable_character_data[i].deck.Length; j++) 
            {
                // ��ũ���ͺ� ������Ʈ���� �����͸� �̾ƿ�
                Cards tempCard = cardsSO.cards[ (int) BattleManager.instance.playable_character_data[i].deck[j] ];
                temp.Add(tempCard);
                
            }

            cards_buffer.Add(temp);
        }

        // �� ���� ����
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
        BattleManager.instance.hand_data[index].Add(card);

        Set_origin_order(index);
        Aline_cards(index);
    } 

    void Set_origin_order(int index) // ī�� orderInLayer����
    {
        for (int i = 0; i < BattleManager.instance.hand_data[index].Count; i++) 
        {
            BattleManager.instance.hand_data[index][i].GetComponent<element_order>().Set_origin_order(i);
        }
    }

    public void Aline_cards(int index) // ī�� ��ġ, ȸ��, ������, ���� �� ���� 
    { 
        List<PRS> origin_cards_PRS = new List<PRS>();
        origin_cards_PRS = set_card_alignment(left_card_transform, right_card_transform, BattleManager.instance.hand_data[index].Count, 0.5f, Vector3.one * 1.5f, index);

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

            // �巡�� ���� ī�尡 �ִٸ� ī�带 ��¦ �Ʒ��� ����
            if (isdragging) 
            {
                targetCard.originPRS.pos.y -= 2;
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
                        targetCard.MoveTransform(new PRS(new Vector3(highlighted_card_transform.position.x, highlighted_card_transform.position.y, targetCard.originPRS.pos.z), highlighted_card_transform.rotation, Vector3.one * 2f), true, 0.2f);
                    }
                    
                    continue;
                }

                // �Ϲ� ī���
                if (isdragging)
                {
                    targetCard.MoveTransform(targetCard.originPRS, true, 0.4f);
                }
                else 
                {
                    targetCard.MoveTransform(targetCard.originPRS, true, 0.5f);
                }
                          
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
