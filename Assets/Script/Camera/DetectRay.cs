using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRay : MonoBehaviour
{


    void Update()
    {
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = Physics2D.OverlapPoint(MousePos);

        if (collider != null) 
        {
            // ĳ���� Ÿ���� ����
            if (collider.gameObject.tag == "PlayerCharacter" || collider.gameObject.tag == "EnemyCharacter") 
            { 
                BattleCalcManager.instance.set_target(collider.gameObject.GetComponent<Character>());
            }
        }


        // Ŭ�� or ��ġ
        if (Input.GetMouseButtonDown(0)) 
        { 
            Collider2D clicked_collider = Physics2D.OverlapPoint(MousePos);

            // ������Ʈ ������ �� ����, ���� ����� �÷��̾� ��ų ��� ������� �۵���
            if (clicked_collider != null && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase) 
            {
                GameObject gameObj = clicked_collider.gameObject;

                // �������� ĳ���� Ŭ�� �� �� ĳ������ �� ������
                if (gameObj.tag == "PlayerCharacter")
                {
                    CardManager.instance.highlighted_card = null;
                    CardManager.instance.Change_active_hand(gameObj.GetComponent<Character>().Character_index);
                    CardManager.instance.Set_origin_order(CardManager.instance.active_index);
                }
                // �������� ī�� Ŭ�� ��
                else if (gameObj.tag == "SkillCard")
                {
                    card card = gameObj.GetComponent<card>();
                    // �Ʊ� ī���̸�
                    if (card.isEnemyCard == false)
                    {
                        // ī�� �巡�� ���� ����
                        card.running_drag = StartCoroutine(card.detect_drag());

                        // �� ī�� ���� ����
                        BattleEventManager.Trigger_event("enemy_skill_card_deactivate");
                    }
                    // ���� ī���
                    else
                    {
                        // �� ī�� ���� ����
                        if (card.state == card.current_mode.highlighted_enemy_card)
                        {
                            BattleEventManager.Trigger_event("enemy_skill_card_deactivate");
                        }
                    }
                }
                // �� ��ų ���� Ŭ����
                else if (gameObj.tag == "enemySkillSlot") 
                {
                    // �Ʊ� ī�� ���� ����
                    CardManager.instance.highlighted_card = null;

                    // ��� ī�带 ���� order�� 
                    CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                    // ī�� ��ġ ��� �� ����
                    CardManager.instance.Aline_cards(CardManager.instance.active_index);
                }
                else // �� �̿��� �� Ŭ����
                {
                    CardManager.instance.highlighted_card = null;
                    CardManager.instance.Change_active_hand(-1);

                    // ��� ī�� ����
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
                    {
                        CardManager.instance.Aline_cards(i);
                    }

                    // �� ī�� ���� ����
                    BattleEventManager.Trigger_event("enemy_skill_card_deactivate");

                    // ��� ī�带 ���� order��
                    CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                    // ī�� ��ġ ��� �� ����
                    CardManager.instance.Aline_cards(CardManager.instance.active_index);
                }
            }
        }

        // ������Ʈ ������ ���� �� ����, ���� ����� �÷��̾� ��ų ��� ������� �۵���
        if (Input.GetMouseButtonUp(0) && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase) 
        {
            Collider2D clicked_collider = Physics2D.OverlapPoint(MousePos);
            
            if (clicked_collider != null)
            {
                GameObject gameObj = clicked_collider.gameObject;

                // ��ų ��� ����
                BattleCalcManager.instance.Calc_skill_use();
            }
        }
    }
}
