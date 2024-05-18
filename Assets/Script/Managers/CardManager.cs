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

    List<Cards> cards_buffer;
    public GameObject card_prefab;

    List<card> hand = new List<card>();

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

    void Summon_card() // ī�� ���� �� �п� �߰�
    {
        var cardObj = Instantiate(card_prefab, card_spawnpoint.position, Quaternion.identity);
        card card = cardObj.GetComponent<card>();
        card.Setup(PopCard());
        hand.Add(card);

        Set_origin_order();
        Aline_cards();
    } 

    void Set_origin_order() // ī�� orderInLayer����
    {
        for (int i = 0; i < hand.Count; i++) 
        {
            hand[i].GetComponent<element_order>().Set_origin_order(i);
        }
    }

    void Aline_cards() // ī�� ��ġ, ȸ��, ������ �� ���� 
    { 
        List<PRS> origin_cards_PRS = new List<PRS>();
        origin_cards_PRS = Round_alignment(left_card_transform, right_card_transform, hand.Count, 0.5f, Vector3.one * 1.5f);

        for (int i = 0; i < hand.Count; i++) 
        {
            card targetCard = hand[i];

            targetCard.originPRS = origin_cards_PRS[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);

        }
    }

    List<PRS> Round_alignment(Transform leftTr, Transform rightTr, int CardCount, float height, Vector3 scale) // ī����� PRS�� ����Ʈ�� ����� ��ȯ
    {
        float[] objLerps = new float[CardCount];
        List<PRS> results = new List<PRS>(CardCount);

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
        Setup_cardBuffer();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1)) 
        {
            Summon_card();
        }
    }
}
