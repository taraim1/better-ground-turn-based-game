using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum character_effect_code 
{ 
    flame
}

public enum character_effect_timing
{
    turn_started,
    card_used,
    turn_ended,
    get_hit
}

public enum character_effect_setType 
{ 
    add,
    replace
}

[System.Serializable]
public struct character_effect 
{ 
    public character_effect_code code;
    public character_effect_timing timing;
    public int power;
}