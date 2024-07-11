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

    public void set_effect_text(character_effect_code code, Camera cam) // 스킬 효과 텍스트 설정
    {
        linkHandler.cam = cam;

        switch (code) 
        {
            case character_effect_code.flame:
                nameTMP.text = "화염";
                effectTMP.text = "턴 종료시 위력의 절반만큼 체력, 정신력 피해를 준다. 이후 위력이 절반으로 감소한다. (소수점 버림)";
                break;
            case character_effect_code.ignition_attack:
                nameTMP.text = "발화 공격";
                effectTMP.text = "보유 캐릭터가 공격으로 피해를 입힐 시 상대방에게 <link=\"flame\"><style=\"Effect_Description\">화염</style></link>을 위력만큼 부여한다. 이 효과는 턴 종료시 사라진다.";
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
