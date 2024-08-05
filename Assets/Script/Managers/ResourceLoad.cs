using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ResourceLoad : MonoBehaviour
{
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI GemText;
    public TextMeshProUGUI WaterText;
    void Update()
    {
        GoldText.text = "<sprite=1> : " +  ResourceManager.instance.Gold;
        GemText.text = "<sprite=2> : " +  ResourceManager.instance.Gem;
        WaterText.text = "<sprite=0> : " +  ResourceManager.instance.Water;
    }
}
