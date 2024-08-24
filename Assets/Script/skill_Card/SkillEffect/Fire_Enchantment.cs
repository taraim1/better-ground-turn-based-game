using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Enchantment : SkillEffect
{
    public Fire_Enchantment(skill_effect_code code, card card, List<int> parameters) : base(code, card, parameters) { }


    public override Tuple<string, string> get_description()
    {
        return Tuple.Create("화염 마법 부여",
            string.Format("사용 시 대상에게 <link=\"ignition_attack\"><style=\"Effect_Description\">발화 공격</style></link>을 <style=\"nexonBold\">{0}</style> 부여한다.", parameters[0])
            );
    }

    protected override void OnSkillUsed(card target_card, Character target_character)
    {
        target_character.give_effect(character_effect_code.ignition_attack, character_effect_setType.add, parameters[0]);
    }

}

