using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_hook_up_object : MonoBehaviour
{
    public GameObject target_object;
    public Vector3 offset; // 오브젝트 위치에서 얼마나 다르게 놓을 것인가

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (target_object != null) 
        {
            // 월드 오브젝트의 위치를 화면 좌표로 변환
            Vector3 obj_screen_pos = Camera.main.WorldToScreenPoint(target_object.transform.position);

            // UI 오브젝트의 위치를 offset 추가해 설정
            rectTransform.position = obj_screen_pos + offset;
        }
    }
}
