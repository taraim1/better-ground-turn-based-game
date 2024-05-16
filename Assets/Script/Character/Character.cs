using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;


[System.Serializable]
public class Character
{
    /*
        ĳ���� �����Ͱ� ����Ǵ� Ŭ����
        ĳ���� �����͸� json ���Ϸ� �����ϴ� ���ҵ� ��
    */

    public string name;
    public string description;
    public CharacterManager.character_code code;
    public int level;
    [SerializeField]
    private List<int> max_health = new List<int>() { 0, 30 };
    public int current_health;
    [SerializeField]
    private List<int> max_willpower = new List<int>() { 0, 15 };
    public int current_willpower;
    public bool is_character_unlocked;

    public int get_max_health_of_level(int level)
    {
        if (level > max_health.Count)
        {
            Debug.Log("����: �Էµ� ������ ü�� �����Ͱ� �����ϴ�.");
            return -1;
        }
        else
        {
            return max_health[level - 1];
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
            return max_willpower[level - 1];
        }
    }
}
