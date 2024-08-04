
using DG.Tweening.Plugins.Core.PathCore;
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

    List<Character> Unlocked_Characters = new List<Character>();
    void Start()
    {
        Make_Characters_display();
        
    }
    void Make_Characters_display() // 캐릭터 창에서 캐릭터들을 보여줌.
    {
        //캐릭터의 json데이터가 저장된 경로
        string dPath = "/Data/CharaterData";
        List<PlayableCharacter> Unlocked_Characters = new List<PlayableCharacter>(); //해금된 캐릭터를 저장할 경로

        //경로에 있는 json 파일들 경로의 모음
        string[] files = Directory.GetFiles(Application.dataPath + dPath, "*.json", SearchOption.TopDirectoryOnly);

        for (int i = 0; i < files.Length; i++)
        {
            GameObject Obj = Instantiate(Char_Scr_Btn_prefab, Scroll_content.transform);
            PlayableCharacter character = Obj.GetComponent<PlayableCharacter>();
            character.Load_data();
            Char_UI char_UI = Obj.GetComponent<Char_UI>();
            if (character.Is_character_unlocked)
            {
                Unlocked_Characters.Add(character);
                char_UI.character = character;
                char_UI.Char_UI_Set();
            }
            else 
            {
                Destroy(Obj);
            }
        }
    }
 
}

