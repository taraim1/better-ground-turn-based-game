using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Rendering;

public class test : MonoBehaviour
{

    private void Start()
    {
        Character character = new Character();


        character = JsonUtility.FromJson<Character>(CharacterManager.instance.load_character_from_json(CharacterManager.character_code.kimchunsik));
        character.name = "±èÃá½Ä";
        character.code = CharacterManager.character_code.kimchunsik;

        CharacterManager.instance.save_character_to_json(character);
    }

}


