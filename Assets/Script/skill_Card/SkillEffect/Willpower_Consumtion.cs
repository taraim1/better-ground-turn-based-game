using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카드 사용 시 정신력 감소. 정신력 없으면 사용 못 함.
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
        return Tuple.Create("정신력 감소",
            string.Format("사용 시 정신력이 <style=\"nexonBold\">{0}</style> 감소한다. 정신력이 <style=\"nexonBold\">{0}</style> 이하일 시 사용이 불가능하다.", parameters[0])
            );
    }

    protected override void OnSkillUsed(card target_card, Character target_character)
    {
        card.owner.Damage_willpower(parameters[0]);
    }

}
