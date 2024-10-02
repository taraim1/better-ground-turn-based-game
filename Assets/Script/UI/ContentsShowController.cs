using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ContentsShowController : MonoBehaviour
{

    [SerializeField] private List<GameObject> contents = new List<GameObject>();

    // 오브젝트중 하나만 활성화되게 하는 메소드
    public void ActivateContent(int index) 
    {
        foreach (GameObject content in contents) 
        {
            content.SetActive(false);
        }

        contents[index].SetActive(true);
    }
}
