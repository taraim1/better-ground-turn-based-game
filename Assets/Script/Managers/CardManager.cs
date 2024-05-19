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

    List<Cards> cards_buffer;
    public GameObject card_prefab;

    // 현재 보는 중인 카드패의 인덱스, 안 보는 중이면 -1
    public int active_index;

    // 버퍼에서 첫 카드 뽑기
    public Cards PopCard() 
    {
        if (cards_buffer.Count == 0) Setup_cardBuffer();

        Cards card = cards_buffer[0];
        cards_buffer.RemoveAt(0);
        return card;
    }

    void Setup_cardBuffer() // 버퍼 초기화
    { 
        cards_buffer = new List<Cards>();
        // 버퍼에 카드 추가
        for (int i = 0; i < cardsSO.cards.Length; i++) 
        { 
            Cards card = cardsSO.cards[i];
            cards_buffer.Add(card);
        }

        // 버퍼 섞기
        for (int i = 0; i < cards_buffer.Count; i++) 
        { 
            int rand = Random.Range(0, cards_buffer.Count);
            Cards temp = cards_buffer[i];
            cards_buffer[i] = cards_buffer[rand];
            cards_buffer[rand] = temp;
        }

        
    }

    void Summon_card(int index) // 카드 생성 후 index번째의 패에 추가
    {
     
        var cardObj = Instantiate(card_prefab, card_spawnpoint.position, Quaternion.identity);
        if (index != active_index)
        {
            cardObj.transform.position = diactivated_card_transform.position;
        }
        card card = cardObj.GetComponent<card>();
        card.Setup(PopCard());
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

    void Aline_cards(int index) // 카드 위치, 회전, 스케일 등 정렬 
    { 
        List<PRS> origin_cards_PRS = new List<PRS>();
        origin_cards_PRS = Round_alignment(left_card_transform, right_card_transform, BattleManager.instance.hand_data[index].Count, 0.5f, Vector3.one * 1.5f, index);

        for (int i = 0; i < BattleManager.instance.hand_data[index].Count; i++) 
        {
            card targetCard = BattleManager.instance.hand_data[index][i];

            targetCard.originPRS = origin_cards_PRS[i];
            if (index == active_index)
            {
                targetCard.MoveTransform(targetCard.originPRS, true, 0.5f);
            }
            else 
            {
                targetCard.MoveTransform(targetCard.originPRS, false, 0f);
            }

        }
    }

    List<PRS> Round_alignment(Transform leftTr, Transform rightTr, int CardCount, float height, Vector3 scale, int index) // 카드들의 PRS값 리스트를 계산해 반환
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

    public void Change_active_hand(int index) // index번째 패 활성화
    {
        active_index = index;
        for (int i = 0; i < BattleManager.instance.hand_data.Count; i++) 
        {
                Aline_cards(i);
        }
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
