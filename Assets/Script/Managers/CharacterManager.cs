using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.TextCore.Text;

public class CharacterManager : Singletone<CharacterManager>
{
    // 스테이지 데이터가 담긴 스크립터블 오브젝트
    [SerializeField] StageSettingSO StageSettingSO;
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

    // 전투하기 전 스테이지 보여줄 때 캐릭터 스폰
    public void spawn_stage_show_character(int stage_index)
    {
        // 아군 캐릭터 생성
        int party_member_count = PartyManager.instance.get_party_member_count();

        for (int i = 0; i < party_member_count; i++)
        {
            // 플레이어블 캐릭터가 소환될 위치를 불러옴
            Vector3 spawn_pos = BattleManager.instance.playable_character_position_settings[i];

            // 플레이어블 캐릭터 오브젝트 생성
            GameObject obj = Instantiate(playable_character_base, spawn_pos, Quaternion.identity);

            // 캐릭터 데이터 불러옴
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(PartyManager.instance.get_charactor_code(i)), character);

            // 캐릭터 적 아군 판별하는 변수 설정
            character.isEnemyCharacter = false;

            // 캐릭터가 전투에 쓰려고 만든 건지 설정
            character.is_in_battle = false;

            // 캐릭터에 붙은 UI들 생성
            BattleUI_Manager.instance.summon_UI(obj, false);

            // SPUM 데이터 불러오기
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
            character.SPUM_unit_obj = Instantiate(spPrefab, obj.transform);
            character.SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            character.SPUM_unit_obj.transform.localScale = new Vector3(-1.3f, 1.3f, 1);

            // 캐릭터 리스트에 넣어줌
            StageManager.instance.characters.Add(obj);

            // 캐릭터 번호 설정
            character.Character_index = i;

            // 스테이지 정보창 전용 스크립트 넣어줌
            obj.AddComponent<Character_On_stage_show>();
        }

        // 적 캐릭터 생성
        int enemy_count = StageSettingSO.stage_Settings[stage_index].enemy_Codes.Count;
        for (int i = 0; i < enemy_count; i++)
        {

            // 적 캐릭터가 소환될 위치를 불러옴
            Vector3 spawn_pos = BattleManager.instance.enemy_character_position_settings[i];

            // 적 캐릭터 오브젝트 생성
            GameObject obj = Instantiate(enemy_character_base, spawn_pos, Quaternion.identity);

            // 적 데이터 불러오기
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(StageSettingSO.stage_Settings[stage_index].enemy_Codes[i]), character);

            // 캐릭터 적 아군 판별하는 변수 설정
            character.isEnemyCharacter = true;

            // 캐릭터가 전투에 쓰려고 만든 건지 설정
            character.is_in_battle = false;

            // 적 AI 제거
            Destroy(obj.GetComponent<EnemyAI>());

            // 캐릭터 리스트에 넣어줌
            StageManager.instance.characters.Add(obj);

            // 캐릭터에 붙은 UI들 생성
            BattleUI_Manager.instance.summon_UI(obj, true);

