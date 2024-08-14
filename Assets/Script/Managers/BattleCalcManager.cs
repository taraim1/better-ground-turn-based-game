using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.NetworkInformation;
using Unity.Burst;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleCalcManager : Singletone<BattleCalcManager>
{
    // ī�� ���� ������ ���
    [SerializeField]
    card using_card; // �ַ� �÷��̾� ī��
    [SerializeField]
    card target_card; // �ַ� �� ī��

    // ī�� ���� ������ ���
    int using_card_power;
    int target_card_power;

    // �Ϲ� ���ݽ� ���
    [SerializeField]
    Character target_character;


    public bool isUsingCard() 
    {
        if (using_card == null)
        {
            return false;
        }
        return true;
    }

    // ����ϴ� ī�尪 �޾Ƽ� ���� �����ϴ� �޼ҵ�
    public void set_using_card(card using_card) 
    { 
        this.using_card = using_card;
        clear_target_card();
        clear_target_character();
    }

    // ���� ��� �޴� �޼ҵ�
    public void set_target<T>(T target)
    {

        if (target is card) 
        {
            target_character = null;
            target_card = target as card;
        }
        else if (target is Character)
        {
            target_card = null;
            target_character = target as Character;
        }
    }

    // Ÿ�� ���ִ� �޼ҵ�
    public void clear_target_card() 
    {
        target_card = null;
    }

    public void clear_target_character() 
    {
        target_character = null;
    }

    public void Clear_all() // ��� �ʱ�ȭ�ϴ� �޼ҵ�
    {
        clear_target_card();
        clear_target_character();
        using_card = null;
    }

    // ī�� ���� ����� �� ������
    public void Calc_skill_use()
    {
        // ��ų ��� �������� �Ǻ�
        if (!check_usable(using_card)) { return; }

        Character OwnerCharacter = using_card.owner.GetComponent<Character>();

        // ��ų ��� ���� ������ ���� �� ��� ���� �˻�
        if (target_character != null) 
        {
            if (!using_card.check_usable_coordinate(target_character.Coordinate)) 
            {
                Clear_all();
                return;
            }
        }
        if (target_card != null) 
        {
            if (!using_card.check_usable_coordinate(target_card.owner.Coordinate)) 
            {
                Clear_all();
                return;
            }
        }

        // �ڽſ��� ����ϴ� ��ų�̰� Ÿ���� �ڽ��̸� ���
        if (using_card.Data.IsSelfUsableOnly) 
        {
            if (target_character != null && target_character == OwnerCharacter) 
            {
                // ī�� ��� �� ȿ�� ����
                apply_skill_effect(using_card, skill_effect_timing.immediate, target_character);

                ActionManager.skill_used?.Invoke(OwnerCharacter, using_card.Data.Code);
                BattleManager.instance.reduce_cost(using_card.Data.Cost);
                using_card_power = UnityEngine.Random.Range(using_card.minpower, using_card.maxpower + 1);
                apply_direct_use_result(using_card, target_character, using_card_power);
            }
            Clear_all();
            return;
        }

        // �Ʊ����� ����ϴ� ��ų�̰� Ÿ���� �Ʊ��̸� ���
        if (using_card.Data.IsFriendlyOnly)
        {
            if (target_character != null && !target_character.check_enemy())
            {
                // ī�� ��� �� ȿ�� ����
                apply_skill_effect(using_card, skill_effect_timing.immediate, target_character);

                ActionManager.skill_used?.Invoke(OwnerCharacter, using_card.Data.Code);
                BattleManager.instance.reduce_cost(using_card.Data.Cost);
                using_card_power = UnityEngine.Random.Range(using_card.minpower, using_card.maxpower + 1);
                apply_direct_use_result(using_card, target_character, using_card_power);
            }
            Clear_all();
            return;
        }

        // ��ų vs ��ų�̸�
        if (target_card != null)
        {
            // ī�� ��� �� ȿ�� ����
            apply_skill_effect(using_card, skill_effect_timing.immediate, OwnerCharacter);
            apply_skill_effect(target_card, skill_effect_timing.immediate, target_card.owner.GetComponent<Character>());

            // �� ī�� ���� ����
            ActionManager.enemy_skillcard_deactivate?.Invoke();

            ActionManager.skill_used?.Invoke(OwnerCharacter, using_card.Data.Code);
            BattleManager.instance.reduce_cost(using_card.Data.Cost);
            using_card_power = UnityEngine.Random.Range(using_card.minpower, using_card.maxpower + 1);
            target_card_power = UnityEngine.Random.Range(target_card.minpower, target_card.maxpower + 1);
            apply_clash_result(using_card, target_card, using_card_power, target_card_power);
        }

        // ������ ���� ����̸�
        else if (target_character != null && target_character.check_enemy()) 
        {
            EnemyCharacter enemyCharacter = target_character as EnemyCharacter;

            // Ÿ�� ĳ�������� ���� ��ų�� ������ �ߵ� �� ��
            if (enemyCharacter.Remaining_skill_count != 0) 
            {
                Clear_all();
                return;
            }

            // ���� ��� ������ ī��� ī�� ���
            if (using_card.Data.IsDirectUsable) 
            {
                // ī�� ��� �� ȿ�� ����
                apply_skill_effect(using_card, skill_effect_timing.immediate, OwnerCharacter);

                // �� ī�� ���� ����
                ActionManager.enemy_skillcard_deactivate?.Invoke();

                ActionManager.skill_used?.Invoke(OwnerCharacter, using_card.Data.Code);
                BattleManager.instance.reduce_cost(using_card.Data.Cost);
                using_card_power = UnityEngine.Random.Range(using_card.minpower, using_card.maxpower + 1);
                apply_direct_use_result(using_card, target_character, using_card_power);
            }
        }

        Clear_all();
    }

    private void apply_skill_effect(card using_card, skill_effect_timing timing, Character target) // Ư�� Ÿ�̹��� ī�� Ư�� ȿ���� ���������
    {
        Character current_target;

        foreach (SkillEffect effect in using_card.Data.Effects)
        {
            if (effect.timing != timing) { continue; } // Ÿ�̹� �˻�

            // Ÿ�� ����
            if (effect.target == skill_effect_target.owner)
            {
                current_target = using_card.owner.GetComponent<Character>();
            }
            else 
            {
                current_target = target;
            }

            switch (effect.code)
            {
                case skill_effect_code.willpower_consumption:
                    // ���ŷ� ����
                    current_target.Damage_willpower(effect.parameters[0]);
                    break;

                case skill_effect_code.willpower_recovery:
                    // ���ŷ� ȸ��
                    current_target.Heal_willpower(effect.parameters[0]);
                    break;
                case skill_effect_code.ignition:
                    // Ÿ�ٿ��� ȭ�� �ο�
                    current_target.give_effect(character_effect_code.flame, character_effect_setType.add, effect.parameters[0]);
                    break;
                case skill_effect_code.fire_enchantment:
                    // Ÿ�ٿ��� ȭ�� ���� �ο�
                    current_target.give_effect(character_effect_code.ignition_attack, character_effect_setType.add, effect.parameters[0]);
                    break;
            }
        }

    }

    private bool check_usable(card card) // ��ų �� �� �ִ��� �Ǻ��ؼ� �� �� ������ true ��
    {
        if (using_card == null) { return false; }

        // �ڽ�Ʈ �����ϸ� �� ��
        if (BattleManager.instance.get_remaining_cost() < card.Data.Cost) { return false; }

        // Ư��ȿ�� �����ؼ� �� ���� �� �˻�
        Character owner_character = card.owner.GetComponent<Character>();
        foreach (SkillEffect effect in card.Data.Effects)
        {
            switch (effect.code)
            {
                case skill_effect_code.willpower_consumption:
                    // ���� ���ŷ��� �Ҹ�� ���ŷ� ���ϸ� �� ��
                    if (owner_character.Current_willpower <= effect.parameters[0]) { return false; }
                    break;
            }
        }

        return true;
    }

    // ���� �� ������ ��ų ���� ����
    public void Calc_enemy_turn_skill_use()
    {

        if (using_card.Data.RangeType == CardRangeType.limited) 
        {

            if (!using_card.check_usable_coordinate(target_character.Coordinate))
            {
                // ī�� ����
                using_card.Destroy_card();
                return;
            }
            
        }

        using_card_power = UnityEngine.Random.Range(using_card.minpower, using_card.maxpower + 1);
        apply_direct_use_result(using_card, target_character, using_card_power);
    }


    // ��ų ��� �� ���� �����ִ°�
    private void show_power_roll_result(card card, int power) 
    {
        card.owner.show_power_meter?.Invoke(power);
    }


    private void apply_direct_use_result(card using_card, Character target_character, int power) // ĳ���Ϳ� ���� ����� ī�� ��� ��������
    {
        if (!using_card.Data.DontShowPowerRollResult) { show_power_roll_result(using_card, power); } // ���� ������ �� �����ֱ�

        switch (using_card.Data.BehaviorType)
        {
            case ("����"):
                target_character.Damage_health(power);
                target_character.Damage_willpower(power);
                ActionManager.attacked?.Invoke(using_card.owner.GetComponent<Character>(), new List<Character>() { target_character });
                break;
        }

        // ī�� ��� �� ȿ�� ����
        apply_skill_effect(using_card, skill_effect_timing.after_use, target_character);

        // ī�� ����
        using_card.Destroy_card();
    }

    private void apply_clash_result(card using_card, card target_card, int power1, int power2) // ī�� �� �ָ� ĳ���Ϳ� ��� ��������
    {

        Character winner_char;
        Character loser_char;
        card winnerData;
        card loserData;
        int win_power;
        int lose_power;
        string win_behavior;
        string los_behavior;


        if (!using_card.Data.DontShowPowerRollResult) { show_power_roll_result(using_card, power1); }// ���� ������ �� �����ֱ�
        if (!target_card.Data.DontShowPowerRollResult) { show_power_roll_result(target_card, power2); }


        // ���º�
        if (power1 == power2)
        {
            // ī�� ����
            using_card.Destroy_card();
            target_card.Destroy_card();
            return;
        }

        // ��� ī�� �̱�
        if (power1 > power2)
        {
            winner_char = using_card.owner.GetComponent<Character>();
            loser_char = target_card.owner.GetComponent<Character>();

            winnerData = using_card;
            loserData = target_card;

            win_power = power1;
            lose_power = power2;
        }
        // Ÿ�� ī�� �̱�
        else 
        {
            winner_char = target_card.owner.GetComponent<Character>();
            loser_char = using_card.owner.GetComponent<Character>();

            winnerData = target_card;
            loserData = using_card;

            win_power = power2;
            lose_power = power1;
        }

        win_behavior = winnerData.Data.BehaviorType;
        los_behavior = loserData.Data.BehaviorType;

        // ī�� ��� �� ȿ�� ����
        apply_skill_effect(winnerData, skill_effect_timing.after_use, loser_char);

        // ī�� �ൿ Ÿ�Ժ� ��� ����
        switch (win_behavior) 
        {
            case ("����"):
                if (los_behavior == "����") { loser_char.Damage_health(win_power); loser_char.Damage_willpower(win_power); }
                else if (los_behavior == "���") { loser_char.Damage_health(win_power - lose_power); loser_char.Damage_willpower(win_power - lose_power); }
                else if (los_behavior == "ȸ��") { loser_char.Damage_health(win_power); loser_char.Damage_willpower(win_power); }
                ActionManager.attacked?.Invoke(winner_char, new List<Character>() { loser_char });
                break;

            case ("���"):
                if (los_behavior == "����") { }
                else if (los_behavior == "���") { loser_char.Damage_willpower(win_power); } // �� �� ���ŷ� ����
                else if (los_behavior == "ȸ��") { loser_char.Damage_willpower(win_power); }
                break;

            case ("ȸ��"):
                if (los_behavior == "����") { winner_char.Heal_willpower(win_power); } // �̱� �� ���ŷ� ȸ��
                else if (los_behavior == "���") { loser_char.Damage_willpower(win_power); }
                else if (los_behavior == "ȸ��") { loser_char.Damage_willpower(win_power); }
                break;
        }

        // ī�� ����

        using_card.Destroy_card();
        target_card.Destroy_card();

    }

    
    private void Update()
    {
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D character_collider = Physics2D.OverlapPoint(MousePos, LayerMask.GetMask("character"));
        if (character_collider != null)
        {
            // ĳ���� Ÿ���� ����
            if (character_collider.gameObject.tag == "PlayerCharacter" || character_collider.gameObject.tag == "EnemyCharacter")
            {
                set_target(character_collider.gameObject.GetComponent<Character>());
            }
        }

        // ������Ʈ ������ ���� �� ����, ���� ����� �÷��̾� ��ų ��� ������� �۵���, ��ų ��� ������
        if (Input.GetMouseButtonUp(0) && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase)
        {
            if (using_card != null)
            {
                // ��ų ��� ����
                Calc_skill_use();
            }
        }
    }

}
