using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Runtime.CompilerServices;

public class test : MonoBehaviour
{
    [SerializeField]
    private CardManager.skillcard_code code;

    private void Start()
    {
        // ĳ���� �ν��Ͻ� ����
        Character character = new Character();

        // ĳ���� �ڵ�� ĳ���� �ҷ����� (���ô� �����)
        JsonUtility.FromJsonOverwrite(CharacterManager.instance.load_character_from_json(CharacterManager.character_code.kimchunsik), character);

        // ĳ���Ϳ� ����ִ� �͵� �ҷ����¹�

        /*
        character.character_name
        character.code
        character.level 
        �̷������� �����ϸ� ��
        
        �ڼ��� �˰������ Script/Character ������ �ִ� Character ��ũ��Ʈ ������

        */
    }

}


