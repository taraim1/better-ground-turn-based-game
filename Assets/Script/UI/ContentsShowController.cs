using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ContentsShowController : MonoBehaviour
{

    // 서로 관련 있는 오브젝트들을 모아두고 한꺼번에 활성화/비활성화하기 위해 만듦.
    [Serializable]
    private class content 
    {
        public string contentName; // 인스펙터에서 보는용도
        public List<GameObject> gameObjects = new List<GameObject>();


        public void Activate() 
        {
            foreach (GameObject gameObject in gameObjects) 
            {
                gameObject.SetActive(true);
            }
        }
        public void Deactivate()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(false);
            }
        }
    }

    [SerializeField] private List<content> contents = new List<content>();

    // 오브젝트중 하나만 활성화되게 하는 메소드
    public void ActivateOnlyOneContent(int index) 
    {
        foreach (content content in contents) 
        {
            content.Deactivate();
        }
        contents[index].Activate();
    }

    // index번째의 content에 오브젝트 추가
    public void AddObj(int index, GameObject obj) 
    {
        contents[index].gameObjects.Add(obj);
    }
}
