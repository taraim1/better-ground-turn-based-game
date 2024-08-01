using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using static CharacterManager;
using System;
using Unity.VisualScripting;


public class Card_UI : MonoBehaviour
{
    [SerializeField]
    public Sprite[] Card_Grade;
    [SerializeField]
    private Image Card_BackImage;
    [SerializeField]
    private Text Card_Name;
    [SerializeField]
    public Image Card_Image;
    [SerializeField]
    public Transform Card_Char;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Card_Grade_set(string Grade)
    {
        if (Grade == "Common")
        {
            Card_BackImage.sprite = Card_Grade[0];
        }
        else if (Grade == "Rare")
        {
            Card_BackImage.sprite = Card_Grade[1];
        }
        else if (Grade == "Epic")
        {
            Card_BackImage.sprite = Card_Grade[2];
        }
        else if (Grade == "Legendary")
        {
            Card_BackImage.sprite = Card_Grade[3];
        }
    }
    public void Card_UI_Set(Card_Data card)
    {
        this.Card_Name.text = card.Card_Name;
        this.Card_Image.sprite = card.Card_Image;

    }
    public void Char_UI_Set(string Code)
    {
        character_code Char_Code;
        Char_Code = CCFromString(Code);

        GameObject character_obj = CharacterBuilder.instance.Code(Char_Code).IsEnemy(false).build();
        Character character = character_obj.GetComponent<Character>();
        Card_Name.text = character.Character_name;  

    }
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
    public void Destroy_this()
    {
        DestroyImmediate(this.gameObject, true);
    }

    // Update is called once per frame
}
