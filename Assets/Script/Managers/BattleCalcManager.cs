using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.NetworkInformation;
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

    // 남은 코스트 표시하는 거
    [SerializeField]
    cost_meter cost_Meter;

    // 카드 사용중인지 저장
    private bool isUsingCard = false;
    public bool IsUsingCard { get { return isUsingCard; } }

    // 사용하는 카드값 받아서 판정 시작하는 메소드
    public void set_using_card(card usinging_card) 
    { 
        using_card = usinging_card;
        clear_target_card();
        clear_target_character();
        isUsingCard = true;
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
        isUsingCard = false;
    }

    // 카드 사용시 경우의 수 판정함
    public void Calc_skill_use()
    {
        // 카드 드래그 중이 아니면
        if (!isUsingCard) { return; }
        // 코스트 부족하면
        if (cost_Meter.Current_cost < using_card.Card.cost) { return; }


        isUsingCard = false;

        // 스킬 vs 스킬이면
        if (target_card != null)
        {


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

    // 적이 턴 끝나고 스킬 사용시 판정
    public void Calc_enemy_turn_skill_use()
    {
        using_card_power = Random.Range(using_card.minpower, using_card.maxpower + 1);
        apply_direct_use_result(using_card, target_character, using_card_power);
    }

    private void apply_direct_use_result(card using_card, Character target_character, int power) // 캐릭터에 직접 사용한 카드 결과 적용해줌
    {
        skill_power_meter PWmeter = using_card.owner.GetComponent<Character>().skill_power_meter;

        // 같은 캐릭터가 텀을 적게 두고 사용시 현재 돌아가는 show를 멈춰서 너무 빨리 숫자가 사라지는 현상을 해결
        if (PWmeter.running_show != null) 
        {
            PWmeter.StopCoroutine(PWmeter.running_show);
        }

        // 스킬 값 보여주기
        PWmeter.running_show = StartCoroutine(using_card.owner.GetComponent<Character>().skill_power_meter.Show(power.ToString()));

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

        skill_power_meter PWmeter1 = using_card.owner.GetComponent<Character>().skill_power_meter;
        skill_power_meter PWmeter2 = target_card.owner.GetComponent<Character>().skill_power_meter;

        // 같은 캐릭터가 텀을 적게 두고 사용시 현재 돌아가는 show를 멈춰서 너무 빨리 숫자가 사라지는 현상을 해결
        if (PWmeter1.running_show != null) { PWmeter1.StopCoroutine(PWmeter1.running_show); }
        if (PWmeter2.running_show != null) { PWmeter2.StopCoroutine(PWmeter2.running_show); }

        // 스킬 값 보여주기
        PWmeter1.running_show = StartCoroutine(using_card.owner.GetComponent<Character>().skill_power_meter.Show(power1.ToString()));
        PWmeter2.running_show = StartCoroutine(target_card.owner.GetComponent<Character>().skill_power_meter.Show(power2.ToString()));

        // 무승부
        if (power1 == power2)
        {
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
                else if (los_behavior == "방어") { loser_char.Damage_willpower(win_power); } // 진 쪽 정신력 감소
                else if (los_behavior == "회피") { loser_char.Damage_willpower(win_power); }
                break;

            case ("회피"):
                if (los_behavior == "공격") { winner_char.Damage_willpower(-win_power); } // 이긴 쪽 정신력 회복
                else if (los_behavior == "방어") { loser_char.Damage_willpower(win_power); }
                else if (los_behavior == "회피") { loser_char.Damage_willpower(win_power); }
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
