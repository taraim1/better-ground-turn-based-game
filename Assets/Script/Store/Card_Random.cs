using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Random_Card : MonoBehaviour
{
    
    //카드들의 등급
    string[] Card_Grade = new string[]
    {
        "Common",
        "Rare",
        "Epic",
        "Legendary"
    };
    //각 등급별 확률
    float[] Card_Percent = new float[4]
    {
        5.0f, 3.0f, 1.0f, 0.5f
    };
    //카드를 뽑는 코드
    string Card_pick(float[] _Percent, string[] _Chosen_Grade)
    {
        //확률 계산을 위한 확률들의 총합 계산
        float total = 0;

        for (int i = 0; i < _Percent.Length; i++)
        {
            total += _Percent[i];
        }
        //무엇이 나올지 랜덤값 결정
        float randomPoint = Random.value * total;
        //이후 랜덤값을 이용하여 확률을 계산, 어떤 등급인지 결정
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
        //랜덤값이 1이 나올경우 확률 계산이 되지 않으므로 최고등급 뽑아주기
        return _Chosen_Grade[_Percent.Length - 1];
    }

    public GameObject cardPrefab;
    public Transform Card_Summon;
    public List<string> Grade_Result;
    public void Randomizer_1Time()//1회 뽑기 코드
    {
        Debug.Log(Card_pick(Card_Percent, Card_Grade));
    }
    public void Randomizer_10Time()//1회 뽑기 코드
    {
        Grade_Result = new List<string>();
        for (int i = 0; i < 10;  i++)
        {
            Grade_Result.Add(Card_pick(Card_Percent, Card_Grade));
            
            Card_UI card_UI = Instantiate(cardPrefab, Card_Summon).GetComponent<Card_UI>();

            card_UI.Card_Grade_set(Grade_Result[i]);
        }
        
    }
    

}