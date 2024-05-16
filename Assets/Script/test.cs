using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Rendering;

public class test : MonoBehaviour
{

    private void Start()
    {
        PartyManager.instance.remove_character_from_party(CharacterManager.character_code.kimchunsik);
        PartyManager.instance.add_character_to_party(CharacterManager.character_code.kimchunsik);
    }

}


