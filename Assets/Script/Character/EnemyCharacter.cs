using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyCharacter : Character
{
    /* 
     * 필드 및 접근용 메소드들
     */
    private int remaining_skill_count = 0;
    public int Remaining_skill_count { get { return remaining_skill_count; } }

    private List<card> reserved_cards;

    public Action<card> skillcard_reserved;

    private EnemyAI AI;
    public void SetAI(EnemyAI AI)
    {
        this.AI = AI;
    }

    /* 
    * 메소드
    */

    public override bool check_enemy() { return true; }

    private void OnSkillReserved(card card)
    {
        remaining_skill_count += 1;
    }

    private void OnCardDestoryed(card card)
    {
        if (card.owner == this && remaining_skill_count >= 0)
        {
            remaining_skill_count -= 1;
        }
    }

    private void OnEnemySkillSettingPhase()
    {
        isMovable = true; // 이 줄은 테스트용으로 넣은 것, 나중에 지워야 함
        AI.Reset();
        AI.Move();
        reserved_cards = AI.Get_skills_for_current_turn();

        foreach (card card in reserved_cards)
        {
            skillcard_reserved?.Invoke(card);
            BattleManager.instance.enemy_cards.Add(card);
        }
        ActionManager.enemy_skill_set_complete?.Invoke();
    }

    private void Awake()
    {
        skillcard_reserved += OnSkillReserved;
        ActionManager.card_destroyed += OnCardDestoryed;
        ActionManager.enemy_skill_setting_phase += OnEnemySkillSettingPhase;
    }

    private void OnDestroy()
    {
        skillcard_reserved -= OnSkillReserved;
        ActionManager.card_destroyed -= OnCardDestoryed;
        ActionManager.enemy_skill_setting_phase -= OnEnemySkillSettingPhase;

    }
}
