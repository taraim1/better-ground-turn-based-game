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
    // 카드 위력 판정시 사용
    [SerializeField]
    card using_card; // 주로 플레이어 카드
    [SerializeField]
    card target_card; // 주로 적 카드

    // 카드 위력 판정시 사용
    int using_card_power;
    int target_card_power;

    // 일방 공격시 사용
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

    // 사용하는 카드값 받아서 판정 시작하는 메소드
    public void set_using_card(card using_card) 
    { 
        this.using_card = using_card;
        clear_target_card();
        clear_target_character();
    }

    // 판정 대상 받는 메소드
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

    // 타겟 없애는 메소드
    public void clear_target_card() 
    {
        target_card = null;
    }

    public void clear_target_character() 
    {
        target_character = null;
    }

    public void Clear_all() // 모두 초기화하는 메소드
    {
        clear_target_card();
        clear_target_character();
        using_card = null;
    }

    // 카드 사용시 경우의 수 판정함
    public void Calc_skill_use()
    {
        // 스킬 사용 가능한지 판별
        if (!check_usable(using_card)) { return; }

        Character OwnerCharacter = using_card.owner.GetComponent<Character>();

        // 스킬 사용 가능 범위가 있을 시 사용 가능 검사
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

        // 자신에게 사용하는 스킬이고 타겟이 자신이면 사용
        if (using_card.Data.IsSelfUsableOnly) 
        {
            if (target_character != null && target_character == OwnerCharacter) 
            {
                // 카드 사용 시 효과 적용
                apply_skill_effect(using_card, skill_effect_timing.immediate, target_character);

                ActionManager.skill_used?.Invoke(OwnerCharacter, using_card.Data.Code);
                BattleManager.instance.reduce_cost(using_card.Data.Cost);
                using_card_power = UnityEngine.Random.Range(using_card.minpower, using_card.maxpower + 1);
                apply_direct_use_result(using_card, target_character, using_card_power);
            }
            Clear_all();
            return;
        }

        // 아군에게 사용하는 스킬이고 타겟이 아군이면 사용
        if (using_card.Data.IsFriendlyOnly)
        {
            if (target_character != null && !target_character.check_enemy())
            {
                // 카드 사용 시 효과 적용
                apply_skill_effect(using_card, skill_effect_timing.immediate, target_character);

                ActionManager.skill_used?.Invoke(OwnerCharacter, using_card.Data.Code);
                BattleManager.instance.reduce_cost(using_card.Data.Cost);
                using_card_power = UnityEngine.Random.Range(using_card.minpower, using_card.maxpower + 1);
                apply_direct_use_result(using_card, target_character, using_card_power);
            }
            Clear_all();
            return;
        }

        // 스킬 vs 스킬이면
        if (target_card != null)
        {
            // 카드 사용 시 효과 적용
            apply_skill_effect(using_card, skill_effect_timing.immediate, OwnerCharacter);
            apply_skill_effect(target_card, skill_effect_timing.immediate, target_card.owner.GetComponent<Character>());

            // 적 카드 강조 해제
            ActionManager.enemy_skillcard_deactivate?.Invoke();

            ActionManager.skill_used?.Invoke(OwnerCharacter, using_card.Data.Code);
            BattleManager.instance.reduce_cost(using_card.Data.Cost);
            using_card_power = UnityEngine.Random.Range(using_card.minpower, using_card.maxpower + 1);
            target_card_power = UnityEngine.Random.Range(target_card.minpower, target_card.maxpower + 1);
            apply_clash_result(using_card, target_card, using_card_power, target_card_power);
        }

        // 적에게 직접 사용이면
        else if (target_character != null && target_character.check_enemy()) 
        {
            EnemyCharacter enemyCharacter = target_character as EnemyCharacter;

            // 타겟 캐릭터한테 남은 스킬이 있으면 발동 안 됨
            if (enemyCharacter.Remaining_skill_count != 0) 
            {
                Clear_all();
                return;
            }

            // 직접 사용 가능한 카드면 카드 사용
            if (using_card.Data.IsDirectUsable) 
            {
                // 카드 사용 시 효과 적용
                apply_skill_effect(using_card, skill_effect_timing.immediate, OwnerCharacter);

                // 적 카드 강조 해제
                ActionManager.enemy_skillcard_deactivate?.Invoke();

                ActionManager.skill_used?.Invoke(OwnerCharacter, using_card.Data.Code);
                BattleManager.instance.reduce_cost(using_card.Data.Cost);
                using_card_power = UnityEngine.Random.Range(using_card.minpower, using_card.maxpower + 1);
                apply_direct_use_result(using_card, target_character, using_card_power);
            }
        }

        Clear_all();
    }

    private void apply_skill_effect(card using_card, skill_effect_timing timing, Character target) // 특정 타이밍의 카드 특수 효과를 실행시켜줌
    {
        Character current_target;

        foreach (SkillEffect effect in using_card.Data.Effects)
        {
            if (effect.timing != timing) { continue; } // 타이밍 검사

            // 타겟 설정
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
                    // 정신력 감소
                    current_target.Damage_willpower(effect.parameters[0]);
                    break;

                case skill_effect_code.willpower_recovery:
                    // 정신력 회복
                    current_target.Heal_willpower(effect.parameters[0]);
                    break;
                case skill_effect_code.ignition:
                    // 타겟에게 화염 부여
                    current_target.give_effect(character_effect_code.flame, character_effect_setType.add, effect.parameters[0]);
                    break;
                case skill_effect_code.fire_enchantment:
                    // 타겟에게 화염 공격 부여
                    current_target.give_effect(character_effect_code.ignition_attack, character_effect_setType.add, effect.parameters[0]);
                    break;
            }
        }

    }

    private bool check_usable(card card) // 스킬 쓸 수 있는지 판별해서 쓸 수 있으면 true 줌
    {
        if (using_card == null) { return false; }

        // 코스트 부족하면 못 씀
        if (BattleManager.instance.get_remaining_cost() < card.Data.Cost) { return false; }

        // 특수효과 관련해서 못 쓰는 거 검사
        Character owner_character = card.owner.GetComponent<Character>();
        foreach (SkillEffect effect in card.Data.Effects)
        {
            switch (effect.code)
            {
                case skill_effect_code.willpower_consumption:
                    // 현재 정신력이 소모될 정신력 이하면 못 씀
                    if (owner_character.Current_willpower <= effect.parameters[0]) { return false; }
                    break;
            }
        }

        return true;
    }

    // 적이 턴 끝나고 스킬 사용시 판정
    public void Calc_enemy_turn_skill_use()
    {

        if (using_card.Data.RangeType == CardRangeType.limited) 
        {

            if (!using_card.check_usable_coordinate(target_character.Coordinate))
            {
                // 카드 제거
                using_card.Destroy_card();
                return;
            }
            
        }

        using_card_power = UnityEngine.Random.Range(using_card.minpower, using_card.maxpower + 1);
        apply_direct_use_result(using_card, target_character, using_card_power);
    }


    // 스킬 사용 시 위력 보여주는거
    private void show_power_roll_result(card card, int power) 
    {
        card.owner.show_power_meter?.Invoke(power);
    }


    private void apply_direct_use_result(card using_card, Character target_character, int power) // 캐릭터에 직접 사용한 카드 결과 적용해줌
    {
        if (!using_card.Data.DontShowPowerRollResult) { show_power_roll_result(using_card, power); } // 위력 판정한 거 보여주기

        switch (using_card.Data.BehaviorType)
        {
            case ("공격"):
                target_character.Damage_health(power);
                target_character.Damage_willpower(power);
                ActionManager.attacked?.Invoke(using_card.owner.GetComponent<Character>(), new List<Character>() { target_character });
                break;
        }

        // 카드 사용 시 효과 적용
        apply_skill_effect(using_card, skill_effect_timing.after_use, target_character);

        // 카드 제거
        using_card.Destroy_card();
    }

    private void apply_clash_result(card using_card, card target_card, int power1, int power2) // 카드 둘 주면 캐릭터에 결과 적용해줌
    {

        Character winner_char;
        Character loser_char;
        card winnerData;
        card loserData;
        int win_power;
        int lose_power;
        string win_behavior;
        string los_behavior;


        if (!using_card.Data.DontShowPowerRollResult) { show_power_roll_result(using_card, power1); }// 위력 판정한 거 보여주기
        if (!target_card.Data.DontShowPowerRollResult) { show_power_roll_result(target_card, power2); }


        // 무승부
        if (power1 == power2)
        {
            // 카드 제거
            using_card.Destroy_card();
            target_card.Destroy_card();
            return;
        }

        // 사용 카드 이김
        if (power1 > power2)
        {
            winner_char = using_card.owner.GetComponent<Character>();
            loser_char = target_card.owner.GetComponent<Character>();

            winnerData = using_card;
            loserData = target_card;

            win_power = power1;
            lose_power = power2;
        }
        // 타겟 카드 이김
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

        // 카드 사용 시 효과 적용
        apply_skill_effect(winnerData, skill_effect_timing.after_use, loser_char);

        // 카드 행동 타입별 결과 적용
        switch (win_behavior) 
        {
            case ("공격"):
                if (los_behavior == "공격") { loser_char.Damage_health(win_power); loser_char.Damage_willpower(win_power); }
                else if (los_behavior == "방어") { loser_char.Damage_health(win_power - lose_power); loser_char.Damage_willpower(win_power - lose_power); }
                else if (los_behavior == "회피") { loser_char.Damage_health(win_power); loser_char.Damage_willpower(win_power); }
                ActionManager.attacked?.Invoke(winner_char, new List<Character>() { loser_char });
                break;

            case ("방어"):
                if (los_behavior == "공격") { }
                else if (los_behavior == "방어") { loser_char.Damage_willpower(win_power); } // 진 쪽 정신력 감소
                else if (los_behavior == "회피") { loser_char.Damage_willpower(win_power); }
                break;

            case ("회피"):
                if (los_behavior == "공격") { winner_char.Heal_willpower(win_power); } // 이긴 쪽 정신력 회복
                else if (los_behavior == "방어") { loser_char.Damage_willpower(win_power); }
                else if (los_behavior == "회피") { loser_char.Damage_willpower(win_power); }
                break;
        }

        // 카드 제거

        using_card.Destroy_card();
        target_card.Destroy_card();

    }

    
    private void Update()
    {
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D character_collider = Physics2D.OverlapPoint(MousePos, LayerMask.GetMask("character"));
        if (character_collider != null)
        {
            // 캐릭터 타게팅 설정
            if (character_collider.gameObject.tag == "PlayerCharacter" || character_collider.gameObject.tag == "EnemyCharacter")
            {
                set_target(character_collider.gameObject.GetComponent<Character>());
            }
        }

        // 오브젝트 눌렀다 떼는 거 감지, 현재 페이즈가 플레이어 스킬 사용 페이즈여만 작동됨, 스킬 사용 판정용
        if (Input.GetMouseButtonUp(0) && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase)
        {
            if (using_card != null)
            {
                // 스킬 사용 판정
                Calc_skill_use();
            }
        }
    }

}
