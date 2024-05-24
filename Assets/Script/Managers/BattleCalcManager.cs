using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleCalcManager : Singletone<BattleCalcManager>
{
    // 카드 위력 판정시 사용
    card card1;
    card card2;

    // 카드 위력 판정시 사용
    int card1_power;
    int card2_power;

    // 남은 코스트 표시하는 거
    [SerializeField]
    cost_meter cost_Meter;

    // 카드 드래그중인지 저장
    private bool isDraggingCard = false;
    public bool IsDraggingCard { get { return isDraggingCard; } }

    // 카드 드래그 값 받아서 판정값 초기화하는 메소드
    public void Receive_dragging(card dragging_card) 
    { 
        card1 = dragging_card;
        card2 = null;
        isDraggingCard = true;

    }

    // 판정 대상 카드값 받는 메소드
    public void Receive_target(card target_card)
    {
        card2 = target_card;

    }

    // 카드 승패 판정하고 적용하는 메소드
    public void Calc_skill_clash()
    {
        // 카드 드래그 중이 아니면
        if (!isDraggingCard) { return; }

        if (card2 == null) { return; }

        // 판정 시작
        BattleEventManager.Trigger_event("skill_clash_started");
        isDraggingCard = false;
        cost_Meter.Current_cost = cost_Meter.Current_cost - card1.Card.cost;
        card1_power = Random.Range(card1.minpower, card1.maxpower + 1);
        card2_power = Random.Range(card2.minpower, card2.maxpower + 1);
        apply_clash_result(card1, card2, card1_power, card2_power);

    }

    private void apply_clash_result(card card1, card card2, int power1, int power2) // 카드 둘 주면 캐릭터에 결과 적용해줌
    {

        Character winner_char;
        Character loser_char;
        card winner_card;
        card loser_card;
        int win_power;
        int lose_power;
        string win_behavior;
        string los_behavior;

        // 무승부
        if (power1 == power2)
        {
            Debug.Log("무승부");
            // 카드 제거
            CardManager.instance.Destroy_card(card1);
            CardManager.instance.Destroy_card(card2);
            return;
        }

        // 카드 1 이김
        if (power1 > power2)
        {
            winner_char = card1.owner.GetComponent<Character>();
            loser_char = card2.owner.GetComponent<Character>();

            winner_card = card1;
            loser_card = card2;

            win_power = power1;
            lose_power = power2;
        }
        // 카드 2 이김
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

        // 카드 행동 타입별 결과 적용
        switch (win_behavior) 
        {
            case ("공격"):
                if (los_behavior == "공격") { loser_char.Damage(win_power); }
                else if (los_behavior == "수비") { loser_char.Damage(win_power - lose_power); }
                else if (los_behavior == "회피") { loser_char.Damage(win_power); }
                
                break;
            case ("수비"):
                if (los_behavior == "공격") { loser_char.Damage(win_power); }
                else if (los_behavior == "수비") { }
                else if (los_behavior == "회피") { }
                break;
            case ("회피"):
                if (los_behavior == "공격") { }
                else if (los_behavior == "수비") { }
                else if (los_behavior == "회피") { }
                break;
        }

        // 카드 제거
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
