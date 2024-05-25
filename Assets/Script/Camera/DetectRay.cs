using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRay : MonoBehaviour
{


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        { 
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D clicked_collider = Physics2D.OverlapPoint(pos);

            // ������Ʈ Ŭ�� ����
            if (clicked_collider != null) 
            {
                GameObject gameObj = clicked_collider.gameObject;

                // �������� ĳ���� Ŭ�� �� �� ĳ������ �� ������
                if (gameObj.tag == "PlayerCharacter")
                {
                    CardManager.instance.highlighted_card = null;
                    CardManager.instance.Change_active_hand(gameObj.GetComponent<Character_Obj>().Character_index);
                }
                // �������� ī�� Ŭ�� ��
                else if (gameObj.tag == "SkillCard")
                {
                    // �Ʊ� ī���
                    if (gameObj.GetComponent<card>().isEnemyCard == false)
                    {
                        // ī�� �巡�� ���� ����
                        gameObj.GetComponent<card>().running_drag = StartCoroutine(gameObj.GetComponent<card>().detect_drag());

                        // �� ī�� ���� ����
                        BattleEventManager.Trigger_event("enemy_skill_card_deactivate");
                    }
                    // ���� ī���
                    else
                    {
                        card card = gameObj.GetComponent<card>();
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

        if (Input.GetMouseButtonUp(0)) 
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D clicked_collider = Physics2D.OverlapPoint(pos);
            
            if (clicked_collider != null)
            {
                GameObject gameObj = clicked_collider.gameObject;

                // ��ų ��� ����
                BattleCalcManager.instance.Calc_skill_use();
            }
        }
    }
}
