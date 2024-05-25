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
    public int current_health;
    [SerializeField]
    private List<int> max_willpower = new List<int>() { 0, 15 };
    public int current_willpower;
    public bool is_character_unlocked;
    [SerializeField]
    public CardManager.skillcard_code[] deck = new CardManager.skillcard_code[6];

    // �Ʒ��� ������ ����Ǹ鼭 �ٲ�� �͵�
    [DoNotSerialize]
    public GameObject health_bar;
    [DoNotSerialize]
    public GameObject willpower_bar;
    [DoNotSerialize]
    private UI_bar_slider health_slider;
    [DoNotSerialize]
    private UI_bar_slider willpower_slider;
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
        health_slider.max_value_tmp.text = max_health[level].ToString();
        willpower_slider.max_value_tmp.text = max_willpower[level].ToString();
        health_slider.slider.maxValue = max_health[level];
        willpower_slider.slider.maxValue = max_willpower[level];
        health_slider.slider.value = health_slider.slider.maxValue;
        willpower_slider.slider.value = willpower_slider.slider.maxValue;
    }

    public void Damage(int value) // ����� �ִ� �޼ҵ�
    {
        current_health -= value;
        if (current_health <= 0) 
        {
            current_health = 0;
            print("ĳ���� ���");
        }
        current_willpower -= value;
        if (current_willpower <= 0)
        {
            current_willpower = 0;
            print("ĳ���� �д�");
        }

        // ü�¹�, ���ŷ¹� ������Ʈ
        health_slider.slider.value = current_health;
        willpower_slider.slider.value = current_willpower;

    }
}
