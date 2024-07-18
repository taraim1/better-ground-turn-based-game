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
    public enum character_code
    {
        not_a_playable_character,
        kimchunsik,
        test,
        fire_mage
    }

    public enum enemy_code
    {
        not_a_enemy_character,
        test
    }


    public GameObject playable_character_base;
    public GameObject enemy_character_base;
    public GameObject effect_container_prefab;

    // ĳ���� �ڵ忡 ���� ���� �̸� ����
    public string get_data_path<T>(T code)
    {
        // ĳ���� �ڵ��
        if (code.GetType() == typeof(character_code))
        {
            switch (code)
            {
                case character_code.kimchunsik:
                    return "/CharaterData/CharacterData_kimchunsik.json";
                case character_code.test:
                    return "/CharaterData/CharacterData_test.json";
                case character_code.fire_mage:
                    return "/CharaterData/CharacterData_fire_mage.json";
                default:
                    return "";

            }
        }

        // �� �ڵ��
        if (code.GetType() == typeof(enemy_code))
        {
            switch (code)
            {
                case enemy_code.test:
                    return "/EnemyData/EnemyData_test.json";
                default:
                    return "";

            }
        }

        // default
        return "";

    }

    // ĳ���͸� ĳ���� �ڵ忡 �ش��ϴ� json ���Ͽ� ����
    public void save_character_to_json(Character character)
    {
        string path = "/Data";
        path += get_data_path(character.code);
        string output = JsonUtility.ToJson(character, true);
        File.WriteAllText(Application.dataPath + path, output);

    }

    // ĳ���͸� ĳ���� �ڵ忡 �ش��ϴ� json ���Ͽ��� �ҷ��� �� string���� ����
    public string load_character_from_json<T>(T code)
    {
        string path = "/Data";
        path += get_data_path(code);
        string output = File.ReadAllText(Application.dataPath + path);
        return output;
    }

    // �����ϱ� �� �������� ������ �� ĳ���� ����
    public void spawn_stage_show_character(int stage_index)
    {
        // �Ʊ� ĳ���� ����
        int party_member_count = PartyManager.instance.get_party_member_count();

        for (int i = 0; i < party_member_count; i++)
        {
            // �÷��̾�� ĳ���Ͱ� ��ȯ�� ��ġ�� �ҷ���
            int x = StageSettingSO.stage_Settings[stage_index].player_spawnpoints[i].x;
            int y = StageSettingSO.stage_Settings[stage_index].player_spawnpoints[i].y;
            List<float> spawn_pos = BattleGridManager.instance.get_tile_pos(x, y);

            // �÷��̾�� ĳ���� ������Ʈ ����
            GameObject obj = Instantiate(playable_character_base, new Vector3(spawn_pos[0], spawn_pos[1], 0f), Quaternion.identity);

            // ĳ���� ������ �ҷ���
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(PartyManager.instance.get_charactor_code(i)), character);

            // ĳ���� ��ǥ ����
            character.set_coordinate(x, y);

            // ĳ���� �� �Ʊ� �Ǻ��ϴ� ���� ����
            character.data.isEnemyCharacter = false;

            // ĳ���Ͱ� ������ ������ ���� ���� ����
            character.data.is_in_battle = false;

            // ĳ���Ϳ� ���� UI�� �ʱ�ȭ
            BattleUI_Manager.instance.Set_UI(obj, false);

            // SPUM ������ �ҷ�����
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
            character.data.SPUM_unit_obj = Instantiate(spPrefab, obj.transform);
            character.data.SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            character.data.SPUM_unit_obj.transform.localScale = new Vector3(-1.3f, 1.3f, 1);

            // ĳ���� ����Ʈ�� �־���
            StageManager.instance.characters.Add(obj);

            // ĳ���� ��ȣ ����
            character.data.Character_index = i;

            // �������� ����â ���� ��ũ��Ʈ �־���
            obj.AddComponent<Character_On_stage_show>();

            // �� Ÿ�� ����
            BattleGridManager.instance.set_tile_type(x, y, BattleGridManager.boardCell.player);
        }

        // �� ĳ���� ����
        int enemy_count = StageSettingSO.stage_Settings[stage_index].enemy_Codes.Count;
        for (int i = 0; i < enemy_count; i++)
        {

            // �� ĳ���Ͱ� ��ȯ�� ��ġ�� �ҷ���
            int x = StageSettingSO.stage_Settings[stage_index].enemy_spawnpoints[i].x;
            int y = StageSettingSO.stage_Settings[stage_index].enemy_spawnpoints[i].y;
            List<float> spawn_pos = BattleGridManager.instance.get_tile_pos(x, y);

            // �� ĳ���� ������Ʈ ����
            GameObject obj = Instantiate(enemy_character_base, new Vector3(spawn_pos[0], spawn_pos[1], 0f), Quaternion.identity);

            // �� ������ �ҷ�����
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(StageSettingSO.stage_Settings[stage_index].enemy_Codes[i]), character);

            // ĳ���� ��ǥ ����
            character.set_coordinate(x, y);

            // ĳ���� �� �Ʊ� �Ǻ��ϴ� ���� ����
            character.data.isEnemyCharacter = true;

            // ĳ���Ͱ� ������ ������ ���� ���� ����
            character.data.is_in_battle = false;

            // �� AI ����
            Destroy(obj.GetComponent<EnemyAI>());

            // ĳ���� ����Ʈ�� �־���
            StageManager.instance.characters.Add(obj);

            // ĳ���Ϳ� ���� UI�� �ʱ�ȭ
            BattleUI_Manager.instance.Set_UI(obj, true);

            // SPUM ������ �ҷ�����
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
            character.data.SPUM_unit_obj = Instantiate(spPrefab, obj.transform);
            character.data.SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            character.data.SPUM_unit_obj.transform.localScale = new Vector3(1.3f, 1.3f, 1);

            // �� Ÿ�� ����
            BattleGridManager.instance.set_tile_type(x, y, BattleGridManager.boardCell.enemy);
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
            int x = StageSettingSO.stage_Settings[stage_index].player_spawnpoints[i].x;
            int y = StageSettingSO.stage_Settings[stage_index].player_spawnpoints[i].y;
            List<float> spawn_pos = BattleGridManager.instance.get_tile_pos(x, y);

            // �÷��̾�� ĳ���� ������Ʈ ����
            GameObject obj = Instantiate(playable_character_base, new Vector3(spawn_pos[0], spawn_pos[1], 0f), Quaternion.identity);

            // �÷��̾�� ĳ���� ������Ʈ ��ȣ ����
            obj.GetComponent<Character>().data.Character_index = i;

            // �� �߰�
            List<card> hand = new List<card>();
            BattleManager.instance.hand_data.Add(hand);

            // ĳ���� ������ �ҷ���
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(PartyManager.instance.get_charactor_code(i)), character);

            // ĳ���� ��ǥ ����
            character.set_coordinate(x, y);

            // ĳ���� �� �Ʊ� �Ǻ��ϴ� ���� ����
            character.data.isEnemyCharacter = false;

            // ĳ���Ͱ� ������ ������ ���� ���� ����
            character.data.is_in_battle = true;

            // �÷��̾�� ĳ���� ������Ʈ�� BattleManager�� ����Ʈ�� �ֱ�
            BattleManager.instance.playable_characters.Add(obj);

            // ĳ���Ϳ� ���� UI�� ����
            BattleUI_Manager.instance.Set_UI(obj, false);

            // SPUM ������ �ҷ�����
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
            character.data.SPUM_unit_obj = Instantiate(spPrefab, obj.transform);
            character.data.SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            character.data.SPUM_unit_obj.transform.localScale = new Vector3(-1.3f, 1.3f, 1);

            // �� Ÿ�� ����
            BattleGridManager.instance.set_tile_type(x, y, BattleGridManager.boardCell.player);
        }

        // �� ĳ���� ����
        int enemy_count = StageSettingSO.stage_Settings[stage_index].enemy_Codes.Count;
        for (int i = 0; i < enemy_count; i++)
        {

            // �� ĳ���Ͱ� ��ȯ�� ��ġ�� �ҷ���
            int x = StageSettingSO.stage_Settings[stage_index].enemy_spawnpoints[i].x;
            int y = StageSettingSO.stage_Settings[stage_index].enemy_spawnpoints[i].y;
            List<float> spawn_pos = BattleGridManager.instance.get_tile_pos(x, y);

            // �� ĳ���� ������Ʈ ����
            GameObject obj = Instantiate(enemy_character_base, new Vector3(spawn_pos[0], spawn_pos[1], 0f), Quaternion.identity);

            // �� ĳ���� ������Ʈ ��ȣ ����
            obj.GetComponent<Character>().data.Character_index = i;

            // �� �����͸� �ҷ�����
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(StageSettingSO.stage_Settings[stage_index].enemy_Codes[i]), character);

            // ĳ���� ��ǥ ����
            character.set_coordinate(x, y);

            // ĳ���� �� �Ʊ� �Ǻ��ϴ� ���� ����
            character.data.isEnemyCharacter = true;

            // ĳ���Ͱ� ������ ������ ���� ���� ����
            character.data.is_in_battle = true;

            // �� �����͸� AI�� ����
            obj.GetComponent<EnemyAI>().enemy = character;

            // �� ĳ���� ������Ʈ�� BattleManager�� ����Ʈ�� �ֱ�
            BattleManager.instance.enemy_characters.Add(obj);

            // ĳ���Ϳ� ���� UI�� ����
            BattleUI_Manager.instance.Set_UI(obj, true);

            // SPUM ������ �ҷ�����
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
            character.data.SPUM_unit_obj = Instantiate(spPrefab, obj.transform);
            character.data.SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            character.data.SPUM_unit_obj.transform.localScale = new Vector3(1.3f, 1.3f, 1);

            // �� Ÿ�� ����
            BattleGridManager.instance.set_tile_type(x, y, BattleGridManager.boardCell.enemy);
        }

        BattleManager.instance.is_Characters_spawned = true;
    }

    public void kill_character(Character character) // ĳ���� ���̴� �޼ҵ� 
    {
        bool isEnemy = character.data.isEnemyCharacter;

        // UI ���ֱ�
        Destroy(character.data.health_bar);
        Destroy(character.data.willpower_bar);
        Destroy(character.data.panic_Sign.gameObject);
        Destroy(character.data.skill_power_meter.gameObject);

        // �Ʊ� ĳ���͸�
        if (!isEnemy)
        {
            // �� ���� ���̾����� �� �����
            if (character.data.Character_index == CardManager.instance.active_index)
            {
                CardManager.instance.Change_active_hand(-1);
            }

            // ĳ������ �� ���ֱ�
            for (int i = 0; i < BattleManager.instance.hand_data[character.data.Character_index].Count; i++)
            {
                CardManager.instance.Destroy_card(BattleManager.instance.hand_data[character.data.Character_index][i]);
            }
            BattleManager.instance.hand_data.RemoveAt(character.data.Character_index);

            // �Ʊ� ĳ���� ����Ʈ���� ���ֱ�
            BattleManager.instance.playable_characters.Remove(character.gameObject);

            // ���� ĳ���� �ε��� ����
            for (int i = 0; i < BattleManager.instance.playable_characters.Count; i++)
            {
                BattleManager.instance.playable_characters[i].GetComponent<Character>().data.Character_index = i;
            }
        }

        else // �� ĳ���͸�
        {
            // UI ���ֱ�
            Destroy(character.data.skill_layoutGroup);

            // ����ϴ� ��ų ī�� ���ֱ�
            List<GameObject> enemy_skills = character.gameObject.GetComponent<EnemyAI>().using_skill_Objects;
            for (int i = 0; i < enemy_skills.Count; i++)
            {
                CardManager.instance.Destroy_card(enemy_skills[i].GetComponent<card>());
            }

            // �� ĳ���� ����Ʈ���� ���ֱ�
            BattleManager.instance.enemy_characters.Remove(character.gameObject);

            // ���� ĳ���� �ε��� ����
            for (int i = 0; i < BattleManager.instance.enemy_characters.Count; i++)
            {
                BattleManager.instance.enemy_characters[i].GetComponent<Character>().data.Character_index = i;
            }
        }


        // ĳ���� ������Ʈ ���ֱ�
        Destroy(character.gameObject);

        // �Ʊ��̸� ��� �̺�Ʈ �ߵ� (���� ��ų �� �� ĳ���͸� Ÿ�����ϰ� �ִ� �� ������)
        if (!isEnemy)
        {
            ActionManager.player_character_died?.Invoke();
        }

        // ���� ������ �� ����
        if (BattleManager.instance.enemy_characters.Count == 0)
        {
            ActionManager.battle_ended?.Invoke(true);
        }
        else if (BattleManager.instance.playable_characters.Count == 0) 
        {
            ActionManager.battle_ended?.Invoke(false);
        }
    }
    public void kill_character_in_stage_show(Character character) // �������� ������ �� ĳ���� ���̴� �޼ҵ� 
    {
        bool isEnemy = character.data.isEnemyCharacter;

        // UI ���ֱ�
        Destroy(character.data.health_bar);
        Destroy(character.data.willpower_bar);
        Destroy(character.data.panic_Sign.gameObject);
        Destroy(character.data.skill_power_meter.gameObject);


        if (isEnemy) // �� ĳ���͸�
        {
            // UI ���ֱ�
            Destroy(character.data.skill_layoutGroup);
        }

        // ĳ���� ������Ʈ ���ֱ�
        Destroy(character.gameObject);

    }

}
