using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class test : MonoBehaviour
{
    private void Start()
    {
        // 캐릭터 인스턴스 생성
        Character character = new Character();

        // 캐릭터 코드로 캐릭터 불러오기 (예시는 춘식이)
        JsonUtility.FromJsonOverwrite(CharacterManager.instance.load_character_from_json(CharacterManager.character_code.kimchunsik), character);

        // 캐릭터에 들어있는 것들 불러오는법

        /*
        character.character_name
        character.code
        character.level 
        이런식으로 접근하면 됨

        자세히 알고싶으면 Script/Character 폴더에 있는 Character 스크립트 보세요

        */
    }

}


