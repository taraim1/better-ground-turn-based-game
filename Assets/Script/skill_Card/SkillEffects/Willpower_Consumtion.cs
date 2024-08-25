using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ī�� ��� �� ���ŷ� ����. ���ŷ� ������ ��� �� ��.
public class Willpower_Consumtion : SkillEffect
{
    public Willpower_Consumtion(skill_effect_code code, card card, List<int> parameters) : base(code, card, parameters) { }

    public override bool check_card_usable(card target_card, Character target_character)
    {
        if (card.owner.Current_willpower < parameters[0]) return false;
        return true;
    }

    public override Tuple<string, string> get_description()
    {
        return Tuple.Create("���ŷ� ����",
            string.Format("��� �� ���ŷ��� <style=\"nexonBold\">{0}</style> �����Ѵ�. ���ŷ��� <style=\"nexonBold\">{0}</style> ������ �� ����� �Ұ����ϴ�.", parameters[0])
            );
    }

    protected override void OnSkillUsed(card target_card, Character target_character)
    {
        card.owner.Damage_willpower(parameters[0]);
    }

}
