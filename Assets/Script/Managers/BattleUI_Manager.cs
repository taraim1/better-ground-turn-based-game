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


    public void Set_UI(GameObject character_obj, bool isEnemy) // ĳ������ UI��ҵ� ����
    {
        canvas = GameObject.Find("Canvas");

        Character character = character_obj.GetComponent<Character>();

        // ü�¹� ���ŷ¹� �� �ʱ�ȭ
        character.Set_UI_bars();
        
        // ��ų �Ŀ� ǥ���ϴ� �� �ʱ�ȭ
        character.data.skill_power_meter.Setup(character_obj);

        // �д� ���� �ʱ�ȭ
        character.data.panic_Sign.Setup(character_obj);

    }

}
