using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterEffect;

public class CharacterEffectManager : Singletone<CharacterEffectManager>
{
    [SerializeField] private characterEffectSO characterEffectSO;
    private characterEffectsDictionary effectDict;

    public Sprite get_icon_by_code(character_effect_code code) 
    {
        return effectDict[code].Sprite;
    }

    private void Start()
    {
        effectDict = characterEffectSO.CharacterEffectDict;
    }
}
