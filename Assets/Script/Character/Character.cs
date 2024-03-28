using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class Character
{
    private string character_name;
    private int max_health;
    private int current_health;
    private int max_willpower;
    private int current_willpower;
    private int number_of_skill_slots;


    public string get_character_name() //캐릭터 이름 가져오는 메소드
    {
        return character_name;
    }

    public void set_character_name(string name) // 캐릭터 이름 설정 메소드
    { 
        character_name = name;
    }

    public int get_character_int_property(CharacterManager.character_int_properties property) 
        // 캐릭터의 인트 속성값 가져오는 메소드
    {

        if (property == CharacterManager.character_int_properties.max_health)
        {
            return max_health;
        }
        else if (property == CharacterManager.character_int_properties.current_health)
        {
            return current_health;
        }
        else if (property == CharacterManager.character_int_properties.max_willpower)
        {
            return max_willpower;
        }
        else if (property == CharacterManager.character_int_properties.current_willpower)
        {
            return current_willpower;
        }
        else if (property == CharacterManager.character_int_properties.number_of_skill_slots)
        {
            return number_of_skill_slots;
        }
        else { return 0; }

        }

    public void set_character_int_property(CharacterManager.character_int_properties property, int value)
    // 캐릭터의 인트 속성값 설정하는 메소드
    {

        if (property == CharacterManager.character_int_properties.max_health)
        {
            max_health = value;
        }
        else if (property == CharacterManager.character_int_properties.current_health)
        {
            current_health = value;
        }
        else if (property == CharacterManager.character_int_properties.max_willpower)
        {
            max_willpower = value;
        }
        else if (property == CharacterManager.character_int_properties.current_willpower)
        {
            current_willpower = value;
        }
        else if (property == CharacterManager.character_int_properties.number_of_skill_slots)
        {
            number_of_skill_slots = value;
        }

    }


}
