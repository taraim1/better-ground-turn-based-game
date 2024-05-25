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

            // 오브젝트 클릭 감지
            if (clicked_collider != null) 
            {
                GameObject gameObj = clicked_collider.gameObject;

                // 전투에서 캐릭터 클릭 시 그 캐릭터의 패 보여줌
                if (gameObj.tag == "PlayerCharacter")
                {
                    CardManager.instance.highlighted_card = null;
                    CardManager.instance.Change_active_hand(gameObj.GetComponent<Character_Obj>().Character_index);
                }
                // 전투에서 카드 클릭 시
                else if (gameObj.tag == "SkillCard")
                {
                    // 아군 카드면
                    if (gameObj.GetComponent<card>().isEnemyCard == false)
                    {
                        // 카드 드래그 감지 시작
                        gameObj.GetComponent<card>().running_drag = StartCoroutine(gameObj.GetComponent<card>().detect_drag());

                        // 적 카드 강조 해제
                        BattleEventManager.Trigger_event("enemy_skill_card_deactivate");
                    }
                    // 적군 카드면
                    else
                    {
                        card card = gameObj.GetComponent<card>();
                        // 적 카드 강조 해제
                        if (card.state == card.current_mode.highlighted_enemy_card)
                        {
                            BattleEventManager.Trigger_event("enemy_skill_card_deactivate");
                        }
                    }
                }
                // 적 스킬 슬롯 클릭시
                else if (gameObj.tag == "enemySkillSlot") 
                {
                    // 아군 카드 강조 해제
                    CardManager.instance.highlighted_card = null;

                    // 모든 카드를 원래 order로 
                    CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                    // 카드 위치 계산 및 정렬
                    CardManager.instance.Aline_cards(CardManager.instance.active_index);
                }
                else // 그 이외의 것 클릭시
                {
                    CardManager.instance.highlighted_card = null;
                    CardManager.instance.Change_active_hand(-1);

                    // 모든 카드 정렬
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
                    {
                        CardManager.instance.Aline_cards(i);
                    }

                    // 적 카드 강조 해제
                    BattleEventManager.Trigger_event("enemy_skill_card_deactivate");

                    // 모든 카드를 원래 order로
                    CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                    // 카드 위치 계산 및 정렬
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

                // 스킬 사용 판정
                BattleCalcManager.instance.Calc_skill_use();
            }
        }
    }
}
