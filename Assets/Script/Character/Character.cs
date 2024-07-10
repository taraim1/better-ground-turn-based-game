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
    public skillcard_code[] deck = new skillcard_code[6];
    public string SPUM_datapath;
    [SerializeField] private GameObject effect_container_prefab;
    [SerializeField] private GameObject effects_layoutGroup_obj;

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
    [DoNotSerialize]
    public GameObject skill_layoutGroup;
    [DoNotSerialize]
    public bool isPanic;
    private int remaining_panic_turn;
    [DoNotSerialize]
    public GameObject SPUM_unit_obj; // ĳ���� spum ������Ʈ
    [DoNotSerialize]
    public bool is_in_battle;
    private List<character_effect_container> effect_Containers = new List<character_effect_container>(); // ĳ���Ͱ� ���� ������ �ִ� ȿ���� �����̳� (���� / �����)

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

    public void Heal_health(int value) 
    {
        current_health += value;
        if (current_health > get_max_health_of_level(level)) 
        {
            current_health = get_max_health_of_level(level);
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

    public void Heal_willpower(int value)
    {
        current_willpower += value;
        if (current_willpower > get_max_willpower_of_level(level))
        {
            current_willpower = get_max_willpower_of_level(level);
        }
        // ���ŷ¹� ������Ʈ
        willpower_slider.slider.value = current_willpower;
        willpower_slider.value_tmp.text = current_willpower.ToString();
    }

    // ī�� ���� Ÿ�� ����, Ÿ�� ������ DetectingRay�� ����
    private void OnMouseExit()
    {
        if (is_in_battle)
        {
            BattleCalcManager.instance.clear_target_character();
        }
    }

    // ����/����� �߰��ϴ� �޼ҵ�
    public void give_effect(character_effect_code code, character_effect_setType type, int power) 
    {
        // �̹� ������ �ִ� ���� Ȯ��
        foreach (character_effect_container container in effect_Containers) 
        {
            if (container.Get_effect_code() == code) 
            {
                container.updateEffect(power, type); // ���� ���� or �߰�
                return;
            }
        }

        // ���°Ÿ� ���� �߰�
        GameObject obj = Instantiate(effect_container_prefab, effects_layoutGroup_obj.transform);
        character_effect_container obj_container = obj.GetComponent<character_effect_container>();
        effect_Containers.Add(obj_container);
        obj_container.Set(buffNdebuffManager.instance.get_effect(code, power), this);

        // ����Ʈ ���̾ƿ��׷� ������Ʈ
        effects_layoutGroup_obj.GetComponent<effectsLayoutGroup>().set_size(effect_Containers.Count);
    }

    // ���� / ����� (�����̳�) ���ִ� �޼ҵ�
    public void remove_effect(character_effect_container target) 
    {
        if (target != null)
        {
            effect_Containers.Remove(target);
            target.clear_delegate_and_destroy();
        }

        // ����Ʈ ���̾ƿ��׷� ������Ʈ
        effects_layoutGroup_obj.GetComponent<effectsLayoutGroup>().set_size(effect_Containers.Count);
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
        ActionManager.turn_start_phase += turn_start;
    }
    private void OnDisable()
    {
        ActionManager.turn_start_phase -= turn_start;
    }
}
