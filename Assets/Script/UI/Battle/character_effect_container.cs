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

    // ����Ʈ�� �޾ƿͼ� �����̳� �ʱ�ȭ
    public void InitializeContainer(character_effect_code code)
    { 
        // ������ �ҷ�����
        image.sprite = CharacterEffectManager.instance.get_icon_by_code(code);
    }

    public void OnEffectUpdated(int power)
    {
        // ���� ǥ�� ������Ʈ
        powerTmp.text = power.ToString();
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

}
