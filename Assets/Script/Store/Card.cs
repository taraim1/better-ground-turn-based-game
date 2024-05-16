using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//ī����� Ŭ����, �� �������� Ʋ�� ��� �ڵ��Դϴ�...
public class Card : MonoBehaviour
{
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