            // SPUM 데이터 불러오기
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
            character.SPUM_unit_obj = Instantiate(spPrefab, obj.transform);
            character.SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            character.SPUM_unit_obj.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        }

    }


    // 전투 시 캐릭터 스폰
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

            // 캐릭터가 전투에 쓰려고 만든 건지 설정
            character.is_in_battle = true;

            // 플레이어블 캐릭터 오브젝트를 BattleManager의 리스트에 넣기
            BattleManager.instance.playable_characters.Add(obj);

            // 캐릭터에 붙은 UI들 생성
            BattleUI_Manager.instance.summon_UI(obj, false);

            // SPUM 데이터 불러오기
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
            character.SPUM_unit_obj = Instantiate(spPrefab, obj.transform);
            character.SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            character.SPUM_unit_obj.transform.localScale = new Vector3(-1.3f, 1.3f, 1);
        }

        // 적 캐릭터 생성
        int enemy_count = StageSettingSO.stage_Settings[stage_index].enemy_Codes.Count;
        for (int i = 0; i < enemy_count; i++)
        {

            // 적 캐릭터가 소환될 위치를 불러옴
            Vector3 spawn_pos = BattleManager.instance.enemy_character_position_settings[i];

            // 적 캐릭터 오브젝트 생성
            GameObject obj = Instantiate(enemy_character_base, spawn_pos, Quaternion.identity);

            // 적 캐릭터 오브젝트 번호 지정
            obj.GetComponent<Character>().Character_index = i;

            // 적 데이터를 불러오기
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(StageSettingSO.stage_Settings[stage_index].enemy_Codes[i]), character);

            // 캐릭터 적 아군 판별하는 변수 설정
            character.isEnemyCharacter = true;

            // 캐릭터가 전투에 쓰려고 만든 건지 설정
            character.is_in_battle = true;

            // 적 데이터를 AI에 연결
            obj.GetComponent<EnemyAI>().enemy = character;

            // 적 캐릭터 오브젝트를 BattleManager의 리스트에 넣기
            BattleManager.instance.enemy_characters.Add(obj);

            // 캐릭터에 붙은 UI들 생성
            BattleUI_Manager.instance.summon_UI(obj, true);

            // SPUM 데이터 불러오기
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
            character.SPUM_unit_obj = Instantiate(spPrefab, obj.transform);
            character.SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            character.SPUM_unit_obj.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        }

        BattleManager.instance.is_Characters_spawned = true;
    }

    public void kill_character(Character character) // 캐릭터 죽이는 메소드 
    {
        bool isEnemy = character.isEnemyCharacter;

        // UI 없애기
        Destroy(character.health_bar);
        Destroy(character.willpower_bar);
        Destroy(character.panic_Sign.gameObject);
        Destroy(character.skill_power_meter.gameObject);

        // 아군 캐릭터면
        if (!isEnemy)
        {
            // 캐릭터의 패 없애기
            for (int i = 0; i < BattleManager.instance.hand_data[character.Character_index].Count; i++)
            {
                CardManager.instance.Destroy_card(BattleManager.instance.hand_data[character.Character_index][i]);
            }
            BattleManager.instance.hand_data.RemoveAt(character.Character_index);

            // 패 보는 중이었으면 패 숨기기
            if (character.Character_index == CardManager.instance.active_index)
            {
                CardManager.instance.Change_active_hand(-1);
            }

            // 아군 캐릭터 리스트에서 없애기
            BattleManager.instance.playable_characters.Remove(character.gameObject);

            // 남은 캐릭터 인덱스 조정
            for (int i = 0; i < BattleManager.instance.playable_characters.Count; i++)
            {
                BattleManager.instance.playable_characters[i].GetComponent<Character>().Character_index = i;
            }
        }

        else // 적 캐릭터면
        {
            // UI 없애기
            Destroy(character.skill_layoutGroup);

            // 사용하는 스킬 카드 없애기
            List<GameObject> enemy_skills = character.gameObject.GetComponent<EnemyAI>().using_skill_Objects;
            for (int i = 0; i < enemy_skills.Count; i++)
            {
                CardManager.instance.Destroy_card(enemy_skills[i].GetComponent<card>());
            }

            // 적 캐릭터 리스트에서 없애기
            BattleManager.instance.enemy_characters.Remove(character.gameObject);

            // 남은 캐릭터 인덱스 조정
            for (int i = 0; i < BattleManager.instance.enemy_characters.Count; i++)
            {
                BattleManager.instance.enemy_characters[i].GetComponent<Character>().Character_index = i;
            }
        }


        // 캐릭터 오브젝트 없애기
        Destroy(character.gameObject);

        // 아군이면 사망 이벤트 발동 (적의 스킬 중 이 캐릭터를 타게팅하고 있는 걸 없애줌)
        if (!isEnemy)
        {
            BattleEventManager.player_character_died?.Invoke();
        }

        // 전투 끝나는 거 감지
        if (BattleManager.instance.enemy_characters.Count == 0)
        {
            BattleEventManager.battle_ended?.Invoke(true);
        }
        else if (BattleManager.instance.playable_characters.Count == 0) 
        {
            BattleEventManager.battle_ended?.Invoke(false);
        }
    }
    public void kill_character_in_stage_show(Character character) // 스테이지 보여줄 때 캐릭터 죽이는 메소드 
    {
        bool isEnemy = character.isEnemyCharacter;

        // UI 없애기
        Destroy(character.health_bar);
        Destroy(character.willpower_bar);
        Destroy(character.panic_Sign.gameObject);
        Destroy(character.skill_power_meter.gameObject);


        if (isEnemy) // 적 캐릭터면
        {
            // UI 없애기
            Destroy(character.skill_layoutGroup);
        }

        // 캐릭터 오브젝트 없애기
        Destroy(character.gameObject);

    }

}
