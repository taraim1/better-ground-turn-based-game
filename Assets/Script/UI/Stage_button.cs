using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage_button : MonoBehaviour
{
    public int stage_index;

    public void Set_stage_index() 
    { 
        StageManager.instance.stage_index = stage_index;
    }

   
}
