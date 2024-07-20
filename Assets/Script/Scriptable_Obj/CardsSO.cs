using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public enum CardRangeType
{
    Unlimited,
    limited
}

[System.Serializable]
public class Cards // ī�� �����Ͱ� ��ũ���ͺ� ������Ʈ�� ����Ǵ� Ŭ����
{
    [System.Serializable]
    private struct coordinate
    {
        public int x, y;
    }

    public string name;
    public int cost;
    public string type;
    public string behavior_type;
    public Sprite sprite;
    public int[] minPowerOfLevel;
    public int[] maxPowerOfLevel;

    public bool isDirectUsable; // ĳ���Ϳ� ���� ����� �������� ����
    public bool isSelfUsableOnly; // �ڱ����� ���� ī������ ����
    public bool isFriendlyOnly; // �Ʊ����� ���� ī������ ����
    public bool DontShowPowerRollResult; // ��ų �� �� ���� ���� ��� �� ���̰� �Ϸ��� üũ

    public CardRangeType rangeType;
    [SerializeField] private List<coordinate> use_range; // ��� ����

    public List<Tuple<int, int>> get_use_range() 
    {
        List<Tuple<int, int>> result = new List<Tuple<int, int>>();

        foreach (coordinate coordinate in use_range) 
        {
            result.Add(Tuple.Create(coordinate.x, coordinate.y));
        }

        return result;
    }

    public List<SkillEffect> effects;
}

[CreateAssetMenu(fileName = "CarsSO", menuName = "Scriptable_Objects")]
public class CardsSO : ScriptableObject 
{
    public CardsSoDictonary cards_dict;
}


[System.Serializable]
public class CardsSoDictonary : SerializableDictionary<skillcard_code, Cards>{}