using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChangeByName : MonoBehaviour
{
    public string SceneName;

    public void ChangeScene() 
    { 
        LoadingSceneControler.LoadScene(SceneName);
    }
}
