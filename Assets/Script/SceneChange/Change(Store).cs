using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeStore : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("Store");// "Store"라는 이름의 씬을 로드
    }
}
