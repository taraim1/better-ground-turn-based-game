using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class panic_sign : MonoBehaviour
{
    public TMP_Text tmp;
    public UI_hook_up_object hook;

    public void Setup(GameObject target_obj) // ó�� ������ ������
    {
        hook.target_object = target_obj;
        tmp.text = "";
    }

    public void show() 
    {
        tmp.text = "�д�!";
    }

    public void hide() 
    {
        tmp.text = "";
    }
}
