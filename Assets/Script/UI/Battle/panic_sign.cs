using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class panic_sign : MonoBehaviour
{
    public TMP_Text tmp;


    public void Setup(GameObject target_obj) // ó�� ������ ������
    {
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
