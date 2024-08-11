using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class CardInfor : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private skillcard_code code;
    public UnityEngine.UI.Image Card_Image;
    public TMP_Text Card_Name;
    public TMP_Text Card_Cost;
    public TMP_Text Card_Elemantal_Type;
    public TMP_Text Card_Behavior_Type;
    public TMP_Text Card_Min;
    public TMP_Text Card_Max;
    public CardData cardData;
    public GameObject Popup;


    public void OnPointerDown(PointerEventData eventData)
    {
        Popup.SetActive(true);
        cardData = CardManager.instance.getData_by_code(code);
        Card_Name.text = cardData.Name;
        Card_Image.sprite = cardData.sprite;
        Card_Cost.text = cardData.Cost.ToString();
        Card_Elemantal_Type.text = cardData.Type;
        Card_Behavior_Type.text = cardData.BehaviorType;
        Card_Min.text = cardData.MinPowerOfLevel[0].ToString();
        Card_Max.text = cardData.MaxPowerOfLevel[0].ToString();
    }
}


