
using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using UnityEngine.UI;
public class Character_Screen : MonoBehaviour
{

    [SerializeField]
    private GameObject Scroll_content;
    [SerializeField]
    private GameObject Char_Scr_Btn_prefab;
    [SerializeField] CharacterDataSO CharacterDataSO;

    void Start()
    {
        Make_Characters_display();   
    }
    void Make_Characters_display() // 캐릭터 창에서 캐릭터들을 보여줌.
    {
        List<character_code> Unlocked_Characters = new List<character_code>();

        foreach (character_code code in Enum.GetValues(typeof(character_code)))
        {
            if (!CharacterDataSO.PlayerData.ContainsKey(code))
            {
                continue;
            }

            if (!CharacterDataSO.PlayerData[code].is_character_unlocked) 
            {
                continue;
            }

            Unlocked_Characters.Add(code);
        }

        foreach (character_code code in Unlocked_Characters)
        {
            GameObject frameObj = Instantiate(Char_Scr_Btn_prefab, Scroll_content.transform);
            PlayableCharacter character = CharacterBuilder.instance.Code(code).build() as PlayableCharacter;
            Char_UI char_UI = frameObj.GetComponent<Char_UI>();
            char_UI.character = character;
            char_UI.Char_UI_Set();
        }
    }
 
}

