using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class character_effect_container : BattleUI.CharacterUI
{ 
    [SerializeField] private character_effect effect;
    [SerializeField] private TMP_Text powerTmp;
    private List<Character> targets = new List<Character>();
    [SerializeField] private Image image;

    public void SetEffect(character_effect effect) // ����Ʈ �ʱ�ȭ
    { 
        this.effect = effect;
        powerTmp.text = effect.power.ToString();

        // ��������Ʈ �߰�
        switch (effect.apply_timing) 
        {
            case character_effect_timing.turn_started:
                ActionManager.turn_start_phase += use_effect_to_all_targets;
                break;
            case character_effect_timing.turn_ended:
                ActionManager.turn_end_phase += use_effect_to_all_targets;
                break;
            case character_effect_timing.after_attack:
                ActionManager.attacked += check_attack_effect_use;
                break;
        }
        switch (effect.power_reduce_timing) 
        {
            case character_effect_power_reduce_timing.turn_started:
                ActionManager.turn_start_phase += reduce_effect_power;
                break;
            case character_effect_power_reduce_timing.turn_ended:
                ActionManager.turn_end_phase += reduce_effect_power;
                break;
        }

        // Ÿ�� ����
        if (effect.target_type == character_effect_target_type.owner) 
        {
            targets.Add(character);
        }

        // ������ �ҷ�����
        image.sprite = CharacterEffectManager.instance.get_icon_by_code(effect.code);
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

    private void check_attack_effect_use(Character attacker, List<Character> targets) // ���� �� �ߵ��� ȿ�� ��� �˻�
    {
        if (attacker == character)
        {
            if (effect.target_type == character_effect_target_type.attack_target) // ȿ�� Ÿ���� ���� ����̸�
            {
                this.targets = targets; // ���� ��� �ޱ�
            }
            use_effect_to_all_targets();
        }

    }

    // ����Ʈ ���� ����
    private void reduce_effect_power()
    {
        switch (effect.code)
        {
            case character_effect_code.flame:
                effect.power = effect.power / 2;
                break;
            case character_effect_code.ignition_attack:
                effect.power = 0;
                break;
        }

        // ȿ�� ���� ����
        if (effect.power <= 0)
        {
            character.remove_effect(effect.code);
            return;
        }

        // ���� ǥ�� ������Ʈ
        powerTmp.text = effect.power.ToString();
    }

    private void use_effect_to_all_targets() // ��� Ÿ�ٿ��� ����Ʈ �ߵ� (�ϳ��� �͵�)
    {
        // ��� Ÿ�ٿ��� ����Ʈ ����
        foreach (Character target in targets) 
        {
            apply_effect(target);
        }

        // ���� ����
        if (effect.power_reduce_timing == character_effect_power_reduce_timing.used) 
        {
            reduce_effect_power();
        }

    }


    private void apply_effect(Character target) // ȿ�� �ߵ�
    {
        switch (effect.code) 
        {
            case character_effect_code.flame:
                // ���¸�ŭ ü��, ���ŷ� ������� ����
                target.Damage_health(effect.power);
                target.Damage_willpower(effect.power);
                break;
            case character_effect_code.ignition_attack:
                // ���¸�ŭ ��󿡰� ȭ���� �ο���
                target.give_effect(character_effect_code.flame, character_effect_setType.add, effect.power);
                break;
        }

    }

    public void clear_delegate_and_destroy() // �׼ǿ� �ٿ��ִ� ��������Ʈ ���� ȿ�� ���� �� �ı�
    {
        // ��������Ʈ ����
        switch (effect.apply_timing)
        {
            case character_effect_timing.turn_started:
                ActionManager.turn_start_phase -= use_effect_to_all_targets;
                break;
            case character_effect_timing.turn_ended:
                ActionManager.turn_end_phase -= use_effect_to_all_targets;
                break;
            case character_effect_timing.after_attack:
                ActionManager.attacked -= check_attack_effect_use;
                break;
        }
        switch (effect.power_reduce_timing)
        {
            case character_effect_power_reduce_timing.turn_started:
                ActionManager.turn_start_phase -= reduce_effect_power;
                break;
            case character_effect_power_reduce_timing.turn_ended:
                ActionManager.turn_end_phase -= reduce_effect_power;
                break;
        }

        Destroy(gameObject);
    }


    public character_effect_code Get_effect_code() 
    {
        return effect.code;
    }

}
