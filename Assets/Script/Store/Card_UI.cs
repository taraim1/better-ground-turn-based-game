using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class Card_UI : MonoBehaviour
{
    [SerializeField]
    public Sprite[] Card_Grade;
    [SerializeField]
    private Image Card_Image;
    [SerializeField]
    private Text Card_Name;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Card_Grade_set(string Grade)
    {
        if (Grade == "Common")
        {
            Card_Image.sprite = Card_Grade[0];
        }
        else if (Grade == "Rare")
        {
            Card_Image.sprite = Card_Grade[1];
        }
        else if (Grade == "Epic")
        {
            Card_Image.sprite = Card_Grade[2];
        }
        else if (Grade == "Legendary")
        {
            Card_Image.sprite = Card_Grade[3];
        }
    }
    public void Card_UI_Set(Card card)
    {
        if(card.Card_Grade == "Common")
        {
            Card_Image.sprite = Card_Grade[0];
        }
        else if (card.Card_Grade == "Rare")
        {
            Card_Image.sprite = Card_Grade[1];
        }
        else if (card.Card_Grade == "Epic")
        {
            Card_Image.sprite = Card_Grade[2];
        }
        else if (card.Card_Grade == "Legendary")
        {
            Card_Image.sprite = Card_Grade[3];
        }

    }

    public void Destroy_this()
    {
        DestroyImmediate(this.gameObject, true);
    }

    // Update is called once per frame
}
