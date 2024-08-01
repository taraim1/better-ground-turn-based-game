using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health_bar : BattleUI.CharacterUI_slider
{
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

    private void OnDestroy()
    {
        character.health_changed -= OnHealthChanged;
    }
}
