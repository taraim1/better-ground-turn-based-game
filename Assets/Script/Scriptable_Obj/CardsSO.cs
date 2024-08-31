using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Unity.VisualScripting;




[System.Serializable]
[CreateAssetMenu(fileName = "CardsDataSO", menuName = "Scriptable_Objects_CardData")]
public class CardDataSO : ScriptableObject 
{
    public CardDataSoDictonary CardData_dict;
}

