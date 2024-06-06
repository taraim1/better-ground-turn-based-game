using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_hook_up_object : MonoBehaviour
{
    public GameObject target_object;
    public Vector3 offset; // ������Ʈ ��ġ���� �󸶳� �ٸ��� ���� ���ΰ�

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (target_object != null) 
        {

            // UI ������Ʈ�� ��ġ�� offset �߰��� ����
            rectTransform.position = target_object.transform.position + offset;
        }
    }
}
