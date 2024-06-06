using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.TextCore.Text;

public class CharacterManager : Singletone<CharacterManager>
{
    // �������� �����Ͱ� ��� ��ũ���ͺ� ������Ʈ
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

    // ĳ���� �ڵ忡 ���� ���� �̸� ����
    public string get_data_path<T>(T code)
    {
        // ĳ���� �ڵ��
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

        // �� �ڵ��
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
            Vector3 spawn_pos = BattleManager.instance.playable_character_position_settings[i];

            // �÷��̾�� ĳ���� ������Ʈ ����
            GameObject obj = Instantiate(playable_character_base, spawn_pos, Quaternion.identity);

            // ĳ���� ������ �ҷ���
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(PartyManager.instance.get_charactor_code(i)), character);

            // ĳ���� �� �Ʊ� �Ǻ��ϴ� ���� ����
            character.isEnemyCharacter = false;

            // ĳ���Ͱ� ������ ������ ���� ���� ����
            character.is_in_battle = false;

            // ĳ���Ϳ� ���� UI�� ����
            BattleUI_Manager.instance.summon_UI(obj, false);

            // SPUM ������ �ҷ�����
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
            character.SPUM_unit_obj = Instantiate(spPrefab, obj.transform);
            character.SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            character.SPUM_unit_obj.transform.localScale = new Vector3(-1.3f, 1.3f, 1);

            // ĳ���� ����Ʈ�� �־���
            StageManager.instance.characters.Add(obj);

            // ĳ���� ��ȣ ����
            character.Character_index = i;

