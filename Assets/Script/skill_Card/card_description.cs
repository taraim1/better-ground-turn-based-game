using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class card_description : MonoBehaviour
{
    private card target_card; // ī�� ������ ������ ī��
    private GameObject target_card_obj; // �� ī���� ������Ʈ
    private float target_card_show_elapsed_time = 0; // ī�� ������ �����ְ� ���� �ð�
    [SerializeField] private Vector3 offset; // ī��� �󸶳� �������־�� �ϴ��� ����
    Vector3 current_offset;
    [SerializeField] private List<skill_effect_text> EffectTextPool;

    [Serializable]
    private class skill_effect_text // ��ų Ư��ȿ�� �����ִ� �ؽ�Ʈ ���� Ŭ����
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
                    text = "�ߵ� ���� " + text;
                    break;
            }

            if (effect.code == skill_effect_code.none) { text = ""; }
            effectTMP.text = text;
        }
    }

    private void deactivate_all_text() // ��� �ؽ�Ʈ ������Ʈ ��Ȱ��ȭ
    {
        foreach (var text in EffectTextPool) 
        {
            text.deactivate();
        }
    }


    public void Set_target(card target_card)
    {
        deactivate_all_text();
        this.target_card = target_card;
        target_card_obj = target_card.gameObject;
        target_card_show_elapsed_time = 0;

        // �ؽ�Ʈ ������Ʈ Ȱ��ȭ �� �ؽ�Ʈ ����
        for (int i = 0; i < target_card.Data.Effects.Count; i++) 
        {
            EffectTextPool[i].activate();
            EffectTextPool[i].set_effect_text(target_card.Data.Effects[i]);
        }
    }

    public card get_target() // Ÿ�� ��������
    {
        if (target_card != null) 
        {
            return target_card;
        }

        return null;
    } 

    public void Clear_target() 
    {
        target_card = null;
        target_card_obj = null;
        transform.position = new Vector3(-13, 0, 0);
    }

    private void Update()
    {
        if (target_card != null)
        {
            target_card_show_elapsed_time += Time.deltaTime;

            // ī�� ���̶���Ʈ �Ǵ� ��
            if (target_card_show_elapsed_time <= 0.2f)
            {
                current_offset = offset * target_card_show_elapsed_time / 0.2f;
            }
            // ī�� ���̶���Ʈ �� ��
            else 
            {
                current_offset = offset;
            }

            transform.position = target_card_obj.transform.position + current_offset;
            transform.rotation = target_card_obj.transform.rotation;
            transform.localScale = target_card_obj.transform.localScale;

        }

    }


}
