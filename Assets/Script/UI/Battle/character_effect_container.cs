using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class character_effect_container : BattleUI.CharacterUI
{ 
    [SerializeField] private character_effect effect;
    [SerializeField] private TMP_Text powerTmp;
    private List<Character> targets = new List<Character>();
    [SerializeField] private Image image;

    public void SetEffect(character_effect effect) // 이펙트 초기화
    { 
        this.effect = effect;
        powerTmp.text = effect.power.ToString();

        // 델리게이트 추가
        switch (effect.apply_timing) 
        {
            case character_effect_timing.turn_started:
                ActionManager.turn_start_phase += use_effect_to_all_targets;
                break;
            case character_effect_timing.turn_ended:
                ActionManager.turn_end_phase += use_effect_to_all_targets;
                break;
            case character_effect_timing.after_attack:
                ActionManager.attacked += check_attack_effect_use;
                break;
        }
        switch (effect.power_reduce_timing) 
        {
            case character_effect_power_reduce_timing.turn_started:
                ActionManager.turn_start_phase += reduce_effect_power;
                break;
            case character_effect_power_reduce_timing.turn_ended:
                ActionManager.turn_end_phase += reduce_effect_power;
                break;
        }

        // 타겟 설정
        if (effect.target_type == character_effect_target_type.owner) 
        {
            targets.Add(character);
        }

        // 아이콘 불러오기
        image.sprite = CharacterEffectManager.instance.get_icon_by_code(effect.code);
    }

    public void updateEffect(int power, character_effect_setType type) // 위력 갱신 or 추가
    {
        switch (type) 
        {
            case character_effect_setType.add:
                effect.power += power;
                break;
            case character_effect_setType.replace:
                effect.power = power;
                break;
        }

        // 숫자 표기 업데이트
        powerTmp.text = effect.power.ToString();
    }

    private void check_attack_effect_use(Character attacker, List<Character> targets) // 공격 시 발동인 효과 사용 검사
    {
        if (attacker == character)
        {
            if (effect.target_type == character_effect_target_type.attack_target) // 효과 타겟이 공격 대상이면
            {
                this.targets = targets; // 공격 대상 받기
            }
            use_effect_to_all_targets();
        }

    }

    // 이펙트 위력 감소
    private void reduce_effect_power()
    {
        switch (effect.code)
        {
            case character_effect_code.flame:
                effect.power = effect.power / 2;
                break;
            case character_effect_code.ignition_attack:
                effect.power = 0;
                break;
        }

        // 효과 삭제 판정
        if (effect.power <= 0)
        {
            character.remove_effect(effect.code);
            return;
        }

        // 숫자 표기 업데이트
        powerTmp.text = effect.power.ToString();
    }

    private void use_effect_to_all_targets() // 모든 타겟에게 이펙트 발동 (하나인 것도)
    {
        // 모든 타겟에게 이펙트 적용
        foreach (Character target in targets) 
        {
            apply_effect(target);
        }

        // 위력 감소
        if (effect.power_reduce_timing == character_effect_power_reduce_timing.used) 
        {
            reduce_effect_power();
        }

    }


    private void apply_effect(Character target) // 효과 발동
    {
        switch (effect.code) 
        {
            case character_effect_code.flame:
                // 위력만큼 체력, 정신력 대미지를 입힘
                target.Damage_health(effect.power);
                target.Damage_willpower(effect.power);
                break;
            case character_effect_code.ignition_attack:
                // 위력만큼 대상에게 화염을 부여함
                target.give_effect(character_effect_code.flame, character_effect_setType.add, effect.power);
                break;
        }

    }

    public void clear_delegate_and_destroy() // 액션에 붙에있는 델리게이트 떼고 효과 삭제 및 파괴
    {
        // 델리게이트 삭제
        switch (effect.apply_timing)
        {
            case character_effect_timing.turn_started:
                ActionManager.turn_start_phase -= use_effect_to_all_targets;
                break;
            case character_effect_timing.turn_ended:
                ActionManager.turn_end_phase -= use_effect_to_all_targets;
                break;
            case character_effect_timing.after_attack:
                ActionManager.attacked -= check_attack_effect_use;
                break;
        }
        switch (effect.power_reduce_timing)
        {
            case character_effect_power_reduce_timing.turn_started:
                ActionManager.turn_start_phase -= reduce_effect_power;
                break;
            case character_effect_power_reduce_timing.turn_ended:
                ActionManager.turn_end_phase -= reduce_effect_power;
                break;
        }

        Destroy(gameObject);
    }


    public character_effect_code Get_effect_code() 
    {
        return effect.code;
    }

}
