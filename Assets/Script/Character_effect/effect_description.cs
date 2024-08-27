using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class effect_description : MonoBehaviour, Iclickable
{

    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text effectTMP;
    [SerializeField] private TMP_LinkHandler linkHandler;
    [SerializeField] private Canvas baseCanvas;
    [SerializeField] private Canvas descriptionCanvas;
    effect_description_handler description_handler;

    public void set_effect_text(string name, string description, Camera cam) // 스킬 효과 텍스트 설정
    {
        linkHandler.cam = cam;

        nameTMP.text = name;
        effectTMP.text = description;

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
        description_handler.reduce_count();
        Destroy(gameObject);
    }

    public void OnClick() 
    { 
        // 아무 것도 안 함
    }


}
