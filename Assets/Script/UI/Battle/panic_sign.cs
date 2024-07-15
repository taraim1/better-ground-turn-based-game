using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class panic_sign : MonoBehaviour
{
    public TMP_Text tmp;
    [SerializeField] private Character _character;

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

    private void Awake()
    {
        _character.data.panic_Sign = this;
    }
}
