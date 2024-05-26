using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;


[System.Serializable]
public class Character : MonoBehaviour
{
    /*
        캐릭터 데이터가 저장되는 클래스
        캐릭터 데이터를 json 파일로 저장하는 역할도 함
    */

    public string character_name;
    public string description;
    public CharacterManager.character_code code;
    public CharacterManager.enemy_code enemy_code;
    public int level;
    [SerializeField]
    private List<int> max_health = new List<int>() { 0, 30 };
    [SerializeField]
    private List<int> max_willpower = new List<int>() { 0, 15 };
    public bool is_character_unlocked;
    [SerializeField]
    public CardManager.skillcard_code[] deck = new CardManager.skillcard_code[6];
    public string SPUM_datapath;

    // 아래는 게임이 진행되면서 바뀌는 것들
    [DoNotSerialize]
    public int current_health;
    [DoNotSerialize]
    public int current_willpower;
    [DoNotSerialize]
    public GameObject health_bar;
    [DoNotSerialize]
    public GameObject willpower_bar;
    [DoNotSerialize]
    private UI_bar_slider health_slider;
    [DoNotSerialize]
    private UI_bar_slider willpower_slider;
    [DoNotSerialize]
    public skill_power_meter skill_power_meter;
    [DoNotSerialize]
    public bool isEnemyCharacter;
    [DoNotSerialize]
    // 전투시 캐릭터 오브젝트의 번호
    public int Character_index;
    [DoNotSerialize]
    public panic_sign panic_Sign;
    public bool isPanic;
    private int remaining_panic_turn;
    [DoNotSerialize]
    public GameObject SPUM_unit_obj; // 캐릭터 spum 오브젝트
    public int get_max_health_of_level(int level)
    {
        if (level > max_health.Count)
        {
            Debug.Log("오류: 입력된 레벨의 체력 데이터가 없습니다.");
            return -1;
        }
        else
        {
            return max_health[level];
        }
    }

    public int get_max_willpower_of_level(int level)
    {
        if (level > max_willpower.Count)
        {
            Debug.Log("오류: 입력된 레벨의 정신력 데이터가 없습니다.");
            return -1;
        }
        else
        {
            return max_willpower[level];
        }
    }

    public void Set_UI_bars() // 캐릭터의 체력바, 정신력바 초기세팅
    {
        health_slider = health_bar.GetComponent<UI_bar_slider>();
        willpower_slider = willpower_bar.GetComponent<UI_bar_slider>();
        health_slider.value_tmp.text = max_health[level].ToString();
        willpower_slider.value_tmp.text = max_willpower[level].ToString();
        health_slider.slider.maxValue = max_health[level];
        willpower_slider.slider.maxValue = max_willpower[level];
        health_slider.slider.value = health_slider.slider.maxValue;
        willpower_slider.slider.value = willpower_slider.slider.maxValue;
    }

    public void Damage_health(int value) // 체력 대미지 주는 메소드
    {
        current_health -= value;

        if (current_health <= 0) // 사망
        {
            CharacterManager.instance.kill_character(this);
        }

        // 체력바 업데이트
        health_slider.slider.value = current_health;
        health_slider.value_tmp.text = current_health.ToString();

    }

    public void Damage_willpower(int value) // 정신력 대미지 주는 메소드
    {

        current_willpower -= value;

        if (current_willpower <= 0) // 패닉
        {
            current_willpower = 0;
            isPanic = true;
            panic_Sign.show();
            remaining_panic_turn = 1;

            if (isEnemyCharacter) // 적이면 쓰는 스킬 다 제거
            {
                gameObject.GetComponent<EnemyAI>().clear_skills();
            }
        }

        // 정신력바 업데이트
        willpower_slider.slider.value = current_willpower;
        willpower_slider.value_tmp.text = current_willpower.ToString();
    }

    // 카드 사용시 타깃 설정
    private void OnMouseEnter()
    {
        if (BattleCalcManager.instance.IsUsingCard) 
        {
            BattleCalcManager.instance.set_target(this);
        }
    }
    private void OnMouseExit()
    {
        if (BattleCalcManager.instance.IsUsingCard)
        {
            BattleCalcManager.instance.clear_target();
        }
    }

    // 턴 시작시 발동되는 메소드
    private void turn_start() 
    {
        // 패닉 해제 or 패닉 턴 감소
        if (isPanic)
        {

            if (remaining_panic_turn == 0)
            {
                isPanic = false;
                panic_Sign.hide();
                Damage_willpower(-((get_max_willpower_of_level(level) + 1) / 2)); // 정신력 회복
            }
            else 
            {
                remaining_panic_turn -= 1;
            }

        }

        
    }

    private void Awake()
    {
        BattleEventManager.turn_start_phase += turn_start;
    }
    private void OnDisable()
    {
        BattleEventManager.turn_start_phase -= turn_start;
    }
}
