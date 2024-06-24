using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "charcterEffectSO", menuName = "Scriptable_Objects_characterEffectSO")]
public class characterEffectSO : ScriptableObject
{
    [SerializeField] private characterEffectSpritesDictionary SpritesDict;
}

[System.Serializable]
public class characterEffectSpritesDictionary : SerializableDictionary<character_effect_code, Sprite> { }
