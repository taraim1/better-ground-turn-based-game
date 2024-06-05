using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class StageManager : Singletone<StageManager>
{
    public int stage_index;
    // ��� ĳ���� ������Ʈ�� ��
    public List<GameObject> characters = new List<GameObject>();

    [SerializeField]
    private StageSettingSO stageSettingSO;

    private void Check_scene(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "Stage_show")
        {
            Reload_characters();
        }
    }

    // ���� �������� ���� �� �ʿ��� ���� ���� ��������
    public int get_holy_water_requirement() 
    {
        return stageSettingSO.stage_Settings[stage_index].holy_water_requirement;
    }

    // ĳ���� �����
    public void Reload_characters() 
    {

        foreach (GameObject obj in characters) 
        {
            CharacterManager.instance.kill_character_in_stage_show(obj.GetComponent<Character>());
        }
        characters.Clear();
        CharacterManager.instance.spawn_stage_show_character(stage_index);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += Check_scene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Check_scene;
    }
}
