using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.TextCore.Text;

public class CharacterManager : Singletone<CharacterManager>
{

    public enum character_code 
    {
        kimchunsik
    }

    public GameObject playable_character_base;

    // 캐릭터 코드에 따른 저장 이름 리턴
    public string get_data_path(CharacterManager.character_code code)
    {
        switch (code)
        {
            case CharacterManager.character_code.kimchunsik:
                return "kimchunsik.json";
            default:
                return "";

        }
    }

    // 캐릭터를 캐릭터 코드에 해당하는 json 파일에 저장
    public void save_character_to_json(Character character)
    {
        string path = "/Data/CharacterData_";
        path += get_data_path(character.code);
        string output = JsonUtility.ToJson(character, true);
        File.WriteAllText(Application.dataPath + path, output);
    }

    // 캐릭터를 캐릭터 코드에 해당하는 json 파일에서 불러온 뒤 string으로 리턴
    public string load_character_from_json(CharacterManager.character_code code)
    {
        string path = "/Data/CharacterData_";
        path += get_data_path(code);
        string output = File.ReadAllText(Application.dataPath + path);
        return output;
    }


    public void spawn_character()
    {
       for (int i = 0; i < 4; i++) 
        {

            //플레이어블 캐릭터가 소환될 위치를 불러옴
            Vector3 spawn_pos = BattleManager.instance.playable_character_position_settings[i];

            //플레이어블 캐릭터 생성
            GameObject obj = Instantiate(playable_character_base, spawn_pos, Quaternion.identity);

            //플레이어블 캐릭터 오브젝트를 BattleManager의 리스트에 넣기
            BattleManager.instance.playable_characters.Add(obj);

        }

        BattleManager.instance.is_Characters_spawned = true;
    }
}
