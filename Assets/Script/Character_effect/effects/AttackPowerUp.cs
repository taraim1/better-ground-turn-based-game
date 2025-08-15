using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterEffect
{
    /*
    * ���� ���� ����
    *
    * ���� ��ųī���� ������ �� ȿ���� ���¸�ŭ �����Ѵ�. �� ����� �������.
    */

    public class AttackPowerUp : character_effect
    {
        public AttackPowerUp(character_effect_code code, int power, Character character, character_effect_container container) : base(code, power, character, container) { }

        public override int Get_power_change(card using_card) 
        {
            if (using_card.Data.BehaviorType == CardBehaviorType.attack) 
            {
                return power;
            }
            return 0;
        }

        protected override void OnTurnEnd()
        {
            SetPower(-99, character_effect_setType.replace);
        }
    }

}
