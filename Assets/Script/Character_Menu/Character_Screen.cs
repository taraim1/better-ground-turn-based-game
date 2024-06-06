
using DG.Tweening.Plugins.Core.PathCore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using UnityEngine.UI;
public class Character_Screen : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //리스트에 해금된 캐릭터를 저장한다
        List<Character> Unlocked_Characters = Unlocked_Character();
        for(int i=0; i<Unlocked_Characters.Count; i++)
        {
            //Char_UI.Char_UI_Set ...머시기 머시기 해서 넣음 되지 않을까?
        }
        

    }
    List<Character> Unlocked_Character() //해금된 캐릭터 리스트 반환 코드
    {
        //캐릭터의 json데이터가 저장된 경로
        string dPath = "/Data/CharaterData";
        List<Character> Unlocked_Characters = new List<Character>(); //해금된 캐릭터를 저장할 경로

        Character character = new Character();
        //경로에 있는 json 파일들 경로의 모음
        string[] files = Directory.GetFiles(Application.dataPath + dPath, "*.json", SearchOption.TopDirectoryOnly);

        for (int i = 0; i < files.Length; i++)
        {
            string output = File.ReadAllText(files[i]);
            JsonUtility.FromJsonOverwrite(output, character);
            if (character.is_character_unlocked)
            {
                Unlocked_Characters.Add(character);
                //Debug.Log(Unlocked_Characters[i].description);
            }
        }
        return Unlocked_Characters;
    }
 
}
