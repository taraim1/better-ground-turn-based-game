using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum character_effect_code 
{ 
    flame,
    ignition_attack
}

public enum character_effect_timing
{
    turn_started,
    card_used,
    turn_ended,
    get_health_damage,
    after_attack
}

public enum character_effect_power_reduce_timing
{
    turn_started,
    turn_ended,
    used
}


public enum character_effect_setType 
{ 
    add,
    replace
}

public enum character_effect_target_type
{
    owner,
    attack_target
}

[System.Serializable]
public struct character_effect 
{ 
    public character_effect_code code;
    public character_effect_timing apply_timing;
    public character_effect_power_reduce_timing power_reduce_timing;
    public character_effect_target_type target_type;
    public int power;
}