using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Character_Data_", menuName = "Scriptable Object/Chracter Data", order = int.MaxValue)]
public class Charater_Data_scriptable_Object : ScriptableObject
{
    [SerializeField]
    private string character_name;
    public string Character_name {  get { return character_name; } }

    [SerializeField]
    private string description;
    public string Description { get { return description; } }

    [SerializeField]
    private string character_position;
    public string Character_position { get { return character_position; } }

    [SerializeField]
    private string character_class;
    public string Character_class { get { return character_class; } }

    [SerializeField]
    private int character_code;
    public int Caracter_code { get { return character_code; } }

    [SerializeField]
    private int base_max_health;
    public int Base_max_health { get { return base_max_health; } }

    [SerializeField]
    private int base_max_willpower;
    public int Base_max_willpower { get { return base_max_willpower; } }

    [SerializeField]
    private int number_of_skill_slots;
    public int Number_of_skill_slots { get { return number_of_skill_slots; } }

    [SerializeField]
    private int min_speed;
    public int Min_speed { get { return min_speed; }}

    [SerializeField]
    private int max_speed;
    public int Max_speed { get { return max_speed; }}
}
