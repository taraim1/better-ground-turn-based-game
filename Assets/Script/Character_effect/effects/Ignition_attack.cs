using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterEffect;

/*
 * 발화 공격
 *
 * 보유 캐릭터가 공격으로 피해를 입힐 시 상대방에게 화염을 위력만큼 부여한다. 이 효과는 턴 종료시 사라진다.
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
