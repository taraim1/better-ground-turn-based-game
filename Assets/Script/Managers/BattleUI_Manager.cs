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


    public void summon_UI_bar(GameObject character_obj, bool isEnemy) // 캐릭터의 체력바, 정신력바 소환
    {
        Character character = character_obj.GetComponent<Character>();
        GameObject health_bar;
        GameObject willpower_bar;
        // 체력바
        health_bar = Instantiate(health_bar_prefab, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
        health_bar.GetComponent<UI_hook_up_object>().target_object = character_obj;
        // 정신력바
        willpower_bar = Instantiate(willpower_bar_prefab, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
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
    }







}
