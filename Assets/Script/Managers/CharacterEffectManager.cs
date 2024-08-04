using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectManager : Singletone<CharacterEffectManager>
{
    [SerializeField] private characterEffectSO characterEffectSO;
    private characterEffectsDictionary effectDict;



    public character_effect get_effect(character_effect_code code, int power) 
    {
        character_effect tmp;
        tmp.code = code;
        tmp.power = power;
        tmp.apply_timing = effectDict[code].Timing;
        tmp.power_reduce_timing = effectDict[code].PowerReduceTiming;
        tmp.target_type = effectDict[code].TargetType;
        return tmp;
    }

    public Sprite get_icon_by_code(character_effect_code code) 
    {
        return effectDict[code].Sprite;
    }

    private void Start()
    {
        effectDict = characterEffectSO.CharacterEffectDict;
    }
}
