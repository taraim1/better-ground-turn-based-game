using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.TextCore.Text;

public class CharacterManager : Singletone<CharacterManager>
{
    // �� �����Ͱ� ��� ��ũ���ͺ� ������Ʈ
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

            // �÷��̾�� ĳ���� ������Ʈ�� BattleManager�� ����Ʈ�� �ֱ�
            BattleManager.instance.playable_characters.Add(obj);

            // ĳ���Ϳ� ���� UI�� ����
            BattleUI_Manager.instance.summon_UI(obj, false);
        }

        // �� ĳ���� ����
        int enemy_count = enemySettingSO.enemy_Settigs[stage_index].enemy_Codes.Count;
        for (int i = 0; i < enemy_count; i++)
        {

            // �� ĳ���Ͱ� ��ȯ�� ��ġ�� �ҷ���
            Vector3 spawn_pos = BattleManager.instance.enemy_character_position_settings[i];

            // �� ĳ���� ������Ʈ ����
            GameObject obj = Instantiate(enemy_character_base, spawn_pos, Quaternion.identity);

            // �� ĳ���� ������Ʈ ��ȣ ����
            obj.GetComponent<Character>().Character_index = i;

            // �� �����͸� �ҷ��� BattleManager�� �� ����Ʈ�� �ֱ�
            Character character = obj.GetComponent<Character>();
            JsonUtility.FromJsonOverwrite(load_character_from_json(enemySettingSO.enemy_Settigs[stage_index].enemy_Codes[i]), character);

            // ĳ���� �� �Ʊ� �Ǻ��ϴ� ���� ����
            character.isEnemyCharacter = true;

            // �� �����͸� AI�� ����
            obj.GetComponent<EnemyAI>().enemy = character;

            // �� ĳ���� ������Ʈ�� BattleManager�� ����Ʈ�� �ֱ�
            BattleManager.instance.enemy_characters.Add(obj);

            // ĳ���Ϳ� ���� UI�� ����
            BattleUI_Manager.instance.summon_UI(obj, true);
        }

        BattleManager.instance.is_Characters_spawned = true;
    }

    public void kill_character(Character character) // ĳ���� ���̴� �޼ҵ� 
    {
        // UI ���ֱ�
        Destroy(character.health_bar);
        Destroy(character.willpower_bar);
        Destroy(character.panic_Sign.gameObject);

        // �Ʊ� ĳ���͸�
        if (!character.isEnemyCharacter)
        {
            // ĳ������ �� ���ֱ�
            for (int i = 0; i < BattleManager.instance.hand_data[character.Character_index].Count; i++)
            {
                CardManager.instance.Destroy_card(BattleManager.instance.hand_data[character.Character_index][i]);
            }

            // �Ʊ� ĳ���� ����Ʈ���� ���ֱ�
            BattleManager.instance.playable_characters.Remove(character.gameObject);
        }

        else // �� ĳ���͸�
        {
            // ����ϴ� ��ų ī�� ���ֱ�
            List<GameObject> enemy_skills = character.gameObject.GetComponent<EnemyAI>().using_skill_Objects;
            for (int i = 0; i < enemy_skills.Count; i++)
            {
                CardManager.instance.Destroy_card(enemy_skills[i].GetComponent<card>());
            }

            // �� ĳ���� ����Ʈ���� ���ֱ�
            BattleManager.instance.enemy_characters.Remove(character.gameObject);
        }
    
        // ĳ���� ������Ʈ ���ֱ�
        Destroy(character.gameObject);
    }
}
