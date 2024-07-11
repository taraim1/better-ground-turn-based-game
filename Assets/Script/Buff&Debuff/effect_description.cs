using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class effect_description : MonoBehaviour
{

    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text effectTMP;
    [SerializeField] private TMP_LinkHandler linkHandler;
    [SerializeField] private Canvas baseCanvas;
    [SerializeField] private Canvas descriptionCanvas;
    effect_description_handler description_handler;

    public void set_effect_text(character_effect_code code, Camera cam) // ��ų ȿ�� �ؽ�Ʈ ����
    {
        linkHandler.cam = cam;

        switch (code) 
        {
            case character_effect_code.flame:
                nameTMP.text = "ȭ��";
                effectTMP.text = "�� ����� ������ ���ݸ�ŭ ü��, ���ŷ� ���ظ� �ش�. ���� ������ �������� �����Ѵ�. (�Ҽ��� ����)";
                break;
            case character_effect_code.ignition_attack:
                nameTMP.text = "��ȭ ����";
                effectTMP.text = "���� ĳ���Ͱ� �������� ���ظ� ���� �� ���濡�� <link=\"flame\"><style=\"Effect_Description\">ȭ��</style></link>�� ���¸�ŭ �ο��Ѵ�. �� ȿ���� �� ����� �������.";
                break;
        }

    }

    public void set_order(int order) 
    {
        baseCanvas.sortingOrder = order;
        descriptionCanvas.sortingOrder = order + 1;
    }

    private void Awake()
    {
        description_handler = GameObject.Find("effect_description_click_handler").GetComponent<effect_description_handler>();
    }

    public void Destroy_self() 
    {
        description_handler.description_count -= 1;
        Destroy(gameObject);
    }




}
