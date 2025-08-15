using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 캐릭터의 덱을 바꿔주는 친구.
public class Deck_changer : MonoBehaviour
{
    [SerializeField] private CharacterDataSO dataSO;
    private Character character;
    [SerializeField] private int target_index;
    [SerializeField] private skillcard_code swap_code;

    public void change() 
    {
        if (target_index < 0) return; // 안 바꾸는 경우

        if (dataSO.BasicData[character.Code].deck.Count <= target_index) 
        {
            print("OutOfIndex: 바꾸려는 번호에 해당하는 덱 자리가 없습니다.");
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
