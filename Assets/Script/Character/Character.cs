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
        캐릭터 데이터가 저장되는 클래스
        캐릭터 데이터를 json 파일로 저장하는 역할도 함
    */

    public string name;
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
}
