using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectController : MonoBehaviour
{
    [SerializeField] private GameObject effectContainer_prefab;

    private Character character;
    private character_base character_base;
    private List<character_effect_container> containers = new List<character_effect_container>();

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
        foreach (character_effect_container container in containers) 
        {
            if (container.Get_effect_code() == code) 
            {
                container.updateEffect(power, type);
                return;
            }
        }

        // 없으면 새로 추가
        character_effect effect = CharacterEffectManager.instance.get_effect(code, power);
        GameObject containerObj = character_base.Attach(character_base.location.effect_layoutGroup, effectContainer_prefab);
        character_effect_container Container = containerObj.GetComponent<character_effect_container>();
        Container.Initialize(character);
        containers.Add(Container);
        Container.SetEffect(effect);
    }

    private void OnEffectDestory(character_effect_code code) 
    {
        foreach (character_effect_container container in containers)
        {
            if (container.Get_effect_code() == code)
            {
                containers.Remove(container);
                container.clear_delegate_and_destroy();
                return;
            }
        }
    }

}
