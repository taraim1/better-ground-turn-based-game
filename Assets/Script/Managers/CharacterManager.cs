using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.TextCore.Text;
using JetBrains.Annotations;
using UnityEditor.U2D.Animation;

public class CharacterManager : Singletone<CharacterManager>
{
    // 스테이지 데이터가 담긴 스크립터블 오브젝트
    [SerializeField] StageSettingSO StageSettingSO;

    public GameObject playable_character_base;
    public GameObject enemy_character_base;
    public GameObject effect_container_prefab;


    // 전투하기 전 스테이지 보여줄 때 캐릭터 스폰
    public void spawn_stage_show_character(int stage_index)
    {
        // 아군 캐릭터 생성
        int party_member_count = PartyManager.instance.get_party_member_count();

        for (int i = 0; i < party_member_count; i++)
        {
            // 플레이어블 캐릭터가 소환될 위치를 불러옴
            coordinate coordinate = StageSettingSO.stage_Settings[stage_index].player_spawnpoints[i];
            Vector2 spawn_pos = BattleGridManager.instance.get_tile_pos(coordinate);

            // 플레이어블 캐릭터 오브젝트 생성
            GameObject obj = CharacterBuilder.instance.
                IsEnemy(false).
                Code(PartyManager.instance.get_charactor_code(i)).
                Coordinate(coordinate).
                MakeSpumObjFlag(true).
                Index(i).
                build();

            // 좌표 설정
            obj.transform.position = spawn_pos;

            // 캐릭터 리스트에 넣어줌
            StageManager.instance.characters.Add(obj);

            // 스테이지 정보창 전용 스크립트 넣어줌
            obj.AddComponent<Character_On_stage_show>();

            // 셀 타입 변경
            BattleGridManager.instance.set_tile_type(coordinate, BattleGridManager.boardCell.player);
        }

        // 적 캐릭터 생성
        int enemy_count = StageSettingSO.stage_Settings[stage_index].enemy_codes.Count;
        for (int i = 0; i < enemy_count; i++)
        {

            // 적 캐릭터가 소환될 위치를 불러옴
            coordinate coordinate = StageSettingSO.stage_Settings[stage_index].enemy_spawnpoints[i];
            Vector2 spawn_pos = BattleGridManager.instance.get_tile_pos(coordinate);

            // 적 캐릭터 오브젝트 생성
             GameObject obj = CharacterBuilder.instance.
                IsEnemy(false).
                Code(StageSettingSO.stage_Settings[stage_index].enemy_codes[i]).
                Coordinate(coordinate).
                MakeSpumObjFlag(true).
                Index(i).
                build();

            // 좌표 설정
            obj.transform.position = spawn_pos;

            // 캐릭터 리스트에 넣어줌
            StageManager.instance.characters.Add(obj);

            // 셀 타입 변경
            BattleGridManager.instance.set_tile_type(coordinate, BattleGridManager.boardCell.enemy);
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
            int x = StageSettingSO.stage_Settings[stage_index].player_spawnpoints[i].x;
            int y = StageSettingSO.stage_Settings[stage_index].player_spawnpoints[i].y;
            List<float> spawn_pos = BattleGridManager.instance.get_tile_pos(x, y);

            // 플레이어블 캐릭터 오브젝트 생성
            GameObject obj = Instantiate(playable_character_base, new Vector3(spawn_pos[0], spawn_pos[1], 0f), Quaternion.identity);

            // 플레이어블 캐릭터 오브젝트 번호 지정
            obj.GetComponent<Character>().data.Character_index = i;

            // 패 추가
            List<card> hand = new List<card>();
            BattleManager.instance.hand_data.Add(hand);

            // 캐릭터 데이터 불러옴
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(PartyManager.instance.get_charactor_code(i)), character);

            // 캐릭터 좌표 설정
            character.set_coordinate(x, y);

            // 캐릭터 적 아군 판별하는 변수 설정
            character.data.isEnemyCharacter = false;

            // 캐릭터가 전투에 쓰려고 만든 건지 설정
            character.data.is_in_battle = true;

            // 플레이어블 캐릭터 오브젝트를 BattleManager의 리스트에 넣기
            BattleManager.instance.playable_characters.Add(obj);

            // 캐릭터에 붙은 UI들 생성
            BattleUI_Manager.instance.Set_UI(obj, false);

            // SPUM 데이터 불러오기
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
            character.data.SPUM_unit_obj = Instantiate(spPrefab, obj.transform);
            character.data.SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            character.data.SPUM_unit_obj.transform.localScale = new Vector3(-1.3f, 1.3f, 1);

            // 셀 타입 변경
            BattleGridManager.instance.set_tile_type(x, y, BattleGridManager.boardCell.player);
        }

