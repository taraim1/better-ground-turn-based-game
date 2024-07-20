using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class BattleUI_Manager : Singletone<BattleUI_Manager>
{
    public GameObject health_bar_prefab;
    public GameObject willpower_bar_prefab;
    public GameObject skill_power_meter_prefab;
    public GameObject layoutGroup_prefab;
    public GameObject panic_sign_prefab;

    GameObject canvas;


    public void Set_UI(GameObject character_obj, bool isEnemy) // 캐릭터의 UI요소들 설정
    {
        canvas = GameObject.Find("Canvas");

        Character character = character_obj.GetComponent<Character>();

        // 체력바 정신력바 등 초기화
        character.Set_UI_bars();
        
        // 스킬 파워 표기하는 거 초기화
        character.data.skill_power_meter.Setup(character_obj);

        // 패닉 사인 초기화
        character.data.panic_Sign.Setup(character_obj);

    }

}
