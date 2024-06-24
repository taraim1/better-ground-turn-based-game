using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class panic_sign : MonoBehaviour
{
    public TMP_Text tmp;


    public void Setup(GameObject target_obj) // 처음 생성시 설정용
    {
        tmp.text = "";
    }

    public void show() 
    {
        tmp.text = "패닉!";
    }

    public void hide() 
    {
        tmp.text = "";
    }
}
