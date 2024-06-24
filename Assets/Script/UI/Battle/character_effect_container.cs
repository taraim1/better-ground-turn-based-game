using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class character_effect_container : MonoBehaviour
{

    [SerializeField] private character_effect effect;
    [SerializeField] private TMP_Text powerTmp;
    private Character character;
    [SerializeField] private Image image;

    public void Set(character_effect effect, Character character) // 초기화
    { 
        this.effect = effect;
        this.character = character;
        powerTmp.text = effect.power.ToString();

        // 델리게이트 추가
        switch (effect.timing) 
        {
            case character_effect_timing.turn_started:
                BattleEventManager.turn_start_phase += apply_effect;
                break;
            case character_effect_timing.turn_ended:
                BattleEventManager.turn_end_phase += apply_effect;
                break;
        }
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

    private void apply_effect() // 효과 발동
    {
        switch (effect.code) 
        {
            case character_effect_code.flame:
                // 위력만큼 체력, 정신력 대미지를 입히고 위력은 절반이 됨 (소수점 버림)
                character.Damage_health(effect.power);
                character.Damage_willpower(effect.power);
                effect.power = effect.power / 2;
                break;    
        }

        // 효과 삭제 판정
        if (effect.power <= 0) 
        {
            character.remove_effect(this);
            return;
        }

        // 숫자 표기 업데이트
        powerTmp.text = effect.power.ToString();
    }

    public void clear_delegate_and_destroy() // 액션에 붙에있는 델리게이트 떼고 효과 삭제 및 파괴
    {
        // 델리게이트 삭제
        switch (effect.timing)
        {
            case character_effect_timing.turn_started:
                BattleEventManager.turn_start_phase -= apply_effect;
                break;
            case character_effect_timing.turn_ended:
                BattleEventManager.turn_end_phase -= apply_effect;
                break;
        }

        Destroy(gameObject);
    }


    public character_effect_code Get_effect_code() 
    {
        return effect.code;
    }



}
