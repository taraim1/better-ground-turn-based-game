using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class willpower_bar : BattleUI.CharacterUI_slider
{
    [SerializeField] private TMP_Text TMP;
    public override void Initialize(Character character)
    {
        base.Initialize(character);
        SetMaxValue(character.get_max_willpower());
        SetValue(character.Current_willpower);
        character.willpower_changed += OnWillpowerChanged;
    }

    private void OnWillpowerChanged(int value)
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
        character.willpower_changed -= OnWillpowerChanged;
    }
}
