using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class panic_sign : BattleUI.CharacterUI
{
    [SerializeField] private TMP_Text TMP;

    private void show() 
    {
        TMP.text = "ÆÐ´Ð!";
    }

    private void hide() 
    {
        TMP.text = "";
    }

    public override void Initialize(Character character)
    {
        base.Initialize(character);

        hide();
        character.panicked += show;
        character.out_of_panic += hide;
    }

    private void OnDestroy()
    {
        character.panicked -= show;
        character.out_of_panic -= hide;
    }
}
