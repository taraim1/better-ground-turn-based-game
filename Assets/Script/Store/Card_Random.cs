/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using static CharacterManager;
using UnityEngine.TextCore.Text;

public class RandomData : MonoBehaviour
{
    GameObject Scene_Changer;
    public GameObject cardPrefab;
    public Transform Card_Summon;
    public List<string> Grade_Result;

    [SerializeField]
    public List<Card_UI> Card_Prefabs;

    [SerializeField]
    public List<Card_Data> Card_Results;

    [SerializeField]
    public List<string> Char_Results;

    public Card_Data card_Data;
    public Card_DataSO card_DataSO;
    //ī����� ���
    string[] Card_Grade = new string[]
    { "Common", "Rare", "Epic","Legendary"};
    //��ų ī�� ����Ʈ
    readonly string[] Skill_CommonData = { "Test1", "Test2" };
    readonly string[] Skill_RareData = { "Test1", "Test2" };
    readonly string[] Skill_EpicData = {"Test1", "Test2" };
    readonly string[] Skill_LegendData = {"Test1", "Test2" };
    //ĳ���� ī�� ����Ʈ
    readonly string[] Char_CommonData ={ "kimchunsik", "test"};
    readonly string[] Char_RareData = { "kimchunsik", "test" };
    readonly string[] Char_EpicData = { "kimchunsik", "test" };
    readonly string[] Char_LegendData = { "kimchunsik", "test" };

    
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
        //�������� 1�� ���ð�� Ȯ�� ����� ���� �����Ƿ� �ְ���� �̾��ֱ�
        return _Chosen_Grade[_Percent.Length - 1];
    }
    
    public Card_Data Card_Pick_Skill(string Grade)
    {
        //card_DataSO = GetComponent<Card_DataSO>();
        String[] selectedDatalist = null;

        //��޿� ���� ī��Ǯ ����
        switch (Grade)
        {
            case "Common": selectedDatalist = Skill_CommonData; break;
            case "Rare": selectedDatalist = Skill_RareData; break;
            case "Epic": selectedDatalist = Skill_EpicData; break;
            case "Legendary": selectedDatalist = Skill_LegendData; break;
        }

        //���� ī�� Ǯ�߿��� �ϳ� ������ ����
        if (selectedDatalist != null && selectedDatalist.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, selectedDatalist.Length);
            for (int i = 0; i < card_DataSO.CardData.Length; i++)
            {
                if (card_DataSO.CardData[i].Card_Name == selectedDatalist[randomIndex])//�̸��� �����ؿ�
                {
                    return card_DataSO.CardData[i];
                }

            }
            return null;
        }
        else return null;
    }

    public string Card_Pick_Char(string Grade)
    {
        String[] selectedDatalist = null;
        switch (Grade)
        {
            case "Common": selectedDatalist = Char_CommonData; break;
            case "Rare": selectedDatalist = Char_RareData; break;
            case "Epic": selectedDatalist = Char_EpicData; break;
            case "Legendary": selectedDatalist = Char_LegendData; break;
        }
        //���� ī�� Ǯ�߿��� �ϳ� ������ ����
        if (selectedDatalist != null && selectedDatalist.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, selectedDatalist.Length);
            return selectedDatalist[randomIndex];
        }
        else
        {
            Debug.LogError("�ƴ� ����");
            return null; 
        }

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
    public void Randomizer_10Time(string Type)//10ȸ �̱� �ڵ�
    {
        Grade_Result = new List<string>();
        Card_Results = new List<Card_Data>();
        Char_Results = new List<string>();
        Card_Prefabs = new List<Card_UI>();
        for (int i = 0; i < 10;  i++)
        {
            Card_UI card_UI = Instantiate(cardPrefab, Card_Summon).GetComponent<Card_UI>();
            Card_Prefabs.Add(card_UI);
            Grade_Result.Add(Card_Grade_pick(Card_Percent, Card_Grade));
            card_UI.Card_Grade_set(Grade_Result[i]);
            if (Type == "Skill")
            {
                Card_Results.Add(Card_Pick_Skill(Grade_Result[i]));
                if (Card_Results[i] != null)
                {
                    card_UI.Card_UI_Set(Card_Results[i]);
                }
                else
                {
                    Debug.LogError("����");
                }

            }

            else if (Type == "Char")
            {
                Char_Results.Add(Card_Pick_Char(Grade_Result[i]));
                if (Char_Results[i] != null)
                {
                    card_UI.Char_UI_Set(Char_Results[i]);
                }
                else if (Char_Results[i] == null)
                {
                    Debug.LogError("List is empty");
                }

            }
        }
        
    }
    
    
    public void Change()
    {
        Scene_Changer.GetComponent<Scene_Change>().Scene_Active(1);
    }

    delegate void _Pick_10(string type);
    _Pick_10 Pick_10;
    delegate void _Pick_1();
    _Pick_1 Pick_1;

    private void Start()
    {
        
        Scene_Changer = GameObject.Find("Scene");
        Pick_10 += (string type) => Change();
        Pick_10 += (string type) => Randomizer_10Time(type);
        Pick_1 += new _Pick_1(Change);
        Pick_1 += new _Pick_1(Randomizer_1Time);


    }
    public void Pick_10t()
    {
        if (BtnManager.Current_Scene == 1)
        {
            Pick_10("Skill");
        }
        else if (BtnManager.Current_Scene == 0)
        {
            Pick_10("Char");
        }
        else
        {
            Debug.LogError("�׾�");
        }

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
*/