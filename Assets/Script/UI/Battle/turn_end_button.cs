using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class turn_end_button : MonoBehaviour, IPointerDownHandler
{
    bool isBattleEnded;

    public void OnPointerDown(PointerEventData pointerEventData) 
    {
        if (isBattleEnded) { return; }

        if (BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase) 
        {
            BattleManager.instance.current_phase_coroutine = BattleManager.instance.enemy_skill_phase();
            StartCoroutine(BattleManager.instance.current_phase_coroutine);
        }
    }

    // 전투 끝났을때
    private void OnBattleEnd(bool victory)
    {
        isBattleEnded = true;
    }

    private void Awake()
    {
        ActionManager.battle_ended += OnBattleEnd;

        isBattleEnded = false;
    }

    private void OnDisable()
    {
        ActionManager.battle_ended -= OnBattleEnd;
    }
}
