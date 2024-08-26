using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterEffect;

public class CharacterEffectController : MonoBehaviour
{
    [SerializeField] private GameObject effectContainer_prefab;
    [SerializeField] private characterEffectSO characterEffectSO;
    private characterEffectsDictionary effectDict;

    private Character character;
    private character_base character_base;
    private List<character_effect> effects;

    private void Awake()
    {
        effectDict = characterEffectSO.CharacterEffectDict;
        character = GetComponentInParent<Character>();
        effects = character.effects;
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

        effects.Add(make_effect(code, power, character, Container));
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
            case character_effect_code.bleeding:
                product = new Bleeding(code, power, character, container);
                break;
            case character_effect_code.attack_power_up:
                product = new AttackPowerUp(code, power, character, container);
                break;

            default:
                product = new Flame(code, power, character, container);
                break;
        }

        return product;
    }
}
