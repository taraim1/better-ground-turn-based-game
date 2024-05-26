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
        ĳ���� �����Ͱ� ����Ǵ� Ŭ����
        ĳ���� �����͸� json ���Ϸ� �����ϴ� ���ҵ� ��
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

    // �Ʒ��� ������ ����Ǹ鼭 �ٲ�� �͵�
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
    // ������ ĳ���� ������Ʈ�� ��ȣ
    public int Character_index;
    [DoNotSerialize]
    public panic_sign panic_Sign;
    public bool isPanic;
    private int remaining_panic_turn;
    [DoNotSerialize]
    public GameObject SPUM_unit_obj; // ĳ���� spum ������Ʈ
    public int get_max_health_of_level(int level)
    {
        if (level > max_health.Count)
        {
            Debug.Log("����: �Էµ� ������ ü�� �����Ͱ� �����ϴ�.");
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
            Debug.Log("����: �Էµ� ������ ���ŷ� �����Ͱ� �����ϴ�.");
            return -1;
        }
        else
        {
            return max_willpower[level];
        }
    }

    public void Set_UI_bars() // ĳ������ ü�¹�, ���ŷ¹� �ʱ⼼��
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

    public void Damage_health(int value) // ü�� ����� �ִ� �޼ҵ�
    {
        current_health -= value;

        if (current_health <= 0) // ���
        {
            CharacterManager.instance.kill_character(this);
        }

        // ü�¹� ������Ʈ
        health_slider.slider.value = current_health;
        health_slider.value_tmp.text = current_health.ToString();

    }

    public void Damage_willpower(int value) // ���ŷ� ����� �ִ� �޼ҵ�
    {

        current_willpower -= value;

        if (current_willpower <= 0) // �д�
        {
            current_willpower = 0;
            isPanic = true;
            panic_Sign.show();
            remaining_panic_turn = 1;

            if (isEnemyCharacter) // ���̸� ���� ��ų �� ����
            {
                gameObject.GetComponent<EnemyAI>().clear_skills();
            }
        }

        // ���ŷ¹� ������Ʈ
        willpower_slider.slider.value = current_willpower;
        willpower_slider.value_tmp.text = current_willpower.ToString();
    }

    // ī�� ���� Ÿ�� ����
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

    // �� ���۽� �ߵ��Ǵ� �޼ҵ�
    private void turn_start() 
    {
        // �д� ���� or �д� �� ����
        if (isPanic)
        {

            if (remaining_panic_turn == 0)
            {
                isPanic = false;
                panic_Sign.hide();
                Damage_willpower(-((get_max_willpower_of_level(level) + 1) / 2)); // ���ŷ� ȸ��
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
