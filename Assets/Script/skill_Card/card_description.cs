using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static card;
using UnityEngine.UI;

public class card_description : MonoBehaviour, Iclickable
{
    private card target_card; // 카드 설명을 보여줄 카드
    private GameObject target_card_obj; // 그 카드의 오브젝트
    private float target_card_show_elapsed_time = 0; // 카드 설명을 보여주고 지난 시간
    [SerializeField] private Vector3 offset; // 카드와 얼마나 떨어져있어야 하는지 저장
    Vector3 current_offset;
    [SerializeField] private List<skill_effect_text> EffectTextPool;
    [SerializeField] private GameObject nameObjPrefab;
    [SerializeField] private GameObject descriptionObjPrefab;
    [SerializeField] private GameObject content_layoutGroup;
    [SerializeField] private ScrollRect scrollRect;
    public void OnClick()
    {
        // 아군 카드이면
        if (target_card.isEnemyCard == false)
        {
            // 아무 것도 안 함
        }
        // 적군 카드면
        else
        {
            // 적 카드 강조 해제
            if (target_card.state == current_mode.highlighted_enemy_card)
            {
                ActionManager.enemy_skillcard_deactivate?.Invoke();
            }
        }
    }

    private void deactivate_all_text() // 모든 텍스트 오브젝트 비활성화
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

        // 텍스트 오브젝트 활성화 및 텍스트 설정
        for (int i = 0; i < target_card.Effects.Count; i++) 
        {
            // 추가해야 하는 경우
            if (i >= EffectTextPool.Count) 
            {
                TMP_Text nameTmp = Instantiate(nameObjPrefab, content_layoutGroup.transform).GetComponent<TMP_Text>();
                TMP_Text descriptionTmp = Instantiate(descriptionObjPrefab, content_layoutGroup.transform).GetComponentInChildren<TMP_Text>();
                EffectTextPool.Add(new skill_effect_text(nameTmp, descriptionTmp));
            }

            EffectTextPool[i].activate();
            EffectTextPool[i].set_effect_text(target_card.Effects[i]);
        }

        // 스크롤 초기화
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

            // 카드 하이라이트 되는 중
            if (target_card_show_elapsed_time <= 0.2f)
            {
                current_offset = offset * target_card_show_elapsed_time / 0.2f;
            }
            // 카드 하이라이트 다 됨
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
