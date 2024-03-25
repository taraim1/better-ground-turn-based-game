using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeCharacter : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("Character");// "Character"라는 이름의 씬을 로드
    }
}
