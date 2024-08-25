using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectManager : Singletone<CharacterEffectManager>
{
    [SerializeField] private characterEffectSO characterEffectSO;
    private characterEffectsDictionary effectDict;



    public character_effect make_effect(character_effect_code code, int power, Character character, character_effect_container container) 
    {
        character_effect product;

        switch (code) 
        {
            case character_effect_code.flame:
                product = new Flame(code, power, character, container);
                break;
            case character_effect_code.ignition_attack:
                product = new Ignition_attack(code, power, character, container);
                break;
            default:
                product = new Flame(code, power, character, container);
                break;
        }

        return product;
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
