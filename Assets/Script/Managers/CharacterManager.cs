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
        kimchunsik,
        test
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
        int party_member_count = PartyManager.instance.get_party_member_count();
        for (int i = 0; i < party_member_count; i++) 
        {

            // 플레이어블 캐릭터가 소환될 위치를 불러옴
            Vector3 spawn_pos = BattleManager.instance.playable_character_position_settings[i];

            // 플레이어블 캐릭터 오브젝트 생성
            GameObject obj = Instantiate(playable_character_base, spawn_pos, Quaternion.identity);

            // 파티에서 캐릭터 데이터를 불러와 BattleManager의 리스트에 넣기
            Character character = new Character();
            character = JsonUtility.FromJson<Character>(CharacterManager.instance.load_character_from_json(PartyManager.instance.get_charactor_code(i)));
            BattleManager.instance.playable_character_data.Add(character);

            // 플레이어블 캐릭터 오브젝트를 BattleManager의 리스트에 넣기
            BattleManager.instance.playable_characters.Add(obj);

        }

        BattleManager.instance.is_Characters_spawned = true;
    }
}
