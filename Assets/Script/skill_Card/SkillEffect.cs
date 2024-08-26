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

            // �׼� ����
            card.OnUsed += OnSkillUsed;
            card.OnDirectUsed += OnDirectUsed;
            card.OnClashWin += OnClashWin;
        }

        public void OnDestroy()
        {
            // �׼� ����
            card.OnUsed -= OnSkillUsed;
            card.OnDirectUsed -= OnDirectUsed;
            card.OnClashWin -= OnClashWin;
        }


        /* 
         *  ī�尡 ��� �������� Ȯ���ϴ� �޼ҵ�
         *  target_card�� null�̸� ���� ����� ���
         *  target_character�� null�� ���� ������� ����
         */
        public virtual bool check_card_usable(card target_card, Character target_character) { return true; }


        /*
         * ȿ�� ������ �����ϴ� �޼ҵ�. [�̸�, ����] ����
         */
        public abstract Tuple<string, string> get_description();

        protected virtual void OnSkillUsed(card target_card, Character target_character) { }
        protected virtual void OnDirectUsed(card target_card, Character target_character) { }
        protected virtual void OnClashWin(card target_card, Character target_character) { }

    }
}


