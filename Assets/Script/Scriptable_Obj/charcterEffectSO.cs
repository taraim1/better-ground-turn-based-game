using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "charcterEffectSO", menuName = "Scriptable_Objects_characterEffectSO")]
public class characterEffectSO : ScriptableObject
{
    [SerializeField] public characterEffectsDictionary CharacterEffectDict;
}

[System.Serializable]
public class characterEffectData 
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private character_effect_timing timing;
    [SerializeField] private character_effect_power_reduce_timing power_reduce_timing;
    [SerializeField] private character_effect_target_type target_type;

    public Sprite Sprite => sprite;
    public character_effect_timing Timing => timing;
    public character_effect_power_reduce_timing PowerReduceTiming => power_reduce_timing;
    public character_effect_target_type TargetType => target_type;

}

[System.Serializable]
public class characterEffectsDictionary : SerializableDictionary<character_effect_code, characterEffectData> { }
