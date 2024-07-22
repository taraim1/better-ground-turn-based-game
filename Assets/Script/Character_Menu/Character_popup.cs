using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Character_popup : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text levelTMP;
    [SerializeField] private Character_Levelup _levelpupClass;

    // ĳ���� �˾� ������ ���� �ҷ���
    public void setup(Character character) 
    {
        _character = character;
        nameTMP.text = _character.character_name;
        levelTMP.text = _character.level.ToString();
        _levelpupClass.setup(_character);

    }
    
}
