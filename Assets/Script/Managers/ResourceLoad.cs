using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ResourceLoad : MonoBehaviour
{
    public TextMeshProUGUI GoldText;
    //public TextMeshProUGUI GemText;
    //public TextMeshProUGUI WaterText;
    void Update()
    {
        GoldText.text = "골드 : " +  ResourceManager.instance.Gold;
        //GemText.text = "보석 : " +  ResourceManager.instance.Gem;
        //WaterText.text = "성수 : " +  ResourceManager.instance.Water;
    }
}
