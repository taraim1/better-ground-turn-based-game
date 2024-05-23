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
    //ī����� ���
    string[] Card_Grade = new string[]
    {
        "Common",
        "Rare",
        "Epic",
        "Legendary"
    };
    //�� ��޺� Ȯ��
    float[] Card_Percent = new float[4]
    {
        5.0f, 3.0f, 1.0f, 0.5f
    };
    //ī�带 �̴� �ڵ�
    string Card_pick(float[] _Percent, string[] _Chosen_Grade)
    {
        //Ȯ�� ����� ���� Ȯ������ ���� ���
        float total = 0;

        for (int i = 0; i < _Percent.Length; i++)
        {
            total += _Percent[i];
        }
        //������ ������ ������ ����
        float randomPoint = UnityEngine.Random.value * total;
        //���� �������� �̿��Ͽ� Ȯ���� ���, � ������� ����
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
        //�������� 1�� ���ð�� Ȯ�� ����� ���� �����Ƿ� �ְ��� �̾��ֱ�
        return _Chosen_Grade[_Percent.Length - 1];
    }

    public void Randomizer_1Time()//1ȸ �̱� �ڵ�
    {
        Grade_Result = new List<string>();
        Card_Prefabs = new List<Card_UI>();
        Grade_Result.Add(Card_pick(Card_Percent, Card_Grade));

        Card_UI card_UI = Instantiate(cardPrefab, Card_Summon).GetComponent<Card_UI>();

        Card_Prefabs.Add(card_UI);

        card_UI.Card_Grade_set(Grade_Result[0]);
    }
    public void Randomizer_10Time()//10ȸ �̱� �ڵ�
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

    public void DestroyCardPrefabs() // ������ ī�� �����յ��� �����ϴ� �Լ�
    {
        foreach (var cardPrefab in Card_Prefabs)
        {
            if (cardPrefab != null)  Destroy(cardPrefab.gameObject); 
        }
        Card_Prefabs.Clear();
    }

}