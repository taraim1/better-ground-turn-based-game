using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Random_Card : MonoBehaviour
{
    
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
        float randomPoint = Random.value * total;
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

    public GameObject cardPrefab;
    public Transform Card_Summon;
    public List<string> Grade_Result;
    public void Randomizer_1Time()//1ȸ �̱� �ڵ�
    {
        Debug.Log(Card_pick(Card_Percent, Card_Grade));
    }
    public void Randomizer_10Time()//1ȸ �̱� �ڵ�
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