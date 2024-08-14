using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyCharacter : Character
{
    /* 
     * �ʵ� �� ���ٿ� �޼ҵ��
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
    * �޼ҵ�
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
        isMovable = true; // �� ���� �׽�Ʈ������ ���� ��, ���߿� ������ ��
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
