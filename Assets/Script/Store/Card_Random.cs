using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Random_Card : MonoBehaviour
{
    GameObject Scene_Changer;
    public GameObject cardPrefab;
    public Transform Card_Summon;
    public List<string> Grade_Result;
    [SerializeField]
    public List<Card_UI> Card_Prefabs;
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
        float randomPoint = UnityEngine.Random.value * total;
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

    public void Randomizer_1Time()//1회 뽑기 코드
    {
        Grade_Result = new List<string>();
        Card_Prefabs = new List<Card_UI>();
        Grade_Result.Add(Card_pick(Card_Percent, Card_Grade));

        Card_UI card_UI = Instantiate(cardPrefab, Card_Summon).GetComponent<Card_UI>();

        Card_Prefabs.Add(card_UI);

        card_UI.Card_Grade_set(Grade_Result[0]);
    }
    public void Randomizer_10Time()//10회 뽑기 코드
    {
        Grade_Result = new List<string>();
        Card_Prefabs = new List<Card_UI>();
        for (int i = 0; i < 10;  i++)
        {
            Grade_Result.Add(Card_pick(Card_Percent, Card_Grade));

            Card_UI card_UI = Instantiate(cardPrefab, Card_Summon).GetComponent<Card_UI>();

            Card_Prefabs.Add(card_UI);

            card_UI.Card_Grade_set(Grade_Result[i]);
        }
        
    }
    
    
    public void Change()
    {
        Scene_Changer.GetComponent<Scene_Change>().Scene_Active(1);
    }

    delegate void _Pick_10();
    _Pick_10 Pick_10;
    delegate void _Pick_1();
    _Pick_1 Pick_1;

    private void Start()
    {
        Scene_Changer = GameObject.Find("Scene");
        Pick_10 += new _Pick_10(Change);
        Pick_10 += new _Pick_10(Randomizer_10Time);
        Pick_1 += new _Pick_1(Change);
        Pick_1 += new _Pick_1(Randomizer_1Time);


    }
    public void Pick_10t()
    {
        Pick_10();
    }
    public void Pick_1t()
    {
        Pick_1();
    }

    public void DestroyCardPrefabs() // 생성된 카드 프리팹들을 제거하는 함수
    {
        foreach (var cardPrefab in Card_Prefabs)
        {
            if (cardPrefab != null)  Destroy(cardPrefab.gameObject); 
        }
        Card_Prefabs.Clear();
    }

}