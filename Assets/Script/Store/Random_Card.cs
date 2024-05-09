using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardGrade {L, E, R, N };

[System.Serializable]
public class Random_Card : MonoBehaviour
{
    [SerializeField] string cardName;
    [SerializeField] string cardImage;
    public CardGrade cardGrade;
    public int weight;

    public Random_Card(Random_Card card)
    {
        this.cardName = card.cardName;
        this.cardImage = card.cardImage;
        this.cardGrade = card.cardGrade;
        this.weight = card.weight;
    }
}
