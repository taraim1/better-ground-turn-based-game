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
            bool isActiveScene = Scene_Num == i; //i번째 탭이 누른 탭인지 아닌지 판단.
            Scene_List[i].SetActive(isActiveScene);//모든 탭을 다 꺼준다.
            
            
        }
    }

    
    
}
