using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using skillEffect;

[Serializable]
public class skill_effect_text // ��ų Ư��ȿ�� �����ִ� �ؽ�Ʈ ���� Ŭ����
{
    private GameObject nameTextObj;
    private GameObject effectTextObj;
    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text effectTMP;


    public skill_effect_text(TMP_Text nameTMP, TMP_Text effectTMP) 
    {
        this.nameTMP = nameTMP;
        this.effectTMP = effectTMP;
        nameTextObj = nameTMP.gameObject;
        effectTextObj = effectTMP.gameObject;
    }

    public void deactivate() // �ؽ�Ʈ ������Ʈ ��Ȱ��ȭ
    {
        nameTextObj.SetActive(false);
        effectTextObj.SetActive(false);
    }

    public void activate() // �ؽ�Ʈ ������Ʈ Ȱ��ȭ
    {
        nameTextObj.SetActive(true);
        effectTextObj.SetActive(true);
    }

    public void set_effect_text(SkillEffect effect) // ��ų ȿ�� �ؽ�Ʈ ����
    {
        Tuple<string, string> description = effect.get_description();
        nameTMP.text = description.Item1;
        effectTMP.text = description.Item2;
    }

    
}