            // �������� ����â ���� ��ũ��Ʈ �־���
            obj.AddComponent<Character_On_stage_show>();
        }

        // �� ĳ���� ����
        int enemy_count = StageSettingSO.stage_Settings[stage_index].enemy_Codes.Count;
        for (int i = 0; i < enemy_count; i++)
        {

            // �� ĳ���Ͱ� ��ȯ�� ��ġ�� �ҷ���
            Vector3 spawn_pos = BattleManager.instance.enemy_character_position_settings[i];

            // �� ĳ���� ������Ʈ ����
            GameObject obj = Instantiate(enemy_character_base, spawn_pos, Quaternion.identity);

            // �� ������ �ҷ�����
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(StageSettingSO.stage_Settings[stage_index].enemy_Codes[i]), character);

            // ĳ���� �� �Ʊ� �Ǻ��ϴ� ���� ����
            character.isEnemyCharacter = true;

            // ĳ���Ͱ� ������ ������ ���� ���� ����
            character.is_in_battle = false;

            // �� AI ����
            Destroy(obj.GetComponent<EnemyAI>());

            // ĳ���� ����Ʈ�� �־���
            StageManager.instance.characters.Add(obj);

            // ĳ���Ϳ� ���� UI�� ����
            BattleUI_Manager.instance.summon_UI(obj, true);

            // SPUM ������ �ҷ�����
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
            character.SPUM_unit_obj = Instantiate(spPrefab, obj.transform);
            character.SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            character.SPUM_unit_obj.transform.localScale = new Vector3(1.3f, 1.3f, 1);
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
            Vector3 spawn_pos = BattleManager.instance.playable_character_position_settings[i];

            // �÷��̾�� ĳ���� ������Ʈ ����
            GameObject obj = Instantiate(playable_character_base, spawn_pos, Quaternion.identity);

            // �÷��̾�� ĳ���� ������Ʈ ��ȣ ����
            obj.GetComponent<Character>().Character_index = i;

            // �� �߰�
            List<card> hand = new List<card>();
            BattleManager.instance.hand_data.Add(hand);

            // ĳ���� ������ �ҷ���
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(PartyManager.instance.get_charactor_code(i)), character);

            // ĳ���� �� �Ʊ� �Ǻ��ϴ� ���� ����
            character.isEnemyCharacter = false;

            // ĳ���Ͱ� ������ ������ ���� ���� ����
            character.is_in_battle = true;

            // �÷��̾�� ĳ���� ������Ʈ�� BattleManager�� ����Ʈ�� �ֱ�
            BattleManager.instance.playable_characters.Add(obj);

            // ĳ���Ϳ� ���� UI�� ����
            BattleUI_Manager.instance.summon_UI(obj, false);

            // SPUM ������ �ҷ�����
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
            character.SPUM_unit_obj = Instantiate(spPrefab, obj.transform);
            character.SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            character.SPUM_unit_obj.transform.localScale = new Vector3(-1.3f, 1.3f, 1);
        }

        // �� ĳ���� ����
        int enemy_count = StageSettingSO.stage_Settings[stage_index].enemy_Codes.Count;
        for (int i = 0; i < enemy_count; i++)
        {

            // �� ĳ���Ͱ� ��ȯ�� ��ġ�� �ҷ���
            Vector3 spawn_pos = BattleManager.instance.enemy_character_position_settings[i];

            // �� ĳ���� ������Ʈ ����
            GameObject obj = Instantiate(enemy_character_base, spawn_pos, Quaternion.identity);

            // �� ĳ���� ������Ʈ ��ȣ ����
            obj.GetComponent<Character>().Character_index = i;

            // �� �����͸� �ҷ�����
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(StageSettingSO.stage_Settings[stage_index].enemy_Codes[i]), character);

            // ĳ���� �� �Ʊ� �Ǻ��ϴ� ���� ����
            character.isEnemyCharacter = true;

            // ĳ���Ͱ� ������ ������ ���� ���� ����
            character.is_in_battle = true;

            // �� �����͸� AI�� ����
            obj.GetComponent<EnemyAI>().enemy = character;

            // �� ĳ���� ������Ʈ�� BattleManager�� ����Ʈ�� �ֱ�
            BattleManager.instance.enemy_characters.Add(obj);

            // ĳ���Ϳ� ���� UI�� ����
            BattleUI_Manager.instance.summon_UI(obj, true);

            // SPUM ������ �ҷ�����
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
            character.SPUM_unit_obj = Instantiate(spPrefab, obj.transform);
            character.SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            character.SPUM_unit_obj.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        }

        BattleManager.instance.is_Characters_spawned = true;
    }

    public void kill_character(Character character) // ĳ���� ���̴� �޼ҵ� 
    {
        bool isEnemy = character.isEnemyCharacter;

        // UI ���ֱ�
        Destroy(character.health_bar);
        Destroy(character.willpower_bar);
        Destroy(character.panic_Sign.gameObject);
        Destroy(character.skill_power_meter.gameObject);

        // �Ʊ� ĳ���͸�
        if (!isEnemy)
        {
            // ĳ������ �� ���ֱ�
            for (int i = 0; i < BattleManager.instance.hand_data[character.Character_index].Count; i++)
            {
                CardManager.instance.Destroy_card(BattleManager.instance.hand_data[character.Character_index][i]);
            }
            BattleManager.instance.hand_data.RemoveAt(character.Character_index);

            // �� ���� ���̾����� �� �����
            if (character.Character_index == CardManager.instance.active_index)
            {
                CardManager.instance.Change_active_hand(-1);
            }

            // �Ʊ� ĳ���� ����Ʈ���� ���ֱ�
            BattleManager.instance.playable_characters.Remove(character.gameObject);

            // ���� ĳ���� �ε��� ����
            for (int i = 0; i < BattleManager.instance.playable_characters.Count; i++)
            {
                BattleManager.instance.playable_characters[i].GetComponent<Character>().Character_index = i;
            }
        }

        else // �� ĳ���͸�
        {
            // UI ���ֱ�
            Destroy(character.skill_layoutGroup);

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
                BattleManager.instance.enemy_characters[i].GetComponent<Character>().Character_index = i;
            }
        }


        // ĳ���� ������Ʈ ���ֱ�
        Destroy(character.gameObject);

        // �Ʊ��̸� ��� �̺�Ʈ �ߵ� (���� ��ų �� �� ĳ���͸� Ÿ�����ϰ� �ִ� �� ������)
        if (!isEnemy)
        {
            BattleEventManager.player_character_died?.Invoke();
        }

        // ���� ������ �� ����
        if (BattleManager.instance.enemy_characters.Count == 0)
        {
            BattleEventManager.battle_ended?.Invoke(true);
        }
        else if (BattleManager.instance.playable_characters.Count == 0) 
        {
            BattleEventManager.battle_ended?.Invoke(false);
        }
    }
    public void kill_character_in_stage_show(Character character) // �������� ������ �� ĳ���� ���̴� �޼ҵ� 
    {
        bool isEnemy = character.isEnemyCharacter;

        // UI ���ֱ�
        Destroy(character.health_bar);
        Destroy(character.willpower_bar);
        Destroy(character.panic_Sign.gameObject);
        Destroy(character.skill_power_meter.gameObject);


        if (isEnemy) // �� ĳ���͸�
        {
            // UI ���ֱ�
            Destroy(character.skill_layoutGroup);
        }

        // ĳ���� ������Ʈ ���ֱ�
        Destroy(character.gameObject);

    }

}
