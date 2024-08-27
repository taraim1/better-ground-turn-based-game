using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace skillEffect
{
    public class DubbleAttack : SkillEffect
    {
        public DubbleAttack(skill_effect_code code, card card, List<int> parameters) : base(code, card, parameters) { }


        public override Tuple<string, string> get_description()
        {
            return Tuple.Create("이연타",
                "위력 판정 승리 시 혹은 직접 사용 시 발동. 위력을 다시 굴려 나온 값만큼 체력, 정신력 피해를 준다."
                );
        }

        protected override void OnClashWin(card target_card, Character target_character)
        {
            int power = card.PowerRoll();
            target_character.Damage_health(power);
            target_character.Damage_willpower(power);
        }

        protected override void OnDirectUsed(card target_card, Character target_character)
        {
            int power = card.PowerRoll();
            target_character.Damage_health(power);
            target_character.Damage_willpower(power);
        }

    }

}
