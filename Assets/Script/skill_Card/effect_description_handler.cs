using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect_description_handler : MonoBehaviour
{
    // TMP ��ũ Ŭ�� �޴� �޼ҵ�
    private void get_click(string link) 
    {
        print(link);
    }

    void OnEnable()
    {
        ActionManager.TMP_link_clicked += get_click;    
    }

    private void OnDisable()
    {
        ActionManager.TMP_link_clicked -= get_click;
    }
}
