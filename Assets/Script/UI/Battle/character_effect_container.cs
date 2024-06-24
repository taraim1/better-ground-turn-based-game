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

        powerTmp.text = effect.power.ToString();
    }

    private void apply_effect() // 효과 발동
    { 
        
    }

    public void clear_delegate_and_destroy() // 액션에 붙에있는 델리게이트 떼고 효과 삭제 및 파괴
    { 
    
    }

    public character_effect_code Get_effect_code() 
    {
        return effect.code;
    }

}
