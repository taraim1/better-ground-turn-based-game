using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleCalcManager : Singletone<BattleCalcManager>
{
    // ī�� ���� ������ ���
    card using_card; // �ַ� �÷��̾� ī��
    card target_card; // �ַ� �� ī��

    // ī�� ���� ������ ���
    int using_card_power;
    int target_card_power;

    // �Ϲ� ���ݽ� ���
    Character target_character;

    // ���� �ڽ�Ʈ ǥ���ϴ� ��
    [SerializeField]
    cost_meter cost_Meter;

    // ī�� �巡�������� ����
    private bool isDraggingCard = false;
    public bool IsDraggingCard { get { return isDraggingCard; } }

    // ī�� �巡�� �� �޾Ƽ� ������ �ʱ�ȭ�ϴ� �޼ҵ�
    public void Receive_dragging(card dragging_card) 
    { 
        using_card = dragging_card;
        target_card = null;
        target_character = null;
        isDraggingCard = true;

    }

    // ���� ��� �޴� �޼ҵ�
    public void Receive_target<T>(T target)
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
    public void clear_target() 
    {
        target_card = null;
        target_character = null;
    }

    // ī�� ���� ����� �� ������
    public void Calc_skill_use()
    {
        // ī�� �巡�� ���� �ƴϸ�
        if (!isDraggingCard) { return; }
        // �ڽ�Ʈ �����ϸ�
        if (cost_Meter.Current_cost < using_card.Card.cost) { return; }


        isDraggingCard = false;

        // ��ų vs ��ų�̸�
        if (target_card != null)
        {
            // ���� ����

            // �� ī�� ���� ����
            BattleEventManager.Trigger_event("enemy_skill_card_deactivate");

            BattleEventManager.Trigger_event("skill_used");
            cost_Meter.Current_cost = cost_Meter.Current_cost - using_card.Card.cost;
            using_card_power = Random.Range(using_card.minpower, using_card.maxpower + 1);
            target_card_power = Random.Range(target_card.minpower, target_card.maxpower + 1);
            apply_clash_result(using_card, target_card, using_card_power, target_card_power);
        }

        // ���� ����̸�
        else if (target_character != null) 
        {
            // Ÿ�� ĳ�������� ���� ��ų�� ������ �ߵ� �� ��
            if (target_character.gameObject.GetComponent<EnemyAI>().using_skill_Objects.Count != 0) 
            {
                return;
            }

            // ���� ��� ������ ī��� ī�� ���
            if (using_card.Card.isDirectUsable) 
            {
                // �� ī�� ���� ����
                BattleEventManager.Trigger_event("enemy_skill_card_deactivate");

                BattleEventManager.Trigger_event("skill_used");
                cost_Meter.Current_cost = cost_Meter.Current_cost - using_card.Card.cost;
                using_card_power = Random.Range(using_card.minpower, using_card.maxpower + 1);
                apply_direct_use_result(using_card, target_character, using_card_power);
            }
        }

    }

    private void apply_direct_use_result(card using_card, Character target_character, int power) // ĳ���Ϳ� ���� ����� ī�� ��� ��������
    {
        switch (using_card.Card.behavior_type)
        {
            case ("����"):
                target_character.Damage_health(power);
                target_character.Damage_willpower(power);
                break;
        }

        // ī�� ����
        CardManager.instance.Destroy_card(using_card);
    }

    private void apply_clash_result(card using_card, card target_card, int power1, int power2) // ī�� �� �ָ� ĳ���Ϳ� ��� ��������
    {

        Character winner_char;
        Character loser_char;
        card winner_card;
        card loser_card;
        int win_power;
        int lose_power;
        string win_behavior;
        string los_behavior;


        // ��ų �� �����ֱ�
        StartCoroutine(using_card.owner.GetComponent<Character>().skill_power_meter.Show(power1.ToString()));
        StartCoroutine(target_card.owner.GetComponent<Character>().skill_power_meter.Show(power2.ToString()));

        // ���º�
        if (power1 == power2)
        {
            Debug.Log("���º�");
            // ī�� ����
            CardManager.instance.Destroy_card(using_card);
            CardManager.instance.Destroy_card(target_card);
            return;
        }

        // ��� ī�� �̱�
        if (power1 > power2)
        {
            winner_char = using_card.owner.GetComponent<Character>();
            loser_char = target_card.owner.GetComponent<Character>();

            winner_card = using_card;
            loser_card = target_card;

            win_power = power1;
            lose_power = power2;
        }
        // Ÿ�� ī�� �̱�
        else 
        {
            winner_char = target_card.owner.GetComponent<Character>();
            loser_char = using_card.owner.GetComponent<Character>();

            winner_card = target_card;
            loser_card = using_card;

            win_power = power2;
            lose_power = power1;
        }

        win_behavior = winner_card.Card.behavior_type;
        los_behavior = loser_card.Card.behavior_type;

        // ī�� �ൿ Ÿ�Ժ� ��� ����
        switch (win_behavior) 
        {
            case ("����"):
                if (los_behavior == "����") { loser_char.Damage_health(win_power); loser_char.Damage_willpower(win_power); }
                else if (los_behavior == "���") { loser_char.Damage_health(win_power - lose_power); loser_char.Damage_willpower(win_power - lose_power); }
                else if (los_behavior == "ȸ��") { loser_char.Damage_health(win_power); loser_char.Damage_willpower(win_power); }
                break;

            case ("���"):
                if (los_behavior == "����") { }
                else if (los_behavior == "���") { loser_char.Damage_willpower(win_power - lose_power); } // �� �� ���ŷ� ����
                else if (los_behavior == "ȸ��") { loser_char.Damage_willpower(win_power - lose_power); }
                break;

            case ("ȸ��"):
                if (los_behavior == "����") { winner_char.Damage_willpower(lose_power - win_power); } // �̱� �� ���ŷ� ȸ��
                else if (los_behavior == "���") { loser_char.Damage_willpower(win_power - lose_power); }
                else if (los_behavior == "ȸ��") { loser_char.Damage_willpower(win_power - lose_power); }
                break;
        }

        // ī�� ����
        CardManager.instance.Destroy_card(using_card);
        CardManager.instance.Destroy_card(target_card);
    }

    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Battle")
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
