using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class test : MonoBehaviour
{
    private void Start()
    {
        // 스킬 언락 확인
        print(CardManager.instance.check_unlocked(CardManager.skillcard_code.simple_attack));

        // 스킬 언락 설정 (json 자동저장됨, true면 언락된거 false면 언락 안된거)
        CardManager.instance.set_unlocked(CardManager.skillcard_code.simple_defend, false);
    }

}


