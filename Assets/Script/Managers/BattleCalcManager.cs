using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.NetworkInformation;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleCalcManager : Singletone<BattleCalcManager>
{

    [SerializeField]
    card using_card;
    [SerializeField]
    card target_card;
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
        // 타겟이 있는지 판별
        if (target_character == null && target_card == null) { Clear_all(); return; }

        // 정확한 타겟 찾기
        if (target_character == null)
        {
            target_character = target_card.owner;
        }

        // 스킬 사용 가능한지 판별
        if (!check_usable(using_card)) { Clear_all(); return; }

        // 적 카드 강조 해제
        ActionManager.enemy_skillcard_deactivate?.Invoke();

        // 스킬 사용
        if (!using_card.owner.check_enemy()) 
        {
            BattleManager.instance.reduce_cost(using_card.Data.Cost);
        }

        // 위력 판정 or 직접 사용함
        if (target_card != null)
        {
            clash(using_card, target_card);
        }
        else 
        {
            direct_use(using_card, target_character);
        }
        ActionManager.skill_used?.Invoke(using_card.owner, using_card.Data.Code);
        using_card.OnUsed?.Invoke(target_card, target_character);

        // 카드 제거 및 포기화
        using_card.Destroy_card();
        if (target_card != null) 
        {
            target_card.Destroy_card();
        }
        Clear_all();
    }


    private bool check_usable(card card) // 스킬 쓸 수 있는지 판별해서 쓸 수 있으면 true 줌
    {
        if (using_card == null) { return false; }

        // 아군 카드인 경우 코스트 부족하면 못 씀
        if (BattleManager.instance.get_remaining_cost() < card.Data.Cost && !card.owner.check_enemy()) { return false; }

        // 스킬 사용 가능 범위 검사
        if (!using_card.check_usable_coordinate(target_character.Coordinate))
        {
            return false;
        }


        // 직접 사용인 경우
        if (target_card == null)
        {
            if (!card.Data.IsDirectUsable) return false;

            // 적에게 직접 사용인 경우 남아있는 스킬이 있다면 사용 불가
            if (target_character is EnemyCharacter)
            {
                EnemyCharacter enemyCharacter = (EnemyCharacter)target_character;
                if (enemyCharacter.Remaining_skill_count != 0)
                {
                    return false;
                }
            }
        }
        // 위력 판정해야 하는 경우, 스킬 타입 검사
        else
        {
            CardBehaviorType target_card_behavior_type = target_card.Data.BehaviorType;
            if (target_card_behavior_type == CardBehaviorType.etc) return false; // 기타 카드는 위력 판정 불가

            switch (card.Data.BehaviorType) 
            {
                case CardBehaviorType.attack: // 공격 카드는 공격, 방어, 회피에 사용 가능
                    break;
                case CardBehaviorType.defend: // 방어 카드는 자신을 공격하는 공격 카드에만 사용 가능
                    if (target_card_behavior_type == CardBehaviorType.defend) return false;
                    if (target_card_behavior_type == CardBehaviorType.dodge) return false;
                    if (target_card.target != card.owner) return false;
                    break;
                case CardBehaviorType.dodge: // 회피 카드는 자신을 공격하는 공격 카드에만 사용 가능
                    if (target_card_behavior_type == CardBehaviorType.defend) return false;
                    if (target_card_behavior_type == CardBehaviorType.dodge) return false;
                    if (target_card.target != card.owner) return false;
                    break;
                case CardBehaviorType.etc:
                    return false;
            }
        }

        // 특수효과 및 스킬 대상 관련해서 못 쓰는 거 검사
        return card.check_card_usable(target_card, target_character);

    }


    // 직접 사용
    private void direct_use(card card, Character target_character) 
    {
        // 공격인 경우
        if (card.Data.BehaviorType == CardBehaviorType.attack) 
        {
            int power = card.PowerRoll();
            target_character.Damage_health(power);
            target_character.Damage_willpower(power);
            ActionManager.attacked?.Invoke(card.owner, new List<Character>() { target_character });
        }

        card.OnDirectUsed?.Invoke(null, target_character);
    }

    // 위력 판정
    private void clash(card using_card, card target_card)
    {

        Character winner_char;
        Character loser_char;
        card winner_card;
        card loser_card;
        int win_power;
        int lose_power;
        CardBehaviorType win_behavior;
        CardBehaviorType los_behavior;

        int power1 = using_card.PowerRoll();
        int power2 = target_card.PowerRoll();


        // 무승부
        if (power1 == power2)
        {   
            return;
        }

        // 사용 카드 이김
        if (power1 > power2)
        {
            winner_char = using_card.owner;
            loser_char = target_card.owner;

            winner_card = using_card;
            loser_card = target_card;

            win_power = power1;
            lose_power = power2;
        }
        // 타겟 카드 이김
        else 
        {
            winner_char = target_card.owner;
            loser_char = using_card.owner;

            winner_card = target_card;
            loser_card = using_card;

            win_power = power2;
            lose_power = power1;
        }

        win_behavior = winner_card.Data.BehaviorType;
        los_behavior = loser_card.Data.BehaviorType;

        winner_card.OnClashWin?.Invoke(loser_card, loser_char);

        // 카드 행동 타입별 결과 적용
        switch (win_behavior) 
        {
            case (CardBehaviorType.attack):
                if (los_behavior == CardBehaviorType.attack) { loser_char.Damage_health(win_power); loser_char.Damage_willpower(win_power); }
                else if (los_behavior == CardBehaviorType.defend) { loser_char.Damage_health(win_power - lose_power); loser_char.Damage_willpower(win_power - lose_power); }
                else if (los_behavior == CardBehaviorType.dodge) { loser_char.Damage_health(win_power); loser_char.Damage_willpower(win_power); }
                ActionManager.attacked?.Invoke(winner_char, new List<Character>() { loser_char });
                break;

            case (CardBehaviorType.defend):
                break;

            case (CardBehaviorType.dodge):
                if (los_behavior == CardBehaviorType.attack) { winner_char.Heal_willpower(win_power); } // 이긴 쪽 정신력 회복
                break;
        }

        
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
