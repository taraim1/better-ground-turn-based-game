using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class character_effect_container : MonoBehaviour
{

    [SerializeField] private character_effect effect;
    [SerializeField] private TMP_Text powerTmp;
    private Character character;
    [SerializeField] private Image image;

    public void Set(character_effect effect, Character character) // �ʱ�ȭ
    { 
        this.effect = effect;
        this.character = character;
        powerTmp.text = effect.power.ToString();
    }

    public void updateEffect(int power, character_effect_setType type) // ���� ���� or �߰�
    {
        switch (type) 
        {
            case character_effect_setType.add:
                effect.power += power;
                break;
            case character_effect_setType.replace:
                effect.power = power;
                break;
        }

        powerTmp.text = effect.power.ToString();
    }

    private void apply_effect() // ȿ�� �ߵ�
    { 
        
    }

    public void clear_delegate_and_destroy() // �׼ǿ� �ٿ��ִ� ��������Ʈ ���� ȿ�� ���� �� �ı�
    { 
    
    }

    public character_effect_code Get_effect_code() 
    {
        return effect.code;
    }

}
