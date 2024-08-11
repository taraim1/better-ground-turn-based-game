using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using Unity.Collections.LowLevel.Unsafe;

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
    [SerializeField]
    public CardDataSO cardsSO;
    private CardData data; // 스크립터블 오브젝트로부터 카드 데이터를 가져옴
    public CardData Data => data;

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

    public void Card_UI_Set(string SCode)
    {
        if (cardsSO == null)
        {
            Debug.LogError("CardsSO is not assigned!");
            return;
        }

        
        skillcard_code skillcard_Code;

        skillcard_Code = SCFromString(SCode);
        data = CardManager.instance.getData_by_code(skillcard_Code);

        if (data != null)
        {
            this.Card_Name.text = data.Name;
            this.Card_Image.sprite = data.sprite;
        }
        else
        {
            Debug.LogWarning("Card not found!");
        }
    }

    /*public void Card_UI_Set_Character(string Code)
    {
        character_code Char_Code = CCFromString(Code);
        Character character = new Character();  // Assuming a 'Character' class exists

        JsonUtility.FromJsonOverwrite(CharacterManager.instance.load_character_from_json(Char_Code), character);

        if (character.is_character_unlocked)
        {
            Debug.Log("Character already unlocked!");
        }

        this.Card_Name.text = character.character_name;
        GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_datapath);
        character.SPUM_unit_obj = Instantiate(spPrefab, Card_Char);
        character.SPUM_unit_obj.transform.localPosition = new Vector3(0f, -40f, 1);
        character.SPUM_unit_obj.transform.localScale = new Vector3(125f, 125f, 1);
    }*/

    // Convert string to skillcard_code enum
    public static skillcard_code SCFromString(string skill_code)
    {
        if (Enum.TryParse(typeof(skillcard_code), skill_code, out var result))
        {
            return (skillcard_code)result;
        }
        return skillcard_code.simple_attack;
    }

    // Convert string to character_code enum
    public character_code CCFromString(string char_code)
    {
        if (Enum.TryParse(typeof(character_code), char_code, out var result))
        {
            return (character_code)result;
        }
        return character_code.kimchunsik;
    }

    public void Destroy_this()
    {
        DestroyImmediate(this.gameObject, true);
    }


    // Update is called once per frame
}
