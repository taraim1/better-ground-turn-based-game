using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum skill_effect_code
{
    target_limit,
    willpower_consumption,
    willpower_recovery,
    ignition,
    fire_enchantment,
    bleeding,
    confusion,
    prepare_counterattack
}

public enum skill_target 
{ 
    self,
    friend,
    enemy
}

[System.Serializable]
public class SkillEffect_label 
{
    [SerializeField] private skill_effect_code code;
    [SerializeField] private List<int> parameters;

    public skill_effect_code Code => code;
    public List<int> Parameters => parameters;
}

namespace skillEffect 
{
    public abstract class SkillEffect
    {
        protected skill_effect_code code;
        protected List<int> parameters;
        protected card card;

        public SkillEffect(skill_effect_code code, card card, List<int> parameters)
        {
            this.code = code;
            this.card = card;
            this.parameters = parameters;

            // 액션 설정
            card.OnUsed += OnSkillUsed;
            card.OnDirectUsed += OnDirectUsed;
            card.OnClashWin += OnClashWin;
        }

        public void OnDestroy()
        {
            // 액션 설정
            card.OnUsed -= OnSkillUsed;
            card.OnDirectUsed -= OnDirectUsed;
            card.OnClashWin -= OnClashWin;
        }


        /* 
         *  카드가 사용 가능한지 확인하는 메소드
         *  target_card가 null이면 직접 사용인 경우
         *  target_character가 null인 경우는 허용하지 않음
         */
        public virtual bool check_card_usable(card target_card, Character target_character) { return true; }


        /*
         * 효과 설명을 리턴하는 메소드. [이름, 설명] 리턴
         */
        public abstract Tuple<string, string> get_description();

        protected virtual void OnSkillUsed(card target_card, Character target_character) { }
        protected virtual void OnDirectUsed(card target_card, Character target_character) { }
        protected virtual void OnClashWin(card target_card, Character target_character) { }

    }
}


