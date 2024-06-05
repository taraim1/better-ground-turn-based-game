using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class StageManager : Singletone<StageManager>
{
    public int stage_index;
    // 모든 캐릭터 오브젝트가 들어감
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

    // 현재 스테이지 입장 시 필요한 성수 개수 리턴해줌
    public int get_holy_water_requirement() 
    {
        return stageSettingSO.stage_Settings[stage_index].holy_water_requirement;
    }

    // 캐릭터 재생성
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
