using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using skillEffect;


namespace skillEffect
{
    // �ڽ�, ��, �Ʊ� �� ī���� ��� ���� ����� �����ϴ� ȿ��
    public class TargetLimitEffect : SkillEffect
    {
        private List<skill_target> skill_Targets;

        public TargetLimitEffect(skill_effect_code code, card card, List<int> parameters, List<skill_target> skill_Targets) : base(code, card, parameters)
        {
            this.skill_Targets = skill_Targets;
        }

        public override bool check_card_usable(card target_card, Character target_character)
        {

            skill_target current_target = skill_target.self;

            if (target_character.check_enemy() != card.owner.check_enemy())
            {
                current_target = skill_target.enemy;
            }
            else if (target_character != card.owner)
            {
                current_target = skill_target.friend;
            }

            return skill_Targets.Contains(current_target);
        }

        public override Tuple<string, string> get_description()
        {

            List<string> target_strings = new List<string>();
            foreach (skill_target target in skill_Targets)
            {
                switch (target)
                {
                    case skill_target.self:
                        target_strings.Add("�ڽ�");
                        break;
                    case skill_target.enemy:
                        target_strings.Add("��");
                        break;
                    case skill_target.friend:
                        target_strings.Add("�ڽ��� ������ �Ʊ�");
                        break;
                }
            }

            return Tuple.Create("��� ��� ����",
                "�� ��ų�� " + string.Join(", ", target_strings) + "���� ��� �����ϴ�."
                );
        }
    }
}