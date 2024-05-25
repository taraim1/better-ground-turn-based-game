using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class test : MonoBehaviour, IPointerDownHandler
{
    public TMP_Text tmp;
   	public CardManager.skillcard_code code;
	public Cards carddata;

    public void OnPointerDown(PointerEventData eventData)
    {
        carddata = CardManager.instance.get_card_by_code(code);
        tmp.text = carddata.name;
    }
    
}


