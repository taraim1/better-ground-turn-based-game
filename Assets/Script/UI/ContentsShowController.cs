using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ContentsShowController : MonoBehaviour
{

    // ���� ���� �ִ� ������Ʈ���� ��Ƶΰ� �Ѳ����� Ȱ��ȭ/��Ȱ��ȭ�ϱ� ���� ����.
    [Serializable]
    private class content 
    {
        public string contentName; // �ν����Ϳ��� ���¿뵵
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

    // ������Ʈ�� �ϳ��� Ȱ��ȭ�ǰ� �ϴ� �޼ҵ�
    public void ActivateOnlyOneContent(int index) 
    {
        foreach (content content in contents) 
        {
            content.Deactivate();
        }
        contents[index].Activate();
    }

    // index��°�� content�� ������Ʈ �߰�
    public void AddObj(int index, GameObject obj) 
    {
        contents[index].gameObjects.Add(obj);
    }
}
