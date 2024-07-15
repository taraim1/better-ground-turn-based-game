using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRay : MonoBehaviour
{
    private bool isBattleEnded;

    private void Awake()
    {
        isBattleEnded = false;
        ActionManager.battle_ended += OnBattleEnd;
    }

    private void OnDestroy()
    {
        ActionManager.battle_ended -= OnBattleEnd;
    }

    private void OnBattleEnd(bool victory) 
    {
        isBattleEnded = true;
    }

    private GameObject check_clicked_obj() 
    {
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ��ųī�� ���̾� Ŭ�� ����
        Collider2D skill_collider = Physics2D.OverlapPoint(MousePos, LayerMask.GetMask("SkillCard"));
        if (skill_collider != null)
        {
            return skill_collider.gameObject;
        }

        // ��ų ���� Ŭ�� ����
        Collider2D skill_slot_collider = Physics2D.OverlapPoint(MousePos, LayerMask.GetMask("skillslot"));
        if (skill_slot_collider != null) 
        {
            return skill_slot_collider.gameObject;
        }

        // ĳ���� Ŭ�� ����
        Collider2D chracter_collider = Physics2D.OverlapPoint(MousePos, LayerMask.GetMask("character"));
        if (chracter_collider != null)
        {
            return chracter_collider.gameObject;
        }

        // �� �� Ŭ�� ����
        Collider2D collider = Physics2D.OverlapPoint(MousePos);
        if (collider != null)
        {
            return collider.gameObject;
        }


        return null;
    }

    private void apply_click_result_by_tag(GameObject obj) 
    {
        switch (obj.tag) 
        {
            // �Ʊ� ĳ���� Ŭ�� �� 
            case "PlayerCharacter":
                Character character = obj.GetComponent<Character>();
                // �� ĳ������ �� ������
                CardManager.instance.clear_highlighted_card();
                CardManager.instance.Change_active_hand(character.data.Character_index);
                CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                // �巡�� ���� ����
                character.data.running_drag = StartCoroutine(character.detect_drag());
                break;

            // ī�� Ŭ�� ��
            case "SkillCard":
                card card = obj.GetComponent<card>();
                // �Ʊ� ī���̸�
                if (card.isEnemyCard == false)
                {
                    // ī�� �巡�� ���� ����
                    card.running_drag = StartCoroutine(card.detect_drag(false));

                    // �� ī�� ���� ����
                    ActionManager.enemy_skill_card_deactivate?.Invoke();
                }
                // ���� ī���
                else
                {
                    // �� ī�� ���� ����
                    if (card.state == card.current_mode.highlighted_enemy_card)
                    {
                        ActionManager.enemy_skill_card_deactivate?.Invoke();
                    }
                }
                break;

            // ī�� Ư��ȿ�� ���� Ŭ����
            case "skillDescription":
                card = obj.GetComponent<card_description>().get_target();

                // �Ʊ� ī���̸�
                if (card.isEnemyCard == false)
                {
                    // ī�� �巡�� ���� ����
                    card.running_drag = StartCoroutine(card.detect_drag(true));

                    // �� ī�� ���� ����
                    ActionManager.enemy_skill_card_deactivate?.Invoke();
                }
                // ���� ī���
                else
                {
                    // �� ī�� ���� ����
                    if (card.state == card.current_mode.highlighted_enemy_card)
                    {
                        ActionManager.enemy_skill_card_deactivate?.Invoke();
                    }
                }
                break;

            // ĳ���� Ư��ȿ�� (���� / �����) ���� Ŭ����
            case "effectDescription":
                break; // �ƹ� �͵� �� ��

            // �� ��ų ���� Ŭ����
            case "enemySkillSlot":
                // �Ʊ� ī�� ���� ����
                CardManager.instance.clear_highlighted_card();

                // ��� ī�带 ���� order�� 
                CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                // ī�� ��ġ ��� �� ����
                CardManager.instance.Align_cards(CardManager.instance.active_index);
                break;

            // �� ���� �� Ŭ����
            default:
                CardManager.instance.clear_highlighted_card();
                CardManager.instance.Change_active_hand(-1);

                // ��� ī�� ����
                for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
                {
                    CardManager.instance.Align_cards(i);
                }

                // �� ī�� ���� ����
                ActionManager.enemy_skill_card_deactivate?.Invoke();

                // ��� ī�带 ���� order��
                CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                // ī�� ��ġ ��� �� ����
                CardManager.instance.Align_cards(CardManager.instance.active_index);
                break;

        }     
    }

    void Update()
    {
        // ���� ���� ����
        if (isBattleEnded) { return; }

        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D character_collider = Physics2D.OverlapPoint(MousePos, LayerMask.GetMask("character"));
        if (character_collider != null) 
        {
            // ĳ���� Ÿ���� ����
            if (character_collider.gameObject.tag == "PlayerCharacter" || character_collider.gameObject.tag == "EnemyCharacter") 
            { 
                BattleCalcManager.instance.set_target(character_collider.gameObject.GetComponent<Character>());
            }
        }



        // Ŭ�� or ��ġ ����, ���� ����� �÷��̾� ��ų ��� ������� �۵���
        if (Input.GetMouseButtonDown(0) && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase) 
        {
            GameObject gameObj = check_clicked_obj(); // Ŭ���� ������Ʈ ����

            if (gameObj != null) 
            {
                apply_click_result_by_tag(gameObj);
            }
        }

        // ������Ʈ ������ ���� �� ����, ���� ����� �÷��̾� ��ų ��� ������� �۵���
        if (Input.GetMouseButtonUp(0) && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase) 
        {
            Collider2D clicked_collider = Physics2D.OverlapPoint(MousePos);
            
            if (clicked_collider != null)
            {
                // ��ų ��� ����
                BattleCalcManager.instance.Calc_skill_use();
            }
        }
    }
}
