using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum character_effect_code 
{ 
    flame,
    ignition_attack,
    bleeding
}


public enum character_effect_setType 
{ 
    add,
    subtract,
    replace,
}

namespace CharacterEffect 
{
    public abstract class character_effect
    {
        protected character_effect_code code;
        protected int power;
        protected Character character;

        private character_effect_container container;

        public character_effect_code Code => code;
        public int Power => power;

        public character_effect(character_effect_code code, int power, Character character, character_effect_container container)
        {
            this.character = character;
            this.container = container;
            this.code = code;
            container.InitializeContainer(code);

            SetPower(power, character_effect_setType.replace);

            // 액션 설정
            ActionManager.character_died += OnCharacterDied;
            ActionManager.turn_end_phase += OnTurnEnd;
            ActionManager.attacked += OnAttackActionInvoked;
            character.skillcard_used += OnSkillUsed;
        }

        public void OnDestroy()
        {
            // 액션 설정
            ActionManager.character_died -= OnCharacterDied;
            ActionManager.turn_end_phase -= OnTurnEnd;
            ActionManager.attacked -= OnAttackActionInvoked;
            if (character != null)
            {
                character.skillcard_used -= OnSkillUsed;
            }

            // 컨테이너 없애기
            container.DestroyThis();
            container = null;
        }

        // power 설정은 이걸로
        public void SetPower(int power, character_effect_setType setType)
        {
            switch (setType)
            {
                case character_effect_setType.add:
                    this.power += power;
                    break;
                case character_effect_setType.subtract:
                    this.power -= power;
                    break;
                case character_effect_setType.replace:
                    this.power = power;
                    break;
            }

            if (container != null)
            {
                container.OnEffectUpdated(this.power);
            }

            if (this.power <= 0)
            {
                // 이펙트 제거
                character.remove_effect(code);
            }
        }

        // 액션매니저에서 주는 attacked에서 공격당한 건지 공격한 건지 구분해줌
        private void OnAttackActionInvoked(Character attacking_character, List<Character> attacked_characters)
        {
            if (character == attacking_character)
            {
                OnAttack(attacked_characters);
            }
            if (attacked_characters.Contains(character))
            {
                OnGetAttacked(attacking_character);
            }
        }

        private void OnCharacterDied(Character character)
        {
            if (this.character == character)
            {
                // 이펙트 제거
                character.remove_effect(code);
            }
        }

        protected virtual void OnTurnEnd() { }
        protected virtual void OnAttack(List<Character> attacked_characters) { }
        protected virtual void OnGetAttacked(Character attacking_character) { }
        protected virtual void OnSkillUsed(card card) { }
    }
}
