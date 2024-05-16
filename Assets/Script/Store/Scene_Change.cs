using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Change : MonoBehaviour
{
    [SerializeField]
    public GameObject[] Scene_List;

    void Start()
    {
        Scene_Active(0);
    }
    public void Scene_Active(int Scene_Num)
    {

        for (int i = 0; i < Scene_List.Length; i++)
        {
            bool isActiveScene = Scene_Num == i; //i��° ���� ���� ������ �ƴ��� �Ǵ�.
            Scene_List[i].SetActive(isActiveScene);//��� ���� �� ���ش�.
            
            
        }
    }

    
    
}
