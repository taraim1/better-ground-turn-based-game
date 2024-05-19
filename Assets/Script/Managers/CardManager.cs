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

    List<Cards> cards_buffer;
    public GameObject card_prefab;

    // ���� ���� ���� ī������ �ε���, �� ���� ���̸� -1
    public int active_index;

    // ���� Ŭ���� ī��
    public card highlighted_card;

    // ���ۿ��� ù ī�� �̱�
    public Cards PopCard() 
    {
        if (cards_buffer.Count == 0) Setup_cardBuffer();

        Cards card = cards_buffer[0];
        cards_buffer.RemoveAt(0);
        return card;
    }

    void Setup_cardBuffer() // ���� �ʱ�ȭ
    { 
        cards_buffer = new List<Cards>();
        // ���ۿ� ī�� �߰�
        for (int i = 0; i < cardsSO.cards.Length; i++) 
        { 
            Cards card = cardsSO.cards[i];
            cards_buffer.Add(card);
        }

        // ���� ����
        for (int i = 0; i < cards_buffer.Count; i++) 
        { 
            int rand = Random.Range(0, cards_buffer.Count);
            Cards temp = cards_buffer[i];
            cards_buffer[i] = cards_buffer[rand];
            cards_buffer[rand] = temp;
        }

        
    }

    void Summon_card(int index) // ī�� ���� �� index��°�� �п� �߰�
    {
     
        var cardObj = Instantiate(card_prefab, card_spawnpoint.position, Quaternion.identity);
        if (index != active_index)
        {
            cardObj.transform.position = diactivated_card_transform.position;
        }
        card card = cardObj.GetComponent<card>();
        card.Setup(PopCard(), index);
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

    public void Aline_cards(int index) // ī�� ��ġ, ȸ��, ������ �� ���� 
    { 
        List<PRS> origin_cards_PRS = new List<PRS>();
        origin_cards_PRS = set_card_alignment(left_card_transform, right_card_transform, BattleManager.instance.hand_data[index].Count, 0.5f, Vector3.one * 1.5f, index);

        for (int i = 0; i < BattleManager.instance.hand_data[index].Count; i++) 
        {
            card targetCard = BattleManager.instance.hand_data[index][i];

            targetCard.originPRS = origin_cards_PRS[i];
            
            // ī�� ��ħ ������ ������ �־���
            targetCard.originPRS.pos.z -= targetCard.GetComponent<element_order>().Get_order()/100f;

            // Ȱ��ȭ�� ī���
            if (index == active_index)
            {
                // ���̶���Ʈ�� ī���
                if (targetCard == highlighted_card)
                {
                    targetCard.MoveTransform(new PRS(highlighted_card_transform.position, highlighted_card_transform.rotation, Vector3.one * 2f), true, 0.2f);
                }
                // �ƴϸ�
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1)) 
        {
            Summon_card(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Summon_card(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            Summon_card(2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Summon_card(3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            Change_active_hand(-1);
        }
    }
}
