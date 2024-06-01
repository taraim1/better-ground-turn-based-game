using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singletone<StageManager>
{
    public int stage_index;

    private void On_stage_show()
    {
        CharacterManager.instance.spawn_stage_show_character(stage_index);
    }

    private void Check_scene(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "Stage_show")
        {
            On_stage_show();
        }
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
