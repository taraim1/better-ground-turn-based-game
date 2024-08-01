using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class willpower_bar : BattleUI.CharacterUI_slider
{
    public override void Initialize(Character character)
    {
        base.Initialize(character);
        SetMaxValue(character.get_max_willpower());
        SetValue(character.Current_willpower);
        character.health_changed += OnWillpowerChanged;
    }

    private void OnWillpowerChanged(int value)
    {
        SetValue(value);
    }

    private void OnDestroy()
    {
        character.health_changed -= OnWillpowerChanged;
    }
}
