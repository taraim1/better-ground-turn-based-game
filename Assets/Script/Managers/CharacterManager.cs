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
        kimchunsik
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
       for (int i = 0; i < 4; i++) 
        {

            //�÷��̾�� ĳ���Ͱ� ��ȯ�� ��ġ�� �ҷ���
            Vector3 spawn_pos = BattleManager.instance.playable_character_position_settings[i];

            //�÷��̾�� ĳ���� ����
            GameObject obj = Instantiate(playable_character_base, spawn_pos, Quaternion.identity);

            //�÷��̾�� ĳ���� ������Ʈ�� BattleManager�� ����Ʈ�� �ֱ�
            BattleManager.instance.playable_characters.Add(obj);

        }

        BattleManager.instance.is_Characters_spawned = true;
    }
}
