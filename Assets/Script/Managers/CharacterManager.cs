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
    // �������� �����Ͱ� ��� ��ũ���ͺ� ������Ʈ
    [SerializeField] StageSettingSO StageSettingSO;

    public GameObject playable_character_base;
    public GameObject enemy_character_base;
    public GameObject effect_container_prefab;


    // �����ϱ� �� �������� ������ �� ĳ���� ����
    public void spawn_stage_show_character(int stage_index)
    {
        // �Ʊ� ĳ���� ����
        int party_member_count = PartyManager.instance.get_party_member_count();

        for (int i = 0; i < party_member_count; i++)
        {
            // �÷��̾�� ĳ���Ͱ� ��ȯ�� ��ġ�� �ҷ���
            coordinate coordinate = StageSettingSO.stage_Settings[stage_index].player_spawnpoints[i];
            Vector2 spawn_pos = BattleGridManager.instance.get_tile_pos(coordinate);

            // �÷��̾�� ĳ���� ������Ʈ ����
            GameObject obj = CharacterBuilder.instance.
                IsEnemy(false).
                Code(PartyManager.instance.get_charactor_code(i)).
                Coordinate(coordinate).
                MakeSpumObj(true).
                Index(i).
                MakeHealthAndWillpowerBar(true).
                build();

            // ��ǥ ����
            obj.transform.position = spawn_pos;

            // ĳ���� ����Ʈ�� �־���
            StageManager.instance.characters.Add(obj);

            // �������� ����â ���� ��ũ��Ʈ �־���
            obj.AddComponent<Character_On_stage_show>();

            // �� Ÿ�� ����
            BattleGridManager.instance.set_tile_type(coordinate, BattleGridManager.boardCell.player);
        }

        // �� ĳ���� ����
        int enemy_count = StageSettingSO.stage_Settings[stage_index].enemy_codes.Count;
        for (int i = 0; i < enemy_count; i++)
        {

            // �� ĳ���Ͱ� ��ȯ�� ��ġ�� �ҷ���
            coordinate coordinate = StageSettingSO.stage_Settings[stage_index].enemy_spawnpoints[i];
            Vector2 spawn_pos = BattleGridManager.instance.get_tile_pos(coordinate);

            // �� ĳ���� ������Ʈ ����
            GameObject obj = CharacterBuilder.instance.
                IsEnemy(true).
                Code(StageSettingSO.stage_Settings[stage_index].enemy_codes[i]).
                Coordinate(coordinate).
                MakeSpumObj(true).
                Index(i).
                MakeHealthAndWillpowerBar(true).
                build();

            // ��ǥ ����
            obj.transform.position = spawn_pos;

            // ĳ���� ����Ʈ�� �־���
            StageManager.instance.characters.Add(obj);

            // �� Ÿ�� ����
            BattleGridManager.instance.set_tile_type(coordinate, BattleGridManager.boardCell.enemy);
        }

    }


    // ���� �� ĳ���� ����
    public void spawn_character(int stage_index)
    {
        // �Ʊ� ĳ���� ����
        int party_member_count = PartyManager.instance.get_party_member_count();
        for (int i = 0; i < party_member_count; i++)
        {

            // �÷��̾�� ĳ���Ͱ� ��ȯ�� ��ġ�� �ҷ���
            coordinate coordinate = StageSettingSO.stage_Settings[stage_index].player_spawnpoints[i];
            Vector2 spawn_pos = BattleGridManager.instance.get_tile_pos(coordinate);

            // �÷��̾�� ĳ���� ������Ʈ ����
            GameObject obj = CharacterBuilder.instance.
                IsEnemy(false).
                Code(PartyManager.instance.get_charactor_code(i)).
                Coordinate(coordinate).
                Index(i).
                MakeSpumObj(true).
                MakePanicSign(true).
                MakeHealthAndWillpowerBar(true).
                build();

            // �� �߰�
            List<card> hand = new List<card>();
            BattleManager.instance.hand_data.Add(hand);

            // ��ǥ ����
            obj.transform.position = spawn_pos;

            // �÷��̾�� ĳ���� ������Ʈ�� BattleManager�� ����Ʈ�� �ֱ�
            BattleManager.instance.playable_characters.Add(obj);

            // �� Ÿ�� ����
            BattleGridManager.instance.set_tile_type(coordinate, BattleGridManager.boardCell.player);
        }

        // �� ĳ���� ����
        int enemy_count = StageSettingSO.stage_Settings[stage_index].enemy_codes.Count;
        for (int i = 0; i < enemy_count; i++)
        {

            // �� ĳ���Ͱ� ��ȯ�� ��ġ�� �ҷ���
            coordinate coordinate = StageSettingSO.stage_Settings[stage_index].enemy_spawnpoints[i];
            Vector2 spawn_pos = BattleGridManager.instance.get_tile_pos(coordinate);

            // �� ĳ���� ������Ʈ ����
            GameObject obj = CharacterBuilder.instance.
                IsEnemy(true).
                Code(StageSettingSO.stage_Settings[stage_index].enemy_codes[i]).
                Coordinate(coordinate).
                Index(i).
                MakeSpumObj(true).
                MakePanicSign(true).
                MakeHealthAndWillpowerBar(true).
                build();

            // ��ǥ ����
            obj.transform.position = spawn_pos;

            // �� ĳ���� ������Ʈ�� BattleManager�� ����Ʈ�� �ֱ�
            BattleManager.instance.enemy_characters.Add(obj);

            // �� Ÿ�� ����
            BattleGridManager.instance.set_tile_type(coordinate, BattleGridManager.boardCell.enemy);
        }
    }
}
