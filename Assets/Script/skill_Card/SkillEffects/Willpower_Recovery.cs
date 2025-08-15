using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace skillEffect
{
    // 카드 사용 시 정신력 회복.
    public class Willpower_Recovery : SkillEffect
    {
        public Willpower_Recovery(skill_effect_code code, card card, List<int> parameters) : base(code, card, parameters) { }


        public override Tuple<string, string> get_description()
        {
            return Tuple.Create("정신력 회복",
                string.Format("사용 시 정신력이 <style=\"nexonBold\">{0}</style> 증가한다.", parameters[0])
                );
        }

        protected override void OnSkillUsed(card target_card, Character target_character)
        {
            card.owner.Heal_willpower(parameters[0]);
        }

    }
}