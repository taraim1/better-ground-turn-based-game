using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Enchantment : SkillEffect
{
    public Fire_Enchantment(skill_effect_code code, card card, List<int> parameters) : base(code, card, parameters) { }


    public override Tuple<string, string> get_description()
    {
        return Tuple.Create("ȭ�� ���� �ο�",
            string.Format("��� �� ��󿡰� <link=\"ignition_attack\"><style=\"Effect_Description\">��ȭ ����</style></link>�� <style=\"nexonBold\">{0}</style> �ο��Ѵ�.", parameters[0])
            );
    }

    protected override void OnSkillUsed(card target_card, Character target_character)
    {
        target_character.give_effect(character_effect_code.ignition_attack, character_effect_setType.add, parameters[0]);
    }

}

