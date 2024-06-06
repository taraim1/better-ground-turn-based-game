using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using static CharacterManager;
using UnityEngine.TextCore.Text;

public class Test : MonoBehaviour
{


    //ĳ���� �ڵ带 ���� ���ڿ����� �����Ϳ�!
    public character_code CCFromString(string char_code)
    {
        // Enum.TryParse�� ����Ͽ� ���ڿ��� character_code ������ ������ ��ȯ �õ�
        if (Enum.TryParse(typeof(character_code), char_code, out var result))
        {
            // ��ȯ ���� �� �ش� ������ ���� ��ȯ
            return (character_code)result;
        }
        // ��ȯ ���� �� ����� ��ȯ
        return character_code.kimchunsik;
    }

    private void Start()
    {
        // CharacterManager.instance�� null���� Ȯ��
        if (CharacterManager.instance == null)
        {
            Debug.LogError("CharacterManager.instance is null.");
            return;
        }

        Character character = GetComponent<Character>();
        //Character character = new Character();
        character_code Char_Code = new character_code();

        // Char_Code�� ����� �ʱ�ȭ�Ǿ����� Ȯ��
        Char_Code = CCFromString("kimchunsik");
        if (Char_Code != character_code.kimchunsik)
        {
            Debug.LogError("Char_Code is not initialized correctly.");
            return;
        }

        // load_character_from_json �޼��� ȣ�� ���� Char_Code�� ��ȿ���� Ȯ��
        Debug.Log("Char_Code: " + Char_Code.ToString());

        // load_character_from_json �޼��� ȣ��
        
        string json = CharacterManager.instance.load_character_from_json(Char_Code);

        // load_character_from_json�� ��ȯ ���� null �Ǵ� �� ���ڿ����� Ȯ��
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("load_character_from_json returned null or empty string.");
            return;
        }

        // JsonUtility.FromJsonOverwrite ȣ�� ���� json ���� ��ȿ���� Ȯ��
        Debug.Log("Loaded JSON: " + json);

        // JsonUtility.FromJsonOverwrite ȣ��
        JsonUtility.FromJsonOverwrite(json, character);

        // character�� ����� �ʱ�ȭ�Ǿ����� Ȯ��
        Debug.Log("Character Name: " + character.character_name);
    }


}