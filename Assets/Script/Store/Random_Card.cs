using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Random_Card : MonoBehaviour
{

    string[] Card_Grade = new string[]
    {
        "Common",
        "Rare",
        "Epic",
        "Legendary"
    };
    float[] Card_Percent = new float[4]
    {
        5.0f, 3.0f, 1.0f, 0.5f
    };
    string Card_pick(float[] _Percent , string[] _Chosen_Grade )
    {
        
        float total = 0;

        for (int i = 0; i < _Percent.Length; i++)
        {
            total += _Percent[i];
        } 

        float randomPoint = Random.value * total;

        for (int i = 0; i < _Percent.Length; i++)
        {
            if (randomPoint < _Percent[i])
            {
                return _Chosen_Grade[i];
            }
            else
            {
                randomPoint -= _Percent[i];
            }
        }
        return _Chosen_Grade[_Percent.Length - 1];
    }
    public void Randomizer_1Time()
    {
        Debug.Log(Card_pick(Card_Percent, Card_Grade));
    }
    

}
