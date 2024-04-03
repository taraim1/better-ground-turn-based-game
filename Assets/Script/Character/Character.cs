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


    public string get_character_name() //ĳ���� �̸� �������� �޼ҵ�
    {
        return character_name;
    }

    public void set_character_name(string name) // ĳ���� �̸� ���� �޼ҵ�
    { 
        character_name = name;
    }

    public int get_character_int_property(CharacterManager.character_int_properties property) 
        // ĳ������ ��Ʈ �Ӽ��� �������� �޼ҵ�
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
    // ĳ������ ��Ʈ �Ӽ��� �����ϴ� �޼ҵ�
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
