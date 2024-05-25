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

    GameObject canvas;

    public void summon_UI(GameObject character_obj, bool isEnemy) // 캐릭터의 UI요소들 생성
    {
        canvas = GameObject.Find("Canvas");
        Character character = character_obj.GetComponent<Character>();

        GameObject health_bar;
        GameObject willpower_bar;
        // 체력바
        health_bar = Instantiate(health_bar_prefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
        health_bar.GetComponent<UI_hook_up_object>().target_object = character_obj;
        // 정신력바
        willpower_bar = Instantiate(willpower_bar_prefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
        willpower_bar.GetComponent<UI_hook_up_object>().target_object = character_obj;

        // 바 붙이기
        if (isEnemy)
        {
            character_obj.GetComponent<Character>().health_bar = health_bar;
            character.willpower_bar = willpower_bar;
            character.Set_UI_bars();
        }
        else
        {
            character.health_bar = health_bar;
            character.willpower_bar = willpower_bar;
            character.Set_UI_bars();
        }

        // 스킬 파워 표기하는 거
        GameObject skill_meter = Instantiate(skill_power_meter_prefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
        character.skill_power_meter = skill_meter.GetComponent<skill_power_meter>();
        character.skill_power_meter.Setup(character_obj);

        // 적이면 적의 스킬 슬롯 레이아웃그룹 만듦
        if (isEnemy) 
        { 
            GameObject layoutGroup = Instantiate(layoutGroup_prefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
            layoutGroup.GetComponent<UI_hook_up_object>().target_object = character_obj;
            character_obj.GetComponent<EnemyAI>().layoutGroup = layoutGroup;
        }

    }
}
