using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.TextCore.Text;

public class CharacterManager : Singletone<CharacterManager>
{
    // �� �����Ͱ� ��� ��ũ���ͺ� ������Ʈ
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
    public string load_character_from_json(CharacterManager.character_code code)
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
            obj.GetComponent<Character_Obj>().Character_index = i;

            // �� �߰�
            List<card> hand = new List<card>();
            BattleManager.instance.hand_data.Add(hand);

            // ��Ƽ���� ĳ���� �����͸� �ҷ��� BattleManager�� ����Ʈ�� �ֱ�
            Character character = new Character();
            character = JsonUtility.FromJson<Character>(CharacterManager.instance.load_character_from_json(PartyManager.instance.get_charactor_code(i)));
            BattleManager.instance.playable_character_data.Add(character);

            // �÷��̾�� ĳ���� ������Ʈ�� BattleManager�� ����Ʈ�� �ֱ�
            BattleManager.instance.playable_characters.Add(obj);

        }

        // �� ĳ���� ����
        //int enemy_count = enemy_setting_data.enemy_Settigs[stage_index].enemy_Codes.Count;

        BattleManager.instance.is_Characters_spawned = true;
    }


}
