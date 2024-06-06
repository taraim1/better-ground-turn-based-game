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


    //캐릭터 코드를 뽑은 문자열에서 가져와요!
    public character_code CCFromString(string char_code)
    {
        // Enum.TryParse를 사용하여 문자열을 character_code 열거형 값으로 변환 시도
        if (Enum.TryParse(typeof(character_code), char_code, out var result))
        {
            // 변환 성공 시 해당 열거형 값을 반환
            return (character_code)result;
        }
        // 변환 실패 시 춘식이 반환
        return character_code.kimchunsik;
    }

    private void Start()
    {
        // CharacterManager.instance가 null인지 확인
        if (CharacterManager.instance == null)
        {
            Debug.LogError("CharacterManager.instance is null.");
            return;
        }

        Character character = GetComponent<Character>();
        //Character character = new Character();
        character_code Char_Code = new character_code();

        // Char_Code가 제대로 초기화되었는지 확인
        Char_Code = CCFromString("kimchunsik");
        if (Char_Code != character_code.kimchunsik)
        {
            Debug.LogError("Char_Code is not initialized correctly.");
            return;
        }

        // load_character_from_json 메서드 호출 전에 Char_Code가 유효한지 확인
        Debug.Log("Char_Code: " + Char_Code.ToString());

        // load_character_from_json 메서드 호출
        
        string json = CharacterManager.instance.load_character_from_json(Char_Code);

        // load_character_from_json의 반환 값이 null 또는 빈 문자열인지 확인
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("load_character_from_json returned null or empty string.");
            return;
        }

        // JsonUtility.FromJsonOverwrite 호출 전에 json 값이 유효한지 확인
        Debug.Log("Loaded JSON: " + json);

        // JsonUtility.FromJsonOverwrite 호출
        JsonUtility.FromJsonOverwrite(json, character);

        // character가 제대로 초기화되었는지 확인
        Debug.Log("Character Name: " + character.character_name);
    }


}