        // 적 캐릭터 생성
        int enemy_count = StageSettingSO.stage_Settings[stage_index].enemy_Codes.Count;
        for (int i = 0; i < enemy_count; i++)
        {

            // 적 캐릭터가 소환될 위치를 불러옴
            int x = StageSettingSO.stage_Settings[stage_index].enemy_spawnpoints[i].x;
            int y = StageSettingSO.stage_Settings[stage_index].enemy_spawnpoints[i].y;
            List<float> spawn_pos = BattleGridManager.instance.get_tile_pos(x, y);

            // 적 캐릭터 오브젝트 생성
            GameObject obj = Instantiate(enemy_character_base, new Vector3(spawn_pos[0], spawn_pos[1], 0f), Quaternion.identity);

            // 적 캐릭터 오브젝트 번호 지정
            obj.GetComponent<Character>().data.Character_index = i;

            // 적 데이터를 불러오기
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(StageSettingSO.stage_Settings[stage_index].enemy_Codes[i]), character);

            // 캐릭터 좌표 설정
            character.set_coordinate(x, y);

            // 캐릭터 적 아군 판별하는 변수 설정
            character.data.isEnemyCharacter = true;

            // 캐릭터가 전투에 쓰려고 만든 건지 설정
            character.data.is_in_battle = true;

            // 적 캐릭터 오브젝트를 BattleManager의 리스트에 넣기
            BattleManager.instance.enemy_characters.Add(obj);

            // 캐릭터에 붙은 UI들 생성
            BattleUI_Manager.instance.Set_UI(obj, true);

            // SPUM 데이터 불러오기
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
            character.data.SPUM_unit_obj = Instantiate(spPrefab, obj.transform);
            character.data.SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            character.data.SPUM_unit_obj.transform.localScale = new Vector3(1.3f, 1.3f, 1);

            // 셀 타입 변경
            BattleGridManager.instance.set_tile_type(x, y, BattleGridManager.boardCell.enemy);
        }

        BattleManager.instance.is_Characters_spawned = true;
    }

    public void kill_character(Character character) // 캐릭터 죽이는 메소드 
    {
        bool isEnemy = character.data.isEnemyCharacter;

        // UI 없애기
        Destroy(character.data.health_bar);
        Destroy(character.data.willpower_bar);
        Destroy(character.data.panic_Sign.gameObject);
        Destroy(character.data.skill_power_meter.gameObject);

        // 아군 캐릭터면
        if (!isEnemy)
        {
            // 패 보는 중이었으면 패 숨기기
            if (character.data.Character_index == CardManager.instance.active_index)
            {
                CardManager.instance.Change_active_hand(-1);
            }

            // 캐릭터의 패 없애기
            for (int i = 0; i < BattleManager.instance.hand_data[character.data.Character_index].Count; i++)
            {
                CardManager.instance.Destroy_card(BattleManager.instance.hand_data[character.data.Character_index][i]);
            }
            BattleManager.instance.hand_data.RemoveAt(character.data.Character_index);

            // 아군 캐릭터 리스트에서 없애기
            BattleManager.instance.playable_characters.Remove(character.gameObject);

            // 남은 캐릭터 인덱스 조정
            for (int i = 0; i < BattleManager.instance.playable_characters.Count; i++)
            {
                Character remianing_character = BattleManager.instance.playable_characters[i].GetComponent<Character>();
                if (remianing_character.data.Character_index == CardManager.instance.active_index) 
                {
                    remianing_character.data.Character_index = i;
                    CardManager.instance.Change_active_hand(i);

                }
                else 
                {
                    remianing_character.data.Character_index = i;
                }
            }
        }

        else // 적 캐릭터면
        {
            // UI 없애기
            Destroy(character.data.skill_layoutGroup);

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
                BattleManager.instance.enemy_characters[i].GetComponent<Character>().data.Character_index = i;
            }
        }


        // 캐릭터 오브젝트 없애기
        Destroy(character.gameObject);

        // 아군이면 사망 이벤트 발동 (적의 스킬 중 이 캐릭터를 타게팅하고 있는 걸 없애줌)
        if (!isEnemy)
        {
            ActionManager.player_character_died?.Invoke();
        }


        // 전투 끝나는 거 감지
        if (BattleManager.instance.enemy_characters.Count == 0)
        {
            ActionManager.battle_ended?.Invoke(true);
        }
        else if (BattleManager.instance.playable_characters.Count == 0) 
        {
            ActionManager.battle_ended?.Invoke(false);
        }
    }
    public void kill_character_in_stage_show(Character character) // 스테이지 보여줄 때 캐릭터 죽이는 메소드 
    {
        bool isEnemy = character.data.isEnemyCharacter;

        // UI 없애기
        Destroy(character.data.health_bar);
        Destroy(character.data.willpower_bar);
        Destroy(character.data.panic_Sign.gameObject);
        Destroy(character.data.skill_power_meter.gameObject);


        if (isEnemy) // 적 캐릭터면
        {
            // UI 없애기
            Destroy(character.data.skill_layoutGroup);
        }

        // 캐릭터 오브젝트 없애기
        Destroy(character.gameObject);

    }

}
