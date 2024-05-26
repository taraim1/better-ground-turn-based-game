using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
//카드들의 클래스, 즉 기초적인 틀을 잡는 코드입니다...
public class Card : MonoBehaviour
{
    public TextMeshPro tmp;
    public string Card_Name;
    public string Card_Grade;
    public Sprite Card_Image;

    public Card (Card card)
    {

        this.Card_Grade = card.Card_Grade;
        this.Card_Name = card.Card_Name;
        this.Card_Image = card.Card_Image;
    }
}
