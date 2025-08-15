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

            // UI 오브젝트의 위치를 offset 추가해 설정
            rectTransform.position = target_object.transform.position + offset;
        }
    }
}
