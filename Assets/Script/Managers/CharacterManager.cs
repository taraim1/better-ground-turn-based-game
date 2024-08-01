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
                MakeSpumObj(true).
                Index(i).
                MakeHealthAndWillpowerBar(true).
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
                IsEnemy(true).
                Code(StageSettingSO.stage_Settings[stage_index].enemy_codes[i]).
                Coordinate(coordinate).
                MakeSpumObj(true).
                Index(i).
                MakeHealthAndWillpowerBar(true).
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
            coordinate coordinate = StageSettingSO.stage_Settings[stage_index].player_spawnpoints[i];
            Vector2 spawn_pos = BattleGridManager.instance.get_tile_pos(coordinate);

            // 플레이어블 캐릭터 오브젝트 생성
            GameObject obj = CharacterBuilder.instance.
                IsEnemy(false).
                Code(PartyManager.instance.get_charactor_code(i)).
                Coordinate(coordinate).
                Index(i).
                MakeSpumObj(true).
                MakePanicSign(true).
                MakeHealthAndWillpowerBar(true).
                build();

            // 패 추가
            List<card> hand = new List<card>();
            BattleManager.instance.hand_data.Add(hand);

            // 좌표 설정
            obj.transform.position = spawn_pos;

            // 플레이어블 캐릭터 오브젝트를 BattleManager의 리스트에 넣기
            BattleManager.instance.playable_characters.Add(obj);

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
                IsEnemy(true).
                Code(StageSettingSO.stage_Settings[stage_index].enemy_codes[i]).
                Coordinate(coordinate).
                Index(i).
                MakeSpumObj(true).
                MakePanicSign(true).
                MakeHealthAndWillpowerBar(true).
                build();

            // 좌표 설정
            obj.transform.position = spawn_pos;

            // 적 캐릭터 오브젝트를 BattleManager의 리스트에 넣기
            BattleManager.instance.enemy_characters.Add(obj);

            // 셀 타입 변경
            BattleGridManager.instance.set_tile_type(coordinate, BattleGridManager.boardCell.enemy);
        }
    }
}
