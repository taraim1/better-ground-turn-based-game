using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotController : MonoBehaviour
{
    EnemyCharacter character;
    character_base character_base;
    [SerializeField] private GameObject skillSlot_prefab;

    private void Awake()
    {
        character = GetComponentInParent<EnemyCharacter>();
        character_base = GetComponentInParent<character_base>();
        character.skillcard_reserved += OnReserveSkillcard;
    }

    private void OnDestroy()
    {
        character.skillcard_reserved -= OnReserveSkillcard;
    }

    private void OnReserveSkillcard(card card) 
    {
        // ½ºÅ³ ½½·Ô ¸¸µê
        GameObject slotObj = character_base.Attach(character_base.location.Skill_slot, skillSlot_prefab);
        enemy_skillCard_slot slot = slotObj.GetComponent<enemy_skillCard_slot>();
        slot.Initialize(card.illust.sprite, card.gameObject);    
    }
}
