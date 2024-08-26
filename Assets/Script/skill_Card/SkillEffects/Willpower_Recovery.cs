using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace skillEffect
{
    // ī�� ��� �� ���ŷ� ȸ��.
    public class Willpower_Recovery : SkillEffect
    {
        public Willpower_Recovery(skill_effect_code code, card card, List<int> parameters) : base(code, card, parameters) { }


        public override Tuple<string, string> get_description()
        {
            return Tuple.Create("���ŷ� ȸ��",
                string.Format("��� �� ���ŷ��� <style=\"nexonBold\">{0}</style> �����Ѵ�.", parameters[0])
                );
        }

        protected override void OnSkillUsed(card target_card, Character target_character)
        {
            card.owner.Heal_willpower(parameters[0]);
        }

    }
}