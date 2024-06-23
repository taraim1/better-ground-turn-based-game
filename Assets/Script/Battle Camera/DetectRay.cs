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
        BattleEventManager.battle_ended += OnBattleEnd;
    }

    private void OnDestroy()
    {
        BattleEventManager.battle_ended -= OnBattleEnd;
    }

    private void OnBattleEnd(bool victory) 
    {
        isBattleEnded = true;
    }
    void Update()
    {
        // 전투 종료 감지
        if (isBattleEnded) { return; }

        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = Physics2D.OverlapPoint(MousePos);

        if (collider != null) 
        {
            // 캐릭터 타게팅 설정
            if (collider.gameObject.tag == "PlayerCharacter" || collider.gameObject.tag == "EnemyCharacter") 
            { 
                BattleCalcManager.instance.set_target(collider.gameObject.GetComponent<Character>());
            }
        }


        // 클릭 or 터치
        if (Input.GetMouseButtonDown(0)) 
        { 
            Collider2D clicked_collider = Physics2D.OverlapPoint(MousePos);

            // 오브젝트 누르는 거 감지, 현재 페이즈가 플레이어 스킬 사용 페이즈여만 작동됨
            if (clicked_collider != null && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase) 
            {
                GameObject gameObj = clicked_collider.gameObject;

                // 전투에서 캐릭터 클릭 시 그 캐릭터의 패 보여줌
                if (gameObj.tag == "PlayerCharacter")
                {
                    CardManager.instance.clear_highlighted_card();
                    CardManager.instance.Change_active_hand(gameObj.GetComponent<Character>().Character_index);
                    CardManager.instance.Set_origin_order(CardManager.instance.active_index);
                }
                // 전투에서 카드 클릭 시
                else if (gameObj.tag == "SkillCard")
                {
                    card card = gameObj.GetComponent<card>();
                    // 아군 카드이면
                    if (card.isEnemyCard == false)
                    {
                        // 카드 드래그 감지 시작
                        card.running_drag = StartCoroutine(card.detect_drag());

                        // 적 카드 강조 해제
                        BattleEventManager.enemy_skill_card_deactivate?.Invoke();
                    }
                    // 적군 카드면
                    else
                    {
                        // 적 카드 강조 해제
                        if (card.state == card.current_mode.highlighted_enemy_card)
                        {
                            BattleEventManager.enemy_skill_card_deactivate?.Invoke();
                        }
                    }
                }
                // 카드 특수효과 설명 클릭시
                else if (gameObj.tag == "skillDescription")
                {
                    card card = gameObj.GetComponent<card_description>().get_target();

                    // 아군 카드이면
                    if (card.isEnemyCard == false)
                    {
                        // 카드 드래그 감지 시작
                        card.running_drag = StartCoroutine(card.detect_drag());

                        // 적 카드 강조 해제
                        BattleEventManager.enemy_skill_card_deactivate?.Invoke();
                    }
                    // 적군 카드면
                    else
                    {
                        // 적 카드 강조 해제
                        if (card.state == card.current_mode.highlighted_enemy_card)
                        {
                            BattleEventManager.enemy_skill_card_deactivate?.Invoke();
                        }
                    }
                }
                // 적 스킬 슬롯 클릭시
                else if (gameObj.tag == "enemySkillSlot")
                {
                    // 아군 카드 강조 해제
                    CardManager.instance.clear_highlighted_card();

                    // 모든 카드를 원래 order로 
                    CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                    // 카드 위치 계산 및 정렬
                    CardManager.instance.Aline_cards(CardManager.instance.active_index);
                }
                else // 그 이외의 것 클릭시
                {
                    CardManager.instance.clear_highlighted_card();
                    CardManager.instance.Change_active_hand(-1);

                    // 모든 카드 정렬
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
                    {
                        CardManager.instance.Aline_cards(i);
                    }

                    // 적 카드 강조 해제
                    BattleEventManager.enemy_skill_card_deactivate?.Invoke();

                    // 모든 카드를 원래 order로
                    CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                    // 카드 위치 계산 및 정렬
                    CardManager.instance.Aline_cards(CardManager.instance.active_index);
                }
            }
        }

        // 오브젝트 눌렀다 떼는 거 감지, 현재 페이즈가 플레이어 스킬 사용 페이즈여만 작동됨
        if (Input.GetMouseButtonUp(0) && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase) 
        {
            Collider2D clicked_collider = Physics2D.OverlapPoint(MousePos);
            
            if (clicked_collider != null)
            {
                GameObject gameObj = clicked_collider.gameObject;

                // 스킬 사용 판정
                BattleCalcManager.instance.Calc_skill_use();
            }
        }
    }
}
