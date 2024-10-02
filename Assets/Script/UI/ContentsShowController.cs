using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ContentsShowController : MonoBehaviour
{

    [SerializeField] private List<GameObject> contents = new List<GameObject>();

    // ������Ʈ�� �ϳ��� Ȱ��ȭ�ǰ� �ϴ� �޼ҵ�
    public void ActivateContent(int index) 
    {
        foreach (GameObject content in contents) 
        {
            content.SetActive(false);
        }

        contents[index].SetActive(true);
    }
}
