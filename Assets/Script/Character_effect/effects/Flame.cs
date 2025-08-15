using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterEffect;

/*
 * ȭ��
 *
 * �� ����� ���¸�ŭ ü��, ���ŷ� ���ظ� �ش�. ���� ������ �������� �����Ѵ�. (�Ҽ��� ����)
 */

namespace CharacterEffect 
{

    public class Flame : character_effect
    {
        public Flame(character_effect_code code, int power, Character character, character_effect_container container) : base(code, power, character, container) { }

        protected override void OnTurnEnd()
        {
            if (character == null) return;

            character.Damage_health(power);
            SetPower(power / 2, character_effect_setType.replace);
        }
    }
}