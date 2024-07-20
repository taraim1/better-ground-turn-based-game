using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Runtime.CompilerServices;
using System;

public class test : MonoBehaviour
{
    [SerializeField] Character _character;
    private void Start()
    {
        JsonUtility.FromJsonOverwrite(CharacterManager.instance.load_character_from_json(CharacterManager.character_code.kimchunsik), _character);

        CharacterManager.instance.save_character_to_json(_character);

    }


}


