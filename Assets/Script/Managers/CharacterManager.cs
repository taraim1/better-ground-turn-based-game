using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.TextCore.Text;

public class CharacterManager : Singletone<CharacterManager>
{
    // 적 데이터가 담긴 스크립터블 오브젝트
    EnemySettingSO enemy_setting_data;
    public enum character_code 
    {
        kimchunsik,
        test
    }

    public enum enemy_code 
    { 
        test
    }


    public GameObject playable_character_base;

    // 캐릭터 코드에 따른 저장 이름 리턴
    public string get_data_path<T>(T code)
    {
        // 캐릭터 코드면
        if (code.GetType() == typeof(character_code))
        {
            switch (code)
            {
                case CharacterManager.character_code.kimchunsik:
                    return "/CharaterData/CharacterData_kimchunsik.json";
                case CharacterManager.character_code.test:
                    return "/CharaterData/CharacterData_test.json";
                default:
                    return "";

            }
        }

        // 적 코드면
        if (code.GetType() == typeof(enemy_code))
        {
            switch (code)
            {
                case CharacterManager.enemy_code.test:
                    return "/EnemyData/EnemyData_test.json";
                default:
                    return "";

            }
        }

        // default
        return "";
        
    }

    // 캐릭터를 캐릭터 코드에 해당하는 json 파일에 저장
    public void save_character_to_json(Character character)
    {
        string path = "/Data";
        path += get_data_path(character.code);
        string output = JsonUtility.ToJson(character, true);
        File.WriteAllText(Application.dataPath + path, output);
    }

    // 캐릭터를 캐릭터 코드에 해당하는 json 파일에서 불러온 뒤 string으로 리턴
    public string load_character_from_json(CharacterManager.character_code code)
    {
        string path = "/Data";
        path += get_data_path(code);
        string output = File.ReadAllText(Application.dataPath + path);
        return output;
    }


    public void spawn_character(int stage_index)
    {
        // 아군 캐릭터 생성
        int party_member_count = PartyManager.instance.get_party_member_count();
        for (int i = 0; i < party_member_count; i++) 
        {

            // 플레이어블 캐릭터가 소환될 위치를 불러옴
            Vector3 spawn_pos = BattleManager.instance.playable_character_position_settings[i];

            // 플레이어블 캐릭터 오브젝트 생성
            GameObject obj = Instantiate(playable_character_base, spawn_pos, Quaternion.identity);

            // 플레이어블 캐릭터 오브젝트 번호 지정
            obj.GetComponent<Character_Obj>().Character_index = i;

            // 패 추가
            List<card> hand = new List<card>();
            BattleManager.instance.hand_data.Add(hand);

            // 파티에서 캐릭터 데이터를 불러와 BattleManager의 리스트에 넣기
            Character character = new Character();
            character = JsonUtility.FromJson<Character>(CharacterManager.instance.load_character_from_json(PartyManager.instance.get_charactor_code(i)));
            BattleManager.instance.playable_character_data.Add(character);

            // 플레이어블 캐릭터 오브젝트를 BattleManager의 리스트에 넣기
            BattleManager.instance.playable_characters.Add(obj);

        }

        // 적 캐릭터 생성
        //int enemy_count = enemy_setting_data.enemy_Settigs[stage_index].enemy_Codes.Count;

        BattleManager.instance.is_Characters_spawned = true;
    }


}
