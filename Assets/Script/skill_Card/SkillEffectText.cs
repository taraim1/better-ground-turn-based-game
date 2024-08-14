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
        string text = "";
        switch (effect.code)
        {
            // ���⿡ �ִ� ĳ���� ȿ���� TMP��ũ ��Ʈ���� �� ȿ���� character_effect_code���� ���ƾ� ��
            case skill_effect_code.none:
                nameTMP.text = "Ư�� ȿ�� ����";
                text = "";
                break;
            case skill_effect_code.willpower_consumption:
                nameTMP.text = "���ŷ� �Ҹ�";
                text = String.Format("���ŷ��� {0} �Ҹ��Ѵ�. ���ŷ��� {0} ������ �� ������ �ʴ´�.", effect.parameters[0]);
                break;
            case skill_effect_code.willpower_recovery:
                nameTMP.text = "���ŷ� ȸ��";
                text = String.Format("���ŷ��� {0} ȸ���Ѵ�. ȸ���Ǵ� ���ŷ��� ���ŷ� �Ѱ�ġ�� ���� �� ����.", effect.parameters[0]);
                break;
            case skill_effect_code.ignition:
                nameTMP.text = "��ȭ";
                text = String.Format("��󿡰� <link=\"flame\"><style=\"Effect_Description\">ȭ��</style></link>�� {0} �ο��Ѵ�.", effect.parameters[0]);
                break;
            case skill_effect_code.fire_enchantment:
                nameTMP.text = "��ȭ ���� �ο�";
                text = String.Format("��󿡰� <link=\"ignition_attack\"><style=\"Effect_Description\">��ȭ ����</style></link>�� {0} �ο��Ѵ�.", effect.parameters[0]);
                break;
        }

        switch (effect.timing)
        {
            case skill_effect_timing.immediate:
                text = "��� �� " + text;
                break;
            case skill_effect_timing.after_use:
                text = "��ų �ߵ� ���� " + text;
                break;
        }

        if (effect.code == skill_effect_code.none) { text = ""; }
        effectTMP.text = text;
    }
}
