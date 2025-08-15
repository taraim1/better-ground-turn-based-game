using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Search;
using Unity.Mathematics;
using Unity.VisualScripting;

public class skill_power_meter : BattleUI.CharacterUI
{
    public TMP_Text tmp;

    [SerializeField] private Character _character;

    private Coroutine running_show = null;

    public override void Initialize(Character character) 
    {
        base.Initialize(character);
        character.show_power_meter += onPowerMeterShow;
        hide();
    }


    private void onPowerMeterShow(int value) 
    {
        if (running_show != null) 
        {
            StopCoroutine(running_show);
        }

        running_show = StartCoroutine(Show(value));
    }
    private IEnumerator Show(int value) // 들어온 입력을 잠시 보여줌
    {
        tmp.text = value.ToString();
        yield return new WaitForSeconds(1f);

        running_show = null;
        hide();
        yield break;
    }

    private void hide()
    {
        tmp.text = "";
    }

    private void Awake()
    {
        ActionManager.turn_start_phase += hide;
    }
    private void OnDisable()
    {
        ActionManager.turn_start_phase -= hide;
        character.show_power_meter -= onPowerMeterShow;
    }
}
