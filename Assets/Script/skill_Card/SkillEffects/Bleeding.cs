using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace skillEffect
{
    public class Bleeding : SkillEffect
    {
        public Bleeding(skill_effect_code code, card card, List<int> parameters) : base(code, card, parameters) { }


        public override Tuple<string, string> get_description()
        {
            return Tuple.Create("출혈",
                string.Format("위력 판정 승리 시 혹은 직접 사용 시 대상에게 <link=\"bleeding\"><style=\"Effect_Description\">출혈</style></link>을 <style=\"nexonBold\">{0}</style> 부여한다.", parameters[0])
                );
        }

        protected override void OnClashWin(card target_card, Character target_character)
        {
            target_character.give_effect(character_effect_code.bleeding, character_effect_setType.add, parameters[0]);
        }

        protected override void OnDirectUsed(card target_card, Character target_character)
        {
            target_character.give_effect(character_effect_code.bleeding, character_effect_setType.add, parameters[0]);
        }

    }

}
