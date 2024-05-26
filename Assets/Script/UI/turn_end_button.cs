using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class turn_end_button : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData pointerEventData) 
    {
        if (BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase) 
        {
            StartCoroutine(BattleManager.instance.enemy_skill_phase());
        }
    }
}
