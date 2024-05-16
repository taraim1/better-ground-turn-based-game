using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.TextCore.Text;

public class CharacterManager : Singletone<CharacterManager>
{

    public enum character_code 
    {
        kimchunsik,
        test
    }

    public GameObject playable_character_base;

    // ĳ���� �ڵ忡 ���� ���� �̸� ����
    public string get_data_path(CharacterManager.character_code code)
    {
        switch (code)
        {
            case CharacterManager.character_code.kimchunsik:
                return "kimchunsik.json";
            default:
                return "";

        }
    }

    // ĳ���͸� ĳ���� �ڵ忡 �ش��ϴ� json ���Ͽ� ����
    public void save_character_to_json(Character character)
    {
        string path = "/Data/CharacterData_";
        path += get_data_path(character.code);
        string output = JsonUtility.ToJson(character, true);
        File.WriteAllText(Application.dataPath + path, output);
    }

    // ĳ���͸� ĳ���� �ڵ忡 �ش��ϴ� json ���Ͽ��� �ҷ��� �� string���� ����
    public string load_character_from_json(CharacterManager.character_code code)
    {
        string path = "/Data/CharacterData_";
        path += get_data_path(code);
        string output = File.ReadAllText(Application.dataPath + path);
        return output;
    }


    public void spawn_character()
    {
        int party_member_count = PartyManager.instance.get_party_member_count();
        for (int i = 0; i < party_member_count; i++) 
        {

            // �÷��̾�� ĳ���Ͱ� ��ȯ�� ��ġ�� �ҷ���
            Vector3 spawn_pos = BattleManager.instance.playable_character_position_settings[i];

            // �÷��̾�� ĳ���� ������Ʈ ����
            GameObject obj = Instantiate(playable_character_base, spawn_pos, Quaternion.identity);

            // ��Ƽ���� ĳ���� �����͸� �ҷ��� BattleManager�� ����Ʈ�� �ֱ�
            Character character = new Character();
            character = JsonUtility.FromJson<Character>(CharacterManager.instance.load_character_from_json(PartyManager.instance.get_charactor_code(i)));
            BattleManager.instance.playable_character_data.Add(character);

            // �÷��̾�� ĳ���� ������Ʈ�� BattleManager�� ����Ʈ�� �ֱ�
            BattleManager.instance.playable_characters.Add(obj);

        }

        BattleManager.instance.is_Characters_spawned = true;
    }
}
