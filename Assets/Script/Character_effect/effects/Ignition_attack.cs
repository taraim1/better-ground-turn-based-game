using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterEffect;

/*
 * ��ȭ ����
 *
 * ���� ĳ���Ͱ� �������� ���ظ� ���� �� ���濡�� ȭ���� ���¸�ŭ �ο��Ѵ�. �� ȿ���� �� ����� �������.
 */

namespace CharacterEffect
{
    public class Ignition_attack : character_effect
    {
        public Ignition_attack(character_effect_code code, int power, Character character, character_effect_container container) : base(code, power, character, container) { }

        protected override void OnAttack(List<Character> attacked_characters)
        {
            foreach (Character character in attacked_characters)
            {
                if (character == null) continue;
                character.give_effect(character_effect_code.flame, character_effect_setType.add, power);
            }
        }

        protected override void OnTurnEnd()
        {
            SetPower(-99, character_effect_setType.replace);
        }
    }
}
