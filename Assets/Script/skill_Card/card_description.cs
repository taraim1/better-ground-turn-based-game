using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class card_description : MonoBehaviour
{
    private card target_card; // 카드 설명을 보여줄 카드
    private GameObject target_card_obj; // 그 카드의 오브젝트
    private float target_card_show_elapsed_time = 0; // 카드 설명을 보여주고 지난 시간
    [SerializeField] private Vector3 offset; // 카드와 얼마나 떨어져있어야 하는지 저장
    Vector3 current_offset;
    [SerializeField] private List<skill_effect_text> EffectTextPool;

    [Serializable]
    private class skill_effect_text // 스킬 특수효과 보여주는 텍스트 관리 클래스
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
            string text = "";
            switch (effect.code) 
            {
                // 여기에 있는 캐릭터 효과의 TMP링크 스트링은 각 효과의 character_effect_code값과 같아야 함
                case skill_effect_code.none:
                    nameTMP.text = "특수 효과 없음";
                    text = "";
                    break;
                case skill_effect_code.willpower_consumption:
                    nameTMP.text = "정신력 소모";
                    text = String.Format("정신력을 {0} 소모한다. 정신력이 {0} 이하일 시 사용되지 않는다.", effect.parameters[0]);
                    break;
                case skill_effect_code.willpower_recovery:
                    nameTMP.text = "정신력 회복";
                    text = String.Format("정신력을 {0} 회복한다. 회복되는 정신력은 정신력 한계치를 넘을 수 없다.", effect.parameters[0]);
                    break;
                case skill_effect_code.ignition:
                    nameTMP.text = "점화";
                    text = String.Format("대상에게 <link=\"flame\"><style=\"Effect_Description\">화염</style></link>을 {0} 부여한다.", effect.parameters[0]);
                    break;
                case skill_effect_code.fire_enchantment:
                    nameTMP.text = "발화 마법 부여";
                    text = String.Format("대상에게 <link=\"ignition_attack\"><style=\"Effect_Description\">발화 공격</style></link>을 {0} 부여한다.", effect.parameters[0]);
                    break;
            }

            switch (effect.timing) 
            {
                case skill_effect_timing.immediate:
                    text = "사용 시 " + text;
                    break;
                case skill_effect_timing.after_use:
                    text = "발동 이후 " + text;
                    break;
            }

            if (effect.code == skill_effect_code.none) { text = ""; }
            effectTMP.text = text;
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
        for (int i = 0; i < target_card.Data.Effects.Count; i++) 
        {
            EffectTextPool[i].activate();
            EffectTextPool[i].set_effect_text(target_card.Data.Effects[i]);
        }
    }

    public card get_target() // 타겟 리턴해줌
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
