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

        // ��������Ʈ �߰�
        switch (effect.timing) 
        {
            case character_effect_timing.turn_started:
                BattleEventManager.turn_start_phase += apply_effect;
                break;
            case character_effect_timing.turn_ended:
                BattleEventManager.turn_end_phase += apply_effect;
                break;
        }
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

        // ���� ǥ�� ������Ʈ
        powerTmp.text = effect.power.ToString();
    }

    private void apply_effect() // ȿ�� �ߵ�
    {
        switch (effect.code) 
        {
            case character_effect_code.flame:
                // ���¸�ŭ ü��, ���ŷ� ������� ������ ������ ������ �� (�Ҽ��� ����)
                character.Damage_health(effect.power);
                character.Damage_willpower(effect.power);
                effect.power = effect.power / 2;
                break;    
        }

        // ȿ�� ���� ����
        if (effect.power <= 0) 
        {
            character.remove_effect(this);
            return;
        }

        // ���� ǥ�� ������Ʈ
        powerTmp.text = effect.power.ToString();
    }

    public void clear_delegate_and_destroy() // �׼ǿ� �ٿ��ִ� ��������Ʈ ���� ȿ�� ���� �� �ı�
    {
        // ��������Ʈ ����
        switch (effect.timing)
        {
            case character_effect_timing.turn_started:
                BattleEventManager.turn_start_phase -= apply_effect;
                break;
            case character_effect_timing.turn_ended:
                BattleEventManager.turn_end_phase -= apply_effect;
                break;
        }

        Destroy(gameObject);
    }


    public character_effect_code Get_effect_code() 
    {
        return effect.code;
    }



}
