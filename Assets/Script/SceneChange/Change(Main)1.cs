using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeMain1 : MonoBehaviour
{
    public void ChangeScene()
    {
        LoadingSceneControler.LoadScene("Main");
    }
}
