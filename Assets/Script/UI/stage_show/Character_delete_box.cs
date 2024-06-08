using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_delete_box : MonoBehaviour
{
    public Vector3 enablePos;
    public Vector3 disablePos;

    void OnCharacterDragStarted() 
    { 
        transform.position = enablePos;
    }

    void OnCharacterDragFinished() 
    {
        transform.position = disablePos;
    }

    void Start()
    {
        BattleEventManager.character_drag_started_on_stageShow += OnCharacterDragStarted;
        BattleEventManager.character_drag_finished_on_stageShow += OnCharacterDragFinished;

        transform.position = disablePos;
    }

    void OnDestroy()
    {
        BattleEventManager.character_drag_started_on_stageShow -= OnCharacterDragStarted;
        BattleEventManager.character_drag_finished_on_stageShow -= OnCharacterDragFinished;
    }
}
