using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Character_popup : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private TMP_Text nameTMP;

    // ĳ���� �˾� ������ ���� �ҷ���
    public void setup(Character character) 
    {
        _character = character;
        nameTMP.text = _character.character_name;
    }
}
