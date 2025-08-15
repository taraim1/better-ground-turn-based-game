using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace skillEffect
{
    public class Prepare_counterAttack : SkillEffect
    {
        public Prepare_counterAttack(skill_effect_code code, card card, List<int> parameters) : base(code, card, parameters) { }


        public override Tuple<string, string> get_description()
        {
            return Tuple.Create("반격 준비",
                string.Format("위력 판정 승리 시 <link=\"attack_power_up\"><style=\"Effect_Description\">공격 위력 증가</style></link>을 <style=\"nexonBold\">{0}</style> 얻는다.", parameters[0])
                );
        }

        protected override void OnClashWin(card target_card, Character target_character)
        {
            card.owner.give_effect(character_effect_code.attack_power_up, character_effect_setType.add, parameters[0]);
        }

    }

}
