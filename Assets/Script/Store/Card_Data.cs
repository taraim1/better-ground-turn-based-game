using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
[System.Serializable]
public class Card_Data
{
    public string Card_Name;
    //public string Card_Grade;
    public Sprite Card_Image;
    public Sprite Card_BackGround;
    
    
}

[CreateAssetMenu(fileName ="Card_datas", menuName = "Scriptable_Objects")]
public class Card_DataSO : ScriptableObject
{
    public Card_Data[] cards;
}
