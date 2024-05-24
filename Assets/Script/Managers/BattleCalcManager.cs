using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleCalcManager : Singletone<BattleCalcManager>
{
    // ī�� ���� ������ ���
    card card1;
    card card2;

    // ī�� ���� ������ ���
    int card1_power;
    int card2_power;

    // ���� �ڽ�Ʈ ǥ���ϴ� ��
    [SerializeField]
    cost_meter cost_Meter;

    // ī�� �巡�������� ����
    private bool isDraggingCard = false;
    public bool IsDraggingCard { get { return isDraggingCard; } }

    // ī�� �巡�� �� �޾Ƽ� ������ �ʱ�ȭ�ϴ� �޼ҵ�
    public void Receive_dragging(card dragging_card) 
    { 
        card1 = dragging_card;
        card2 = null;
        isDraggingCard = true;

    }

    // ���� ��� ī�尪 �޴� �޼ҵ�
    public void Receive_target(card target_card)
    {
        card2 = target_card;

    }

    // ī�� ���� �����ϰ� �����ϴ� �޼ҵ�
    public void Calc_skill_clash()
    {
        // ī�� �巡�� ���� �ƴϸ�
        if (!isDraggingCard) { return; }

        if (card2 == null) { return; }

        // ���� ����
        BattleEventManager.Trigger_event("skill_clash_started");
        isDraggingCard = false;
        cost_Meter.Current_cost = cost_Meter.Current_cost - card1.Card.cost;
        card1_power = Random.Range(card1.minpower, card1.maxpower + 1);
        card2_power = Random.Range(card2.minpower, card2.maxpower + 1);
        apply_clash_result(card1, card2, card1_power, card2_power);

    }

    private void apply_clash_result(card card1, card card2, int power1, int power2) // ī�� �� �ָ� ĳ���Ϳ� ��� ��������
    {

        Character winner_char;
        Character loser_char;
        card winner_card;
        card loser_card;
        int win_power;
        int lose_power;
        string win_behavior;
        string los_behavior;

        // ���º�
        if (power1 == power2)
        {
            Debug.Log("���º�");
            // ī�� ����
            CardManager.instance.Destroy_card(card1);
            CardManager.instance.Destroy_card(card2);
            return;
        }

        // ī�� 1 �̱�
        if (power1 > power2)
        {
            winner_char = card1.owner.GetComponent<Character>();
            loser_char = card2.owner.GetComponent<Character>();

            winner_card = card1;
            loser_card = card2;

            win_power = power1;
            lose_power = power2;
        }
        // ī�� 2 �̱�
        else 
        {
            winner_char = card2.owner.GetComponent<Character>();
            loser_char = card1.owner.GetComponent<Character>();

            winner_card = card2;
            loser_card = card1;

            win_power = power2;
            lose_power = power1;
        }

        win_behavior = winner_card.Card.behavior_type;
        los_behavior = loser_card.Card.behavior_type;

        // ī�� �ൿ Ÿ�Ժ� ��� ����
        switch (win_behavior) 
        {
            case ("����"):
                if (los_behavior == "����") { loser_char.Damage(win_power); }
                else if (los_behavior == "����") { loser_char.Damage(win_power - lose_power); }
                else if (los_behavior == "ȸ��") { loser_char.Damage(win_power); }
                
                break;
            case ("����"):
                if (los_behavior == "����") { loser_char.Damage(win_power); }
                else if (los_behavior == "����") { }
                else if (los_behavior == "ȸ��") { }
                break;
            case ("ȸ��"):
                if (los_behavior == "����") { }
                else if (los_behavior == "����") { }
                else if (los_behavior == "ȸ��") { }
                break;
        }

        // ī�� ����
        CardManager.instance.Destroy_card(card1);
        CardManager.instance.Destroy_card(card2);
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
