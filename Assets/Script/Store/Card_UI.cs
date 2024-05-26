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
    private Image Card_BackImage;
    [SerializeField]
    private Text Card_Name;
    [SerializeField]
    public Image Card_Image;

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

    public void Destroy_this()
    {
        DestroyImmediate(this.gameObject, true);
    }

    // Update is called once per frame
}
