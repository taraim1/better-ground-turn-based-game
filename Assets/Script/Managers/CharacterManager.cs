using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.TextCore.Text;

public class CharacterManager : Singletone<CharacterManager>
{
    // 적 데이터가 담긴 스크립터블 오브젝트
    [SerializeField] EnemySettingSO enemySettingSO;
    public enum character_code 
    {
        not_a_playable_character,
        kimchunsik,
        test
    }

    public enum enemy_code 
    {
        not_a_enemy_character,
        test
    }


    public GameObject playable_character_base;
    public GameObject enemy_character_base;

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
    public string load_character_from_json<T>(T code)
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
            obj.GetComponent<Character>().Character_index = i;

            // 패 추가
            List<card> hand = new List<card>();
            BattleManager.instance.hand_data.Add(hand);

            // 캐릭터 데이터 불러옴
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(PartyManager.instance.get_charactor_code(i)), character);

            // 캐릭터 적 아군 판별하는 변수 설정
            character.isEnemyCharacter = false;

            // 플레이어블 캐릭터 오브젝트를 BattleManager의 리스트에 넣기
            BattleManager.instance.playable_characters.Add(obj);

            // 캐릭터에 붙은 UI들 생성
            BattleUI_Manager.instance.summon_UI(obj, false);
        }

        // 적 캐릭터 생성
        int enemy_count = enemySettingSO.enemy_Settigs[stage_index].enemy_Codes.Count;
        for (int i = 0; i < enemy_count; i++)
        {

            // 적 캐릭터가 소환될 위치를 불러옴
            Vector3 spawn_pos = BattleManager.instance.enemy_character_position_settings[i];

            // 적 캐릭터 오브젝트 생성
            GameObject obj = Instantiate(enemy_character_base, spawn_pos, Quaternion.identity);

            // 적 캐릭터 오브젝트 번호 지정
            obj.GetComponent<Character>().Character_index = i;

            // 적 데이터를 불러와 BattleManager의 적 리스트에 넣기
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(enemySettingSO.enemy_Settigs[stage_index].enemy_Codes[i]), character);

            // 캐릭터 적 아군 판별하는 변수 설정
            character.isEnemyCharacter = true;

            // 적 데이터를 AI에 연결
            obj.GetComponent<EnemyAI>().enemy = character;

            // 적 캐릭터 오브젝트를 BattleManager의 리스트에 넣기
            BattleManager.instance.enemy_characters.Add(obj);

            // 캐릭터에 붙은 UI들 생성
            BattleUI_Manager.instance.summon_UI(obj, true);
        }

        BattleManager.instance.is_Characters_spawned = true;
    }

    public void kill_character(Character character) // 캐릭터 죽이는 메소드 
    {
        // UI 없애기
        Destroy(character.health_bar);
        Destroy(character.willpower_bar);
        Destroy(character.panic_Sign.gameObject);

        // 아군 캐릭터면
        if (!character.isEnemyCharacter)
        {
            // 캐릭터의 패 없애기
            for (int i = 0; i < BattleManager.instance.hand_data[character.Character_index].Count; i++)
            {
                CardManager.instance.Destroy_card(BattleManager.instance.hand_data[character.Character_index][i]);
            }

            // 아군 캐릭터 리스트에서 없애기
            BattleManager.instance.playable_characters.Remove(character.gameObject);
        }

        else // 적 캐릭터면
        {
            // 사용하는 스킬 카드 없애기
            List<GameObject> enemy_skills = character.gameObject.GetComponent<EnemyAI>().using_skill_Objects;
            for (int i = 0; i < enemy_skills.Count; i++)
            {
                CardManager.instance.Destroy_card(enemy_skills[i].GetComponent<card>());
            }

            // 적 캐릭터 리스트에서 없애기
            BattleManager.instance.enemy_characters.Remove(character.gameObject);
        }
    
        // 캐릭터 오브젝트 없애기
        Destroy(character.gameObject);
    }
}
