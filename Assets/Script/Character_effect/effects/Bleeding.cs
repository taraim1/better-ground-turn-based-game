using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterEffect;

/*
 * 출혈
 *
 * 스킬카드 사용 시 위력만큼 체력 피해를 준다. 턴 종료 시 위력이 절반으로 감소한다. (소수점 버림)
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