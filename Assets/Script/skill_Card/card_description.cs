using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static card;
using UnityEngine.UI;

public class card_description : MonoBehaviour, Iclickable
{
    private card target_card; // ī�� ������ ������ ī��
    private GameObject target_card_obj; // �� ī���� ������Ʈ
    private float target_card_show_elapsed_time = 0; // ī�� ������ �����ְ� ���� �ð�
    [SerializeField] private Vector3 offset; // ī��� �󸶳� �������־�� �ϴ��� ����
    Vector3 current_offset;
    [SerializeField] private List<skill_effect_text> EffectTextPool;
    [SerializeField] private GameObject nameObjPrefab;
    [SerializeField] private GameObject descriptionObjPrefab;
    [SerializeField] private GameObject content_layoutGroup;
    [SerializeField] private ScrollRect scrollRect;
    public void OnClick()
    {
        // �Ʊ� ī���̸�
        if (target_card.isEnemyCard == false)
        {
            // �ƹ� �͵� �� ��
        }
        // ���� ī���
        else
        {
            // �� ī�� ���� ����
            if (target_card.state == current_mode.highlighted_enemy_card)
            {
                ActionManager.enemy_skillcard_deactivate?.Invoke();
            }
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
        for (int i = 0; i < target_card.Effects.Count; i++) 
        {
            // �߰��ؾ� �ϴ� ���
            if (i >= EffectTextPool.Count) 
            {
                TMP_Text nameTmp = Instantiate(nameObjPrefab, content_layoutGroup.transform).GetComponent<TMP_Text>();
                TMP_Text descriptionTmp = Instantiate(descriptionObjPrefab, content_layoutGroup.transform).GetComponentInChildren<TMP_Text>();
                EffectTextPool.Add(new skill_effect_text(nameTmp, descriptionTmp));
            }

            EffectTextPool[i].activate();
            EffectTextPool[i].set_effect_text(target_card.Effects[i]);
        }

        // ��ũ�� �ʱ�ȭ
        scrollRect.verticalNormalizedPosition = 1;
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
