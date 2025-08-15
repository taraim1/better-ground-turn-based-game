using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Changestory : MonoBehaviour
{
    public void ChangeScene()
    {
        LoadingSceneControler.LoadScene("Story");
    }
}
