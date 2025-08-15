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
            return Tuple.Create("����",
                string.Format("���� ���� �¸� �� Ȥ�� ���� ��� �� ��󿡰� <link=\"bleeding\"><style=\"Effect_Description\">����</style></link>�� <style=\"nexonBold\">{0}</style> �ο��Ѵ�.", parameters[0])
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
