using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class character_effect_container : BattleUI.CharacterUI
{ 
    [SerializeField] private TMP_Text powerTmp;
    private List<Character> targets = new List<Character>();
    [SerializeField] private Image image;

    // 이펙트를 받아와서 컨테이너 초기화
    public void InitializeContainer(character_effect_code code)
    { 
        // 아이콘 불러오기
        image.sprite = CharacterEffectManager.instance.get_icon_by_code(code);
    }

    public void OnEffectUpdated(int power)
    {
        // 숫자 표기 업데이트
        powerTmp.text = power.ToString();
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

}
