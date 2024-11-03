/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using static CharacterManager;
using UnityEngine.TextCore.Text;
using TMPro;

public class Random_Card : MonoBehaviour
{
    GameObject Scene_Changer;
    public GameObject cardPrefab;
    public Transform Card_Summon;
    public List<string> Grade_Result;
    public GameObject NoGem;

    [SerializeField]
    public List<Card_UI> Card_Prefabs;

    [SerializeField]
    public List<string> Card_Results;

    [SerializeField]
    public List<string> Char_Results;

    public Card_Data card_Data;
    public Card_DataSO card_DataSO;
    //ī����� ���
    string[] Card_Grade = new string[]
    { "Common", "Rare", "Epic","Legendary"};
    //��ų ī�� ����Ʈ
    readonly string[] Skill_Common_Card = { "simple_attack", "simple_defend" };
    readonly string[] Skill_Rare_Card = { "simple_dodge" };
    readonly string[] Skill_Epic_Card = { "powerful_attack", "fire_ball2" };
    readonly string[] Skill_Legend_Card = { "concentration", "fire_enchantment" };
    //ĳ���� ī�� ����Ʈ
    readonly string[] Char_Common_Card = { "kimchunsik", "test" };
    readonly string[] Char_Rare_Card = { "kimchunsik", "test" };
    readonly string[] Char_Epic_Card = { "kimchunsik", "test" };
    readonly string[] Char_Legend_Card = { "kimchunsik", "test" };


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

    public string Card_Pick_Skill(string Grade)
    {
        //card_DataSO = GetComponent<Card_DataSO>();
        String[] selected_cardlist = null;

        //��޿� ���� ī��Ǯ ����
        switch (Grade)
        {
            case "Common": selected_cardlist = Skill_Common_Card; break;
            case "Rare": selected_cardlist = Skill_Rare_Card; break;
            case "Epic": selected_cardlist = Skill_Epic_Card; break;
            case "Legendary": selected_cardlist = Skill_Legend_Card; break;
        }


        if (selected_cardlist != null && selected_cardlist.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, selected_cardlist.Length);
            return selected_cardlist[randomIndex];
        }
        else
        {
            Debug.LogError("�ƴ� ����");
            return null;
        }

    }

    public string Card_Pick_Char(string Grade)
    {
        String[] selected_cardlist = null;
        switch (Grade)
        {
            case "Common": selected_cardlist = Char_Common_Card; break;
            case "Rare": selected_cardlist = Char_Rare_Card; break;
            case "Epic": selected_cardlist = Char_Epic_Card; break;
            case "Legendary": selected_cardlist = Char_Legend_Card; break;
        }
        //���� ī�� Ǯ�߿��� �ϳ� ������ ����
        if (selected_cardlist != null && selected_cardlist.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, selected_cardlist.Length);
            return selected_cardlist[randomIndex];
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
        Card_Results = new List<string>();
        Char_Results = new List<string>();
        Card_Prefabs = new List<Card_UI>();
        for (int i = 0; i < 10; i++)
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

            /*else if (Type == "Char")
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
        if (ResourceManager.instance.Gem >= 10)
        {
            ResourceManager.instance.Gem -= 10;
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
        else
        {
            NoGem.SetActive(true);
        }
    }
    public void Pick_1t()
    {
        if (ResourceManager.instance.Gem >= 1)
        {
            ResourceManager.instance.Gem -= 1;
            Pick_1();
        }
        else
        {
            NoGem.SetActive(true);
        }
    }

    public void DestroyCardPrefabs() //프리팹을 파괴합니다
    {
        foreach (var cardPrefab in Card_Prefabs)
        {
            if (cardPrefab != null) Destroy(cardPrefab.gameObject);
        }
        Card_Prefabs.Clear();
    }

}
*/