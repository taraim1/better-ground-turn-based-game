using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class loot : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameTMP;
    [SerializeField]
    private TMP_Text quantityTMP;

    public void Set_name(string name) 
    {
        nameTMP.text = name;
    }

    public void Set_quantity(int quantity) 
    {
        quantityTMP.text = quantity.ToString();
    }
}
