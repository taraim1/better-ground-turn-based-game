using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace skillEffect
{
    public class Confusion : SkillEffect
    { 
        public Confusion(skill_effect_code code, card card, List<int> parameters) : base(code, card, parameters) { }


        public override Tuple<string, string> get_description()
        {
            return Tuple.Create("ȥ��",
                string.Format("���� ���� �¸� �� Ȥ�� ���� ��� �� ����� ���ŷ��� <style=\"nexonBold\">{0}</style> ���ҽ�Ų��.", parameters[0])
                );
        }

        protected override void OnClashWin(card target_card, Character target_character)
        {
            target_character.Damage_willpower(parameters[0]);
        }

        protected override void OnDirectUsed(card target_card, Character target_character)
        {
            target_character.Damage_willpower(parameters[0]);
        }
    }
}