using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeSkill : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("Skill");// "Skill"라는 이름의 씬을 로드
    }
}
