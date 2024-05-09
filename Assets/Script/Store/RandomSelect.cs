using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelect : MonoBehaviour
{
    public List<Random_Card> Card_Lists = new List<Random_Card>();
    public int total = 0;

    public Random_Card Random_pick()
    {
        return Card_Lists[Random.Range(0, Card_Lists.Count)];
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Card_Lists.Count; i++)
        {
            total += Card_Lists[i].weight;
        }
    }

    
}
