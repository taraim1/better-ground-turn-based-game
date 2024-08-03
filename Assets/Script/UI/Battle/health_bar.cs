using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class health_bar : BattleUI.CharacterUI_slider
{
    [SerializeField] private TMP_Text TMP;

    public override void Initialize(Character character)
    {
        base.Initialize(character);
        SetMaxValue(character.get_max_health());
        SetValue(character.Current_health);
        character.health_changed += OnHealthChanged;
    }

    private void OnHealthChanged(int value) 
    { 
        SetValue(value);
    }

    public override void SetValue(int value)
    {
        base.SetValue(value);
        TMP.text = value.ToString();
    }

    private void OnDestroy()
    {
        character.health_changed -= OnHealthChanged;
    }
}
