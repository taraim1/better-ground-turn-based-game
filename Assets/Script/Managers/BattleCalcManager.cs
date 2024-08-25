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
        // Ÿ���� �ִ��� �Ǻ�
        if (target_character == null && target_card == null) { Clear_all(); return; }

        // ��Ȯ�� Ÿ�� ã��
        if (target_character == null)
        {
            target_character = target_card.owner;
        }

        // ��ų ��� �������� �Ǻ�
        if (!check_usable(using_card)) { Clear_all(); return; }

        // �� ī�� ���� ����
        ActionManager.enemy_skillcard_deactivate?.Invoke();

        // ��ų ���
        if (!using_card.owner.check_enemy()) 
        {
            BattleManager.instance.reduce_cost(using_card.Data.Cost);
        }

        // ���� ���� or ���� �����
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

        // ī�� ���� �� ����ȭ
        using_card.Destroy_card();
        if (target_card != null) 
        {
            target_card.Destroy_card();
        }
        Clear_all();
    }


    private bool check_usable(card card) // ��ų �� �� �ִ��� �Ǻ��ؼ� �� �� ������ true ��
    {
        if (using_card == null) { return false; }

        // �Ʊ� ī���� ��� �ڽ�Ʈ �����ϸ� �� ��
        if (BattleManager.instance.get_remaining_cost() < card.Data.Cost && !card.owner.check_enemy()) { return false; }

        // ��ų ��� ���� ���� �˻�
        if (!using_card.check_usable_coordinate(target_character.Coordinate))
        {
            return false;
        }


        // ���� ����� ���
        if (target_card == null)
        {
            if (!card.Data.IsDirectUsable) return false;

            // ������ ���� ����� ��� �����ִ� ��ų�� �ִٸ� ��� �Ұ�
            if (target_character is EnemyCharacter)
            {
                EnemyCharacter enemyCharacter = (EnemyCharacter)target_character;
                if (enemyCharacter.Remaining_skill_count != 0)
                {
                    return false;
                }
            }
        }
        // ���� �����ؾ� �ϴ� ���, ��ų Ÿ�� �˻�
        else
        {
            CardBehaviorType target_card_behavior_type = target_card.Data.BehaviorType;
            if (target_card_behavior_type == CardBehaviorType.etc) return false; // ��Ÿ ī��� ���� ���� �Ұ�

            switch (card.Data.BehaviorType) 
            {
                case CardBehaviorType.attack: // ���� ī��� ����, ���, ȸ�ǿ� ��� ����
                    break;
                case CardBehaviorType.defend: // ��� ī��� �ڽ��� �����ϴ� ���� ī�忡�� ��� ����
                    if (target_card_behavior_type == CardBehaviorType.defend) return false;
                    if (target_card_behavior_type == CardBehaviorType.dodge) return false;
                    if (target_card.target != card.owner) return false;
                    break;
                case CardBehaviorType.dodge: // ȸ�� ī��� �ڽ��� �����ϴ� ���� ī�忡�� ��� ����
                    if (target_card_behavior_type == CardBehaviorType.defend) return false;
                    if (target_card_behavior_type == CardBehaviorType.dodge) return false;
                    if (target_card.target != card.owner) return false;
                    break;
                case CardBehaviorType.etc:
                    return false;
            }
        }

        // Ư��ȿ�� �� ��ų ��� �����ؼ� �� ���� �� �˻�
        return card.check_card_usable(target_card, target_character);

    }


    // ���� ���
    private void direct_use(card card, Character target_character) 
    {
        // ������ ���
        if (card.Data.BehaviorType == CardBehaviorType.attack) 
        {
            int power = card.PowerRoll();
            target_character.Damage_health(power);
            target_character.Damage_willpower(power);
            ActionManager.attacked?.Invoke(card.owner, new List<Character>() { target_character });
        }

        card.OnDirectUsed?.Invoke(null, target_character);
    }

    // ���� ����
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


        // ���º�
        if (power1 == power2)
        {   
            return;
        }

        // ��� ī�� �̱�
        if (power1 > power2)
        {
            winner_char = using_card.owner;
            loser_char = target_card.owner;

            winner_card = using_card;
            loser_card = target_card;

            win_power = power1;
            lose_power = power2;
        }
        // Ÿ�� ī�� �̱�
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

        // ī�� �ൿ Ÿ�Ժ� ��� ����
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
                if (los_behavior == CardBehaviorType.attack) { winner_char.Heal_willpower(win_power); } // �̱� �� ���ŷ� ȸ��
                break;
        }

        
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
