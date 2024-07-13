using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Character_popup : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private TMP_Text nameTMP;

    // 캐릭터 팝업 생성시 정보 불러옴
    public void setup(Character character) 
    {
        _character = character;
        nameTMP.text = _character.character_name;
    }
}
