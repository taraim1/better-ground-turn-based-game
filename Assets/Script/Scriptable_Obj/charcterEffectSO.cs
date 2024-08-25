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
    [SerializeField] private string name;
    [SerializeField] private string description;

    public Sprite Sprite => sprite;
    public string Name => name;
    public string Description => description;

}

[System.Serializable]
public class characterEffectsDictionary : SerializableDictionary<character_effect_code, characterEffectData> { }
