using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ĳ������ ���� �ٲ��ִ� ģ��.
public class Deck_changer : MonoBehaviour
{
    [SerializeField] private CharacterDataSO dataSO;
    private Character character;
    [SerializeField] private int target_index;
    [SerializeField] private skillcard_code swap_code;

    public void change() 
    {
        if (target_index < 0) return; // �� �ٲٴ� ���

        if (dataSO.BasicData[character.Code].deck.Count <= target_index) 
        {
            print("OutOfIndex: �ٲٷ��� ��ȣ�� �ش��ϴ� �� �ڸ��� �����ϴ�.");
        }
        dataSO.BasicData[character.Code].deck[target_index] = swap_code;
    }

    public void set_character(Character character) 
    { 
        this.character = character;
    }

    public void set_index(int index) 
    {
        target_index = index;
    }

    public void set_swap_code(skillcard_code code) 
    {
        swap_code = code;
    }
}
