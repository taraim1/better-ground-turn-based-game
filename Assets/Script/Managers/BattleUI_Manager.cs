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
        

        // 스킬 파워 표기하는 거 생성
        GameObject skill_meter = Instantiate(skill_power_meter_prefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
        character.skill_power_meter = skill_meter.GetComponent<skill_power_meter>();
        character.skill_power_meter.Setup(character_obj);

        // 패닉 사인 생성
        GameObject panic_sign = Instantiate(panic_sign_prefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
        character.panic_Sign = panic_sign.GetComponent<panic_sign>();
        character.panic_Sign.Setup(character_obj);

        // 적이면 적의 스킬 슬롯 레이아웃그룹 만듦
        if (isEnemy) 
        { 
            GameObject layoutGroup = Instantiate(layoutGroup_prefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
            layoutGroup.GetComponent<UI_hook_up_object>().target_object = character_obj;
            character_obj.GetComponent<EnemyAI>().layoutGroup = layoutGroup;
            character.skill_layoutGroup = layoutGroup;
        }

    }

}
