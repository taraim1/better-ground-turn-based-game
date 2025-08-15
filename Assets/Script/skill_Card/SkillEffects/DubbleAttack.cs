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
            return Tuple.Create("�̿�Ÿ",
                "���� ���� �¸� �� Ȥ�� ���� ��� �� �ߵ�. ������ �ٽ� ���� ���� ����ŭ ü��, ���ŷ� ���ظ� �ش�."
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
