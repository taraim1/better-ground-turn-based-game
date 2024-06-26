using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffNdebuffManager : Singletone<buffNdebuffManager>
{
    [SerializeField] private characterEffectSO characterEffectSO;



    private character_effect_timing get_character_effect_timing_by_code(character_effect_code code) 
    {
        switch (code) 
        {
            case character_effect_code.flame:
                return character_effect_timing.turn_ended;
            case character_effect_code.ignition_attack:
                return character_effect_timing.after_attack;
        }

        return character_effect_timing.turn_started;
    }

    private character_effect_power_reduce_timing get_reduce_timing_by_code(character_effect_code code)
    {
        switch (code)
        {
            case character_effect_code.flame:
                return character_effect_power_reduce_timing.used;
            case character_effect_code.ignition_attack:
                return character_effect_power_reduce_timing.turn_ended;
        }

        return character_effect_power_reduce_timing.turn_started;
    }

    private character_effect_target_type get_target_type_by_code(character_effect_code code)
    {
        switch (code)
        {
            case character_effect_code.flame:
                return character_effect_target_type.owner;
            case character_effect_code.ignition_attack:
                return character_effect_target_type.attack_target;
        }

        return character_effect_target_type.owner;
    }

    public character_effect get_effect(character_effect_code code, int power) 
    {
        character_effect tmp;
        tmp.code = code;
        tmp.power = power;
        tmp.apply_timing = get_character_effect_timing_by_code(code);
        tmp.power_reduce_timing = get_reduce_timing_by_code(code);
        tmp.target_type = get_target_type_by_code(code);
        return tmp;
    }

    public Sprite get_icon_by_code(character_effect_code code) 
    {
        return characterEffectSO.SpritesDict[code];
    }
}
