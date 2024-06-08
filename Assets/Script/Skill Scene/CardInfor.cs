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
    private CardManager.skillcard_code code;
    public UnityEngine.UI.Image Card_Image;
    public TMP_Text Card_Name;
    public TMP_Text Card_Cost;
    public TMP_Text Card_Elemantal_Type;
    public TMP_Text Card_Behavior_Type;
    public TMP_Text Card_Min;
    public TMP_Text Card_Max;
    public Cards carddata;
    public GameObject Popup;


    public void OnPointerDown(PointerEventData eventData)
    {
        Popup.SetActive(true);
        carddata = CardManager.instance.get_card_by_code(code);
        Card_Name.text = carddata.name;
        Card_Image.sprite = carddata.sprite;
        Card_Cost.text = carddata.cost.ToString();
        Card_Elemantal_Type.text = carddata.type;
        Card_Behavior_Type.text = carddata.behavior_type;
        Card_Min.text = carddata.minPowerOfLevel[0].ToString();
        Card_Max.text = carddata.maxPowerOfLevel[0].ToString();
    }
}


