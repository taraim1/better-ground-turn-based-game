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

        // 스킬카드 레이어 클릭 감지
        Collider2D skill_collider = Physics2D.OverlapPoint(MousePos, LayerMask.GetMask("SkillCard"));
        if (skill_collider != null)
        {
            return skill_collider.gameObject;
        }

        // 스킬 슬롯 클릭 감지
        Collider2D skill_slot_collider = Physics2D.OverlapPoint(MousePos, LayerMask.GetMask("skillslot"));
        if (skill_slot_collider != null) 
        {
            return skill_slot_collider.gameObject;
        }

        // 캐릭터 클릭 감지
        Collider2D chracter_collider = Physics2D.OverlapPoint(MousePos, LayerMask.GetMask("character"));
        if (chracter_collider != null)
        {
            return chracter_collider.gameObject;
        }

        // 그 외 클릭 감지
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
            // 아군 캐릭터 클릭 시 
            case "PlayerCharacter":
                Character character = obj.GetComponent<Character>();
                // 그 캐릭터의 패 보여줌
                CardManager.instance.clear_highlighted_card();
                CardManager.instance.Change_active_hand(character.data.Character_index);
                CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                // 드래그 감지 시작
                character.data.running_drag = StartCoroutine(character.detect_drag());
                break;

            // 카드 클릭 시
            case "SkillCard":
                card card = obj.GetComponent<card>();
                // 아군 카드이면
                if (card.isEnemyCard == false)
                {
                    // 카드 드래그 감지 시작
                    card.running_drag = StartCoroutine(card.detect_drag(false));

                    // 적 카드 강조 해제
                    ActionManager.enemy_skill_card_deactivate?.Invoke();
                }
                // 적군 카드면
                else
                {
                    // 적 카드 강조 해제
                    if (card.state == card.current_mode.highlighted_enemy_card)
                    {
                        ActionManager.enemy_skill_card_deactivate?.Invoke();
                    }
                }
                break;

            // 카드 특수효과 설명 클릭시
            case "skillDescription":
                card = obj.GetComponent<card_description>().get_target();

                // 아군 카드이면
                if (card.isEnemyCard == false)
                {
                    // 카드 드래그 감지 시작
                    card.running_drag = StartCoroutine(card.detect_drag(true));

                    // 적 카드 강조 해제
                    ActionManager.enemy_skill_card_deactivate?.Invoke();
                }
                // 적군 카드면
                else
                {
                    // 적 카드 강조 해제
                    if (card.state == card.current_mode.highlighted_enemy_card)
                    {
                        ActionManager.enemy_skill_card_deactivate?.Invoke();
                    }
                }
                break;

            // 캐릭터 특수효과 (버프 / 디버프) 설명 클릭시
            case "effectDescription":
                break; // 아무 것도 안 함

            // 적 스킬 슬롯 클릭시
            case "enemySkillSlot":
                // 아군 카드 강조 해제
                CardManager.instance.clear_highlighted_card();

                // 모든 카드를 원래 order로 
                CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                // 카드 위치 계산 및 정렬
                CardManager.instance.Align_cards(CardManager.instance.active_index);
                break;

            // 그 외의 것 클릭시
            default:
                CardManager.instance.clear_highlighted_card();
                CardManager.instance.Change_active_hand(-1);

                // 모든 카드 정렬
                for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
                {
                    CardManager.instance.Align_cards(i);
                }

                // 적 카드 강조 해제
                ActionManager.enemy_skill_card_deactivate?.Invoke();

                // 모든 카드를 원래 order로
                CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                // 카드 위치 계산 및 정렬
                CardManager.instance.Align_cards(CardManager.instance.active_index);
                break;

        }     
    }

    void Update()
    {
        // 전투 종료 감지
        if (isBattleEnded) { return; }

        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D character_collider = Physics2D.OverlapPoint(MousePos, LayerMask.GetMask("character"));
        if (character_collider != null) 
        {
            // 캐릭터 타게팅 설정
            if (character_collider.gameObject.tag == "PlayerCharacter" || character_collider.gameObject.tag == "EnemyCharacter") 
            { 
                BattleCalcManager.instance.set_target(character_collider.gameObject.GetComponent<Character>());
            }
        }



        // 클릭 or 터치 감지, 현재 페이즈가 플레이어 스킬 사용 페이즈여만 작동됨
        if (Input.GetMouseButtonDown(0) && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase) 
        {
            GameObject gameObj = check_clicked_obj(); // 클릭된 오브젝트 감지

            if (gameObj != null) 
            {
                apply_click_result_by_tag(gameObj);
            }
        }

        // 오브젝트 눌렀다 떼는 거 감지, 현재 페이즈가 플레이어 스킬 사용 페이즈여만 작동됨
        if (Input.GetMouseButtonUp(0) && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase) 
        {
            Collider2D clicked_collider = Physics2D.OverlapPoint(MousePos);
            
            if (clicked_collider != null)
            {
                // 스킬 사용 판정
                BattleCalcManager.instance.Calc_skill_use();
            }
        }
    }
}
