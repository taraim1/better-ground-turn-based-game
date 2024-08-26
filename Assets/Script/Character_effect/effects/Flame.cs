using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterEffect;

/*
 * 화염
 *
 * 턴 종료시 위력만큼 체력, 정신력 피해를 준다. 이후 위력이 절반으로 감소한다. (소수점 버림)
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