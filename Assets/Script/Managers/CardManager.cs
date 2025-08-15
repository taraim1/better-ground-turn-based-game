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
    fire_enchantment,
    scratch,
    distracting_attack,
    fast_stab,
    vital_point_stab,
    distracting_shot,
    aimed_shot,
    double_shot,
    swing,
    downward_strike,
    heavy_strike,
    beat_to_a_pulp,
    rampage,
    all_out_strike,
    awkward_footwork,
    confusing_footwork,
    confusion_spell,
    deflect,
    filp_balance,
    block,
    fend_off,
    clash
}

public class CardManager : Singletone<CardManager>
{
    [SerializeField] CardDataSO CardDataSO;
    [SerializeField] Transform card_spawnpoint;
    [SerializeField] Transform left_card_transform;
    [SerializeField] Transform left_card_over4_transform;
    [SerializeField] Transform right_card_transform;
    [SerializeField] Transform right_card_over4_transform;
    [SerializeField] Transform diactivated_card_transform;
    [SerializeField] Transform highlighted_card_transform;
    [SerializeField] Transform enemy_card_transform;
    [SerializeField] Transform enemy_card_highlighted_transform;

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

    // ī�� �ڵ� �ָ� ī�� ������ ��
    public CardData getData_by_code(skillcard_code code) 
    {
        return CardDataSO.CardData_dict[code];
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
            List<skillcard_code> deck = BattleManager.instance.playable_characters[i].GetComponent<Character>().Deck;

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
        Align_cards(index);
    }

    public GameObject Summon_enemy_card(skillcard_code code, Character owner) // �� ī�� �����ؼ� ����
    {

        var cardObj = Instantiate(card_prefab, enemy_card_transform.position, Quaternion.identity);
        card card = cardObj.GetComponent<card>();
        card.owner = owner;
        card.isEnemyCard = true;
        card.originPRS = new PRS(enemy_card_transform.position, enemy_card_transform.rotation, Vector3.one * 1.5f);
        card.Setup(getData_by_code(code), 0);

        return cardObj;
    }

    public void OnCardDestroyed(card card)
    {
        // ī�� ����
        Align_cards(active_index);
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
            originDatas_PRS = set_card_alignment(left_card_over4_transform, right_card_over4_transform, BattleManager.instance.hand_data[index].Count, 0.5f, Vector3.one * 1.8f, index);
        }
        else 
        {
            originDatas_PRS = set_card_alignment(left_card_transform, right_card_transform, BattleManager.instance.hand_data[index].Count, 0.5f, Vector3.one * 1.8f, index);
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
                        targetCard.MoveTransform(new PRS(new Vector3(highlighted_card_transform.position.x, highlighted_card_transform.position.y, targetCard.originPRS.pos.z), highlighted_card_transform.rotation, Vector3.one * 2f), true, 0.2f);
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
                Align_cards(i);
        }
    }

    public void highlight_card(card card) // ī�� ���̶���Ʈ
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

    private void OnSkillUsed(Character character, skillcard_code code) 
    {
        Align_cards(active_index);
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
        ActionManager.skill_used += OnSkillUsed;
    }

    private void OnDisable()
    {
        ActionManager.character_drag_started -= On_character_drag_start;
        ActionManager.character_drag_ended -= On_character_drag_end;
        ActionManager.character_died -= OnCharacterDied;
        ActionManager.card_destroyed -= OnCardDestroyed;
        ActionManager.skill_used -= OnSkillUsed;
    }


}
