using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.IO;
using DG.Tweening.Plugins.Core.PathCore;

// ��ųī�� �ڵ�
public enum skillcard_code
{
    simple_attack,
    simple_defend,
    simple_dodge,
    powerful_attack,
    fire_ball,
    concentration,
    fire_enchantment
}

public class CardManager : Singletone<CardManager>
{
    [SerializeField] CardDataSO CardDataSO;
    [SerializeField] Transform card_spawnpoint;
    [SerializeField] Transform leftData_transform;
    [SerializeField] Transform leftData_over4_transform;
    [SerializeField] Transform rightData_transform;
    [SerializeField] Transform rightData_over4_transform;
    [SerializeField] Transform diactivatedData_transform;
    [SerializeField] Transform highlightedData_transform;
    [SerializeField] Transform enemyData_transform;
    [SerializeField] Transform enemyData_highlighted_transform;

    // ��Ÿ ī�� ���� ���� Ŭ����
    private class card_data_json 
    {
        // ī�� ��� �����Ͱ� ���� ����Ʈ (i���� i�� ��ų �������)
        public List<bool> unlockedData_checkList;

        // ������ �ҷ�����
        public void read_json() 
        { 
            string output = File.ReadAllText(Application.dataPath + "/Data/skillData.json");
            this.unlockedData_checkList = JsonUtility.FromJson<card_data_json>(output).unlockedData_checkList;
        }

        // ������ �����ϱ�
        public void write_json() 
        {
            string output = JsonUtility.ToJson(this, true);
            File.WriteAllText(Application.dataPath + "/Data/skillData.json", output);
        }
    }

    card_data_json JsonCardData = new card_data_json();

    // ĳ���͵��� ���� �������� ���� ���� ����
    List<List<CardData>> CardData_buffer = new List<List<CardData>>();

    public GameObject card_prefab;

    // ���� ���� ���� ĳ���� ī������ �ε���, �� ���� ���̸� -1
    public int active_index;

    // ���� ���� ���� ī��
    public card highlightedData;

    // ī�� ȿ�� �������ִ� ������Ʈ�� ������ �ִ� ��
    private card_description card_Description;

    private bool _isCharacterDragging = false;

    private void Start()
    {
        JsonCardData.read_json();
    }

    // ī�� �ڵ� �ָ� ī�� ������ ��
    public CardData getData_by_code(skillcard_code code) 
    {
        return CardDataSO.CardData_dict[code];
    }


    // ī�� �ڵ� �ָ� ����Ȱ��� ã����
    public bool check_unlocked(skillcard_code code) 
    {
        return JsonCardData.unlockedData_checkList[(int)code];
    }

    public void set_unlocked(skillcard_code code, bool value) 
    {
        JsonCardData.unlockedData_checkList[(int)code] = value;
        JsonCardData.write_json();
    }

    // index��° ĳ������ �� ���ۿ��� ù ī�� �̱�
    public CardData PopCard(int index) 
    {
        if (CardData_buffer[index].Count == 0) SetupDataBuffer();

        CardData card = CardData_buffer[index][0];
        CardData_buffer[index].RemoveAt(0);
        return card;
    }

    void SetupDataBuffer() // ���� �ʱ�ȭ
    { 
        CardData_buffer.Clear();

        // ���ۿ� ī��� �߰�
        for (int i = 0; i < BattleManager.instance.playable_characters.Count; i++) 
        {
            List<CardData> temp = new List<CardData>();
            List<skillcard_code> deck = BattleManager.instance.playable_characters[i].GetComponent<Character>().Get_deck_copy();

            // ��Ƽ�� ĳ���͸����� ������ �ڵ带 �� ī�� �����͸� �ҷ���
            for (int j = 0; j < deck.Count; j++) 
            {
                // ��ũ���ͺ� ������Ʈ���� �����͸� �̾ƿ�
                CardData tempCard = getData_by_code(deck[j]);
                temp.Add(tempCard);
                
            }

            CardData_buffer.Add(temp);
        }

        // �� ���� ����
        for (int i = 0; i < CardData_buffer.Count; i++) 
        {
            for (int j = 0; j < CardData_buffer[i].Count; j++)
            {
                int rand = UnityEngine.Random.Range(0, CardData_buffer[i].Count);
                CardData temp = CardData_buffer[i][j];
                CardData_buffer[i][j] = CardData_buffer[i][rand];
                CardData_buffer[i][rand] = temp;
            }
        }

        
    }

    public void SummonData(int index) // ī�� ���� �� index��°�� �п� �߰�
    {
     
        var cardObj = Instantiate(card_prefab, card_spawnpoint.position, Quaternion.identity);
        if (index != active_index)
        {
            cardObj.transform.position = diactivatedData_transform.position;
        }
        card card = cardObj.GetComponent<card>();
        // index��° ĳ������ ������ ī�带 �̾ƿ�
        card.Setup(PopCard(index), index);
        card.owner = BattleManager.instance.playable_characters[index];
        BattleManager.instance.hand_data[index].Add(card);

        Set_origin_order(index);
        Align_cards(index);
    }

