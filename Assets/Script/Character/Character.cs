using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class Character
{
    private string character_name;
    public string Character_name { get { return character_name; } set { character_name = value; } }

    private string description;
    public string Description { get { return description; }}

    private int character_code;
    public int Character_code { get { return character_code; }}

    private int max_health;
    public int Max_health { get { return max_health; } set { max_health = value; } }

    private int current_health;
    public int Current_health { get { return current_health; } set { current_health = value; } }

    private int max_willpower;
    public int Max_willpower { get { return max_willpower; } set { max_willpower = value; } }

    private int current_willpower;
    public int Current_willpower { get { return current_willpower; } set { current_willpower = value; } }

    private int number_of_skill_slots;
    public int Number_of_skill_slots { get { return number_of_skill_slots; } set { number_of_skill_slots = value; } }

    private int min_speed;
    public int Min_speed { get { return min_speed; } set { min_speed = value; } }

    private int max_speed;
    public int Max_speed { get { return max_speed; } set { max_speed = value; } }


}
