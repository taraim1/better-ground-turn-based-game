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
        Tuple<string, string> description = effect.get_description();
        nameTMP.text = description.Item1;
        effectTMP.text = description.Item2;
    }
}
