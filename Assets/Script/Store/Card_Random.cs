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
    [SerializeField]
    public List<Card_Data> Card_Results;
    //ī����� ���
    string[] Card_Grade = new string[]
    {
        "Common",
        "Rare",
        "Epic",
        "Legendary"
    };
    //ī�� ����Ʈ
    string[] Common_Card =
    {
        "Gay",
        "Hay"
    };
    string[] Rare_Card =
    {
        "Gay",
        "Hay"
    };
    string[] Epic_Card =
    {
        "Gay",
        "Hay"
    };
    string[] Legend_Card =
    {
        "Gay",
        "Hay"
    };
    //�� ��޺� Ȯ��
    float[] Card_Percent = new float[4]
    {
        5.0f, 3.0f, 1.0f, 0.5f
    };
    
//ī�带 �̴� �ڵ�
    string Card_Grade_pick(float[] _Percent, string[] _Chosen_Grade)
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
    public Card_Data card_Data;
    public Card_DataSO card_DataSO;
    public Card_Data Card_Pick_Which(string Grade)
    {
        //card_DataSO = GetComponent<Card_DataSO>();
        String[] selected_cardlist = null;

        //��޿� ���� ī��Ǯ ����
        switch (Grade)
        {
            case "Common":
                selected_cardlist = Common_Card;
                break;
            case "Rare":
                selected_cardlist = Rare_Card;
                break;
            case "Epic":
                selected_cardlist = Epic_Card;
                break;
            case "Legendary":
                selected_cardlist = Legend_Card;
                break;
        }

        //�� ī�� Ǯ�߿��� �ϳ� ������ ����
        if (selected_cardlist != null && selected_cardlist.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, selected_cardlist.Length);
            for (int i = 0; i < card_DataSO.cards.Length; i++)
            {
                if(card_DataSO.cards[i].Card_Name == selected_cardlist[randomIndex])//�̸��� �����ؿ�
                {
                    return card_DataSO.cards[i];
                }
                
            }
            return null;
        }
        else return null;

    }

    public void Randomizer_1Time()//1ȸ �̱� �ڵ�
    {
        Grade_Result = new List<string>();
        Card_Prefabs = new List<Card_UI>();
        Grade_Result.Add(Card_Grade_pick(Card_Percent, Card_Grade));

        Card_UI card_UI = Instantiate(cardPrefab, Card_Summon).GetComponent<Card_UI>();

        Card_Prefabs.Add(card_UI);

        card_UI.Card_Grade_set(Grade_Result[0]);
    }
    public void Randomizer_10Time()//10ȸ �̱� �ڵ�
    {
        Grade_Result = new List<string>();
        Card_Results = new List<Card_Data>();
        Card_Prefabs = new List<Card_UI>();
        for (int i = 0; i < 10;  i++)
        {
            Grade_Result.Add(Card_Grade_pick(Card_Percent, Card_Grade));

            Card_Results.Add(Card_Pick_Which(Grade_Result[i]));

            Card_UI card_UI = Instantiate(cardPrefab, Card_Summon).GetComponent<Card_UI>();

            Card_Prefabs.Add(card_UI);

            card_UI.Card_Grade_set(Grade_Result[i]);
            if(Card_Results[i] != null)
            {
                card_UI.Card_UI_Set(Card_Results[i]);
            }
            else
            {
                Debug.LogError("����");
            }
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