using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class skill_effect_text // ��ų Ư��ȿ�� �����ִ� �ؽ�Ʈ ���� Ŭ����
{
    public GameObject nameTextObj;
    public GameObject effectTextObj;
    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text effectTMP;

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
