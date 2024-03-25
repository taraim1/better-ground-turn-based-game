using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeMain : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("Main");// "Store"라는 이름의 씬을 로드
    }
}