    public GameObject Summon_enemyData(skillcard_code code, GameObject owner) // �� ī�� �����ؼ� ����
    {

        var cardObj = Instantiate(card_prefab, enemyData_transform.position, Quaternion.identity);
        card card = cardObj.GetComponent<card>();
        card.owner = owner;
        card.isEnemyCard = true;
        card.originPRS = new PRS(enemyData_transform.position, enemyData_transform.rotation, Vector3.one * 1.5f);
        card.Setup(getData_by_code(code), 0);

        return cardObj;
    }

    public void OnCardDestroyed(card card)
    {
        // ī�� ����
        Align_cards(active_index);
    }

    // �� ī�带 ����
    public void highlight_enemyData(GameObject obj) 
    {
        card card = obj.GetComponent<card>();
        PRS prs = new PRS(enemyData_highlighted_transform.position, enemyData_highlighted_transform.rotation, Vector3.one * 2.2f);
        card.MoveTransform(prs, true, 0.4f);
        card.state = card.current_mode.highlighted_enemyData;

    }


    public void Set_origin_order(int index) // ī�� orderInLayer����
    {
        if (index == -1) { return; }

        for (int i = 0; i < BattleManager.instance.hand_data[index].Count; i++) 
        {
            BattleManager.instance.hand_data[index][i].GetComponent<element_order>().Set_origin_order(i);
        }
    }



    public void Align_cards(int index) // index��° ���� ī�� ��ġ, ȸ��, ������, ���� �� ���� 
    {
        // ���� ���� �ִ� �а� ���� ���� active_index
        if (index == -1) 
        {
            return;
        }

        List<PRS> originDatas_PRS;
        if (BattleManager.instance.hand_data[index].Count >= 4) 
        {
            originDatas_PRS = setData_alignment(leftData_over4_transform, rightData_over4_transform, BattleManager.instance.hand_data[index].Count, 0.5f, Vector3.one * 1.8f, index);
        }
        else 
        {
            originDatas_PRS = setData_alignment(leftData_transform, rightData_transform, BattleManager.instance.hand_data[index].Count, 0.5f, Vector3.one * 1.8f, index);
        }
        

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

            targetCard.originPRS = originDatas_PRS[i];
            
            // ī�� ��ħ ������ ������ �־���
            targetCard.originPRS.pos.z -= targetCard.GetComponent<element_order>().Get_order()/100f;

            // �巡�� ���� ī�峪 ĳ���Ͱ� �ִٸ� �巡�� ���� �ƴ� ī����� ��¦ �Ʒ��� ����
            if (isdragging || _isCharacterDragging) 
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
                if (targetCard == highlightedData)
                {
                    // �巡�� ���� ī�尡 ���ٸ�
                    if (!isdragging) 
                    {
                        targetCard.MoveTransform(new PRS(new Vector3(highlightedData_transform.position.x, highlightedData_transform.position.y, targetCard.originPRS.pos.z), highlightedData_transform.rotation, Vector3.one * 2f), true, 0.2f);
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

    List<PRS> setData_alignment(Transform leftTr, Transform rightTr, int CardCount, float height, Vector3 scale, int index) // ī����� PRS�� ����Ʈ�� ����� ��ȯ
    {
        float[] objLerps = new float[CardCount];
        List<PRS> results = new List<PRS>(CardCount);

        // Ȱ��ȭ���� �а� �ƴ� ���
        if (index != active_index) 
        {
            for (int i = 0; i < CardCount; i++) 
            {
                results.Add(new PRS(diactivatedData_transform.position, diactivatedData_transform.rotation, scale));
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
                Align_cards(i);
        }
    }

    public void highlightData(card card) // ī�� ���̶���Ʈ
    {
        highlightedData = card;
        card_Description.Set_target(card);
    }

    public void clear_highlighted_card() // ī�� ���̶���Ʈ ����
    {
        highlightedData = null;
        card_Description.Clear_target();
    }

    private void On_character_drag_start() 
    {
        _isCharacterDragging = true;
        Align_cards(active_index);
    }

    private void On_character_drag_end()
    {
        _isCharacterDragging = false;
        Align_cards(active_index);
    }

    private void OnCharacterDied(Character character) 
    {
        // �� �����
        Change_active_hand(-1);
    }
    public void Setup_all() // ó�� ����
    {
        SetupDataBuffer();
        active_index = -1;
        card_Description = GameObject.Find("card_description_base").GetComponent<card_description>();
        clear_highlighted_card();
    }

    private void OnEnable()
    {
        ActionManager.character_drag_started += On_character_drag_start;
        ActionManager.character_drag_ended += On_character_drag_end;
        ActionManager.character_died += OnCharacterDied;
        ActionManager.card_destroyed += OnCardDestroyed;
    }

    private void OnDisable()
    {
        ActionManager.character_drag_started -= On_character_drag_start;
        ActionManager.character_drag_ended -= On_character_drag_end;
        ActionManager.character_died -= OnCharacterDied;
        ActionManager.card_destroyed -= OnCardDestroyed;
    }


}
