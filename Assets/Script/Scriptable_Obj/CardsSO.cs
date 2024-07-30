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

    public List<coordinate> get_copy_of_use_range() 
    {
        List<coordinate> result = new List<coordinate>();

        foreach (coordinate coordinate in use_range) 
        {
            result.Add(coordinate);
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