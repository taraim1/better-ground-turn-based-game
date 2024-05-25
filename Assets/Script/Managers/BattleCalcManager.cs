using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleCalcManager : Singletone<BattleCalcManager>
{
    // 카드 위력 판정시 사용
    card using_card; // 주로 플레이어 카드
    card target_card; // 주로 적 카드

    // 카드 위력 판정시 사용
    int using_card_power;
    int target_card_power;

    // 일방 공격시 사용
    Character target_character;

    // 남은 코스트 표시하는 거
    [SerializeField]
    cost_meter cost_Meter;

    // 카드 드래그중인지 저장
    private bool isDraggingCard = false;
    public bool IsDraggingCard { get { return isDraggingCard; } }

    // 카드 드래그 값 받아서 판정값 초기화하는 메소드
    public void Receive_dragging(card dragging_card) 
    { 
        using_card = dragging_card;
        target_card = null;
        target_character = null;
        isDraggingCard = true;

    }

    // 판정 대상 받는 메소드
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

    // 타겟 없애는 메소드
    public void clear_target() 
    {
        target_card = null;
        target_character = null;
    }

    // 카드 사용시 경우의 수 판정함
    public void Calc_skill_use()
    {
        // 카드 드래그 중이 아니면
        if (!isDraggingCard) { return; }
        // 코스트 부족하면
        if (cost_Meter.Current_cost < using_card.Card.cost) { return; }


        isDraggingCard = false;

        // 스킬 vs 스킬이면
        if (target_card != null)
        {
            // 판정 시작

            // 적 카드 강조 해제
            BattleEventManager.Trigger_event("enemy_skill_card_deactivate");

            BattleEventManager.Trigger_event("skill_used");
            cost_Meter.Current_cost = cost_Meter.Current_cost - using_card.Card.cost;
            using_card_power = Random.Range(using_card.minpower, using_card.maxpower + 1);
            target_card_power = Random.Range(target_card.minpower, target_card.maxpower + 1);
            apply_clash_result(using_card, target_card, using_card_power, target_card_power);
        }

        // 직접 사용이면
        else if (target_character != null) 
        {
            // 타겟 캐릭터한테 남은 스킬이 있으면 발동 안 됨
            if (target_character.gameObject.GetComponent<EnemyAI>().using_skill_Objects.Count != 0) 
            {
                return;
            }

            // 직접 사용 가능한 카드면 카드 사용
            if (using_card.Card.isDirectUsable) 
            {
                // 적 카드 강조 해제
                BattleEventManager.Trigger_event("enemy_skill_card_deactivate");

                BattleEventManager.Trigger_event("skill_used");
                cost_Meter.Current_cost = cost_Meter.Current_cost - using_card.Card.cost;
                using_card_power = Random.Range(using_card.minpower, using_card.maxpower + 1);
                apply_direct_use_result(using_card, target_character, using_card_power);
            }
        }

    }

    private void apply_direct_use_result(card using_card, Character target_character, int power) // 캐릭터에 직접 사용한 카드 결과 적용해줌
    {
        switch (using_card.Card.behavior_type)
        {
            case ("공격"):
                target_character.Damage_health(power);
                target_character.Damage_willpower(power);
                break;
        }

        // 카드 제거
        CardManager.instance.Destroy_card(using_card);
    }

    private void apply_clash_result(card using_card, card target_card, int power1, int power2) // 카드 둘 주면 캐릭터에 결과 적용해줌
    {

        Character winner_char;
        Character loser_char;
        card winner_card;
        card loser_card;
        int win_power;
        int lose_power;
        string win_behavior;
        string los_behavior;


        // 스킬 값 보여주기
        StartCoroutine(using_card.owner.GetComponent<Character>().skill_power_meter.Show(power1.ToString()));
        StartCoroutine(target_card.owner.GetComponent<Character>().skill_power_meter.Show(power2.ToString()));

        // 무승부
        if (power1 == power2)
        {
            Debug.Log("무승부");
            // 카드 제거
            CardManager.instance.Destroy_card(using_card);
            CardManager.instance.Destroy_card(target_card);
            return;
        }

        // 사용 카드 이김
        if (power1 > power2)
        {
            winner_char = using_card.owner.GetComponent<Character>();
            loser_char = target_card.owner.GetComponent<Character>();

            winner_card = using_card;
            loser_card = target_card;

            win_power = power1;
            lose_power = power2;
        }
        // 타겟 카드 이김
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

        // 카드 행동 타입별 결과 적용
        switch (win_behavior) 
        {
            case ("공격"):
                if (los_behavior == "공격") { loser_char.Damage_health(win_power); loser_char.Damage_willpower(win_power); }
                else if (los_behavior == "방어") { loser_char.Damage_health(win_power - lose_power); loser_char.Damage_willpower(win_power - lose_power); }
                else if (los_behavior == "회피") { loser_char.Damage_health(win_power); loser_char.Damage_willpower(win_power); }
                break;

            case ("방어"):
                if (los_behavior == "공격") { }
                else if (los_behavior == "방어") { loser_char.Damage_willpower(win_power - lose_power); } // 진 쪽 정신력 감소
                else if (los_behavior == "회피") { loser_char.Damage_willpower(win_power - lose_power); }
                break;

            case ("회피"):
                if (los_behavior == "공격") { winner_char.Damage_willpower(lose_power - win_power); } // 이긴 쪽 정신력 회복
                else if (los_behavior == "방어") { loser_char.Damage_willpower(win_power - lose_power); }
                else if (los_behavior == "회피") { loser_char.Damage_willpower(win_power - lose_power); }
                break;
        }

        // 카드 제거
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
