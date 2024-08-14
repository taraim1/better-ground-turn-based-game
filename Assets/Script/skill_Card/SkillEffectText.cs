using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class skill_effect_text // 스킬 특수효과 보여주는 텍스트 관리 클래스
{
    public GameObject nameTextObj;
    public GameObject effectTextObj;
    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text effectTMP;

    public void deactivate() // 텍스트 오브젝트 비활성화
    {
        nameTextObj.SetActive(false);
        effectTextObj.SetActive(false);
    }

    public void activate() // 텍스트 오브젝트 활성화
    {
        nameTextObj.SetActive(true);
        effectTextObj.SetActive(true);
    }

    public void set_effect_text(SkillEffect effect) // 스킬 효과 텍스트 설정
    {
        string text = "";
        switch (effect.code)
        {
            // 여기에 있는 캐릭터 효과의 TMP링크 스트링은 각 효과의 character_effect_code값과 같아야 함
            case skill_effect_code.none:
                nameTMP.text = "특수 효과 없음";
                text = "";
                break;
            case skill_effect_code.willpower_consumption:
                nameTMP.text = "정신력 소모";
                text = String.Format("정신력을 {0} 소모한다. 정신력이 {0} 이하일 시 사용되지 않는다.", effect.parameters[0]);
                break;
            case skill_effect_code.willpower_recovery:
                nameTMP.text = "정신력 회복";
                text = String.Format("정신력을 {0} 회복한다. 회복되는 정신력은 정신력 한계치를 넘을 수 없다.", effect.parameters[0]);
                break;
            case skill_effect_code.ignition:
                nameTMP.text = "점화";
                text = String.Format("대상에게 <link=\"flame\"><style=\"Effect_Description\">화염</style></link>을 {0} 부여한다.", effect.parameters[0]);
                break;
            case skill_effect_code.fire_enchantment:
                nameTMP.text = "발화 마법 부여";
                text = String.Format("대상에게 <link=\"ignition_attack\"><style=\"Effect_Description\">발화 공격</style></link>을 {0} 부여한다.", effect.parameters[0]);
                break;
        }

        switch (effect.timing)
        {
            case skill_effect_timing.immediate:
                text = "사용 시 " + text;
                break;
            case skill_effect_timing.after_use:
                text = "스킬 발동 이후 " + text;
                break;
        }

        if (effect.code == skill_effect_code.none) { text = ""; }
        effectTMP.text = text;
    }
}
