using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterEffect;

/*
 * ����
 *
 * ��ųī�� ��� �� ���¸�ŭ ü�� ���ظ� �ش�. �� ���� �� ������ �������� �����Ѵ�. (�Ҽ��� ����)
 */

namespace CharacterEffect
{

    public class Bleeding : character_effect
    {
        public Bleeding(character_effect_code code, int power, Character character, character_effect_container container) : base(code, power, character, container) { }

        protected override void OnSkillUsed(card card)
        {
            character.Damage_health(power);
        }

        protected override void OnTurnEnd()
        {
            if (character == null) return;
            SetPower(power / 2, character_effect_setType.replace);
        }
    }

}