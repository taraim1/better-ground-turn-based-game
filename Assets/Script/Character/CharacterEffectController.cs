using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterEffect;

public class CharacterEffectController : MonoBehaviour
{
    [SerializeField] private GameObject effectContainer_prefab;

    private Character character;
    private character_base character_base;
    private List<character_effect> effects = new List<character_effect>();

    private void Awake()
    {
        character = GetComponentInParent<Character>();
        character_base = GetComponentInParent<character_base>();
        character.got_effect += OnEffectGiven;
        character.destroy_effect += OnEffectDestory;
    }

    private void OnDestroy()
    {
        character.got_effect -= OnEffectGiven;
        character.destroy_effect -= OnEffectDestory;
    }

    private void OnEffectGiven(character_effect_code code, character_effect_setType type, int power) 
    {

        // 이미 있는 버프 / 디버프인지 확인
        foreach (character_effect effect in effects) 
        {
            if (effect.Code == code) 
            {
                effect.SetPower(power, type);
                return;
            }
        }

        // 없으면 새로 추가
        GameObject containerObj = character_base.Attach(character_base.location.effect_layoutGroup, effectContainer_prefab);
        character_effect_container Container = containerObj.GetComponent<character_effect_container>();
        Container.Initialize(character);

        effects.Add(CharacterEffectManager.instance.make_effect(code, power, character, Container));
    }

    private void OnEffectDestory(character_effect_code code) 
    {
        foreach (character_effect effect in effects)
        {
            if (effect.Code == code)
            {
                effects.Remove(effect);
                effect.OnDestroy();
                return;
            }
        }
    }

}
