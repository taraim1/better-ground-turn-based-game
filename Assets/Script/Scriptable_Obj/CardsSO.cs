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
public class CardData // ī�� �����Ͱ� ��ũ���ͺ� ������Ʈ�� ����Ǵ� Ŭ����
{
    [SerializeField] private string name;
    [SerializeField] private int cost;
    [SerializeField] private string type;
    public Sprite sprite;
    [SerializeField] private string behavior_type;
    [SerializeField] private int[] minPowerOfLevel;
    [SerializeField] private int[] maxPowerOfLevel;
    [SerializeField] private skillcard_code code;
    [SerializeField] private int level;
    [SerializeField] private bool isDirectUsable; // ĳ���Ϳ� ���� ����� �������� ����
    [SerializeField] private bool isSelfUsableOnly; // �ڱ����� ���� ī������ ����
    [SerializeField] private bool isFriendlyOnly; // �Ʊ����� ���� ī������ ����
    [SerializeField] private bool dontShowPowerRollResult; // ��ų �� �� ���� ���� ��� �� ���̰� �Ϸ��� üũ
    [SerializeField] private CardRangeType rangeType;
    [SerializeField] private List<coordinate> use_range; // ��� ����
    [SerializeField] private List<SkillEffect> effects;

    public string Name => name;
    public int Cost => cost;
    public string Type => type;
    public string BehaviorType => behavior_type;
    public int[] MinPowerOfLevel => minPowerOfLevel;
    public int[] MaxPowerOfLevel => maxPowerOfLevel;
    public skillcard_code Code => code;
    public int Level => level;
    public bool IsDirectUsable => isDirectUsable;
    public bool IsSelfUsableOnly => isSelfUsableOnly;
    public bool IsFriendlyOnly => isFriendlyOnly;
    public bool DontShowPowerRollResult => DontShowPowerRollResult;
    public CardRangeType RangeType => rangeType;
    public List<coordinate> get_copy_of_use_range() 
    {
        List<coordinate> result = new List<coordinate>();

        foreach (coordinate coordinate in use_range) 
        {
            result.Add(coordinate);
        }

        return result;
    }
    public List<SkillEffect> Effects => effects;
}

[CreateAssetMenu(fileName = "CarsSO", menuName = "Scriptable_Objects")]
public class CardDataSO : ScriptableObject 
{
    public CardDataSoDictonary CardData_dict;
}


[System.Serializable]
public class CardDataSoDictonary : SerializableDictionary<skillcard_code, CardData>{}