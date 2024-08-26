using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Unity.VisualScripting;


public enum CardRangeType
{
    Unlimited,
    limited
}

public enum CardBehaviorType 
{ 
    attack,
    defend,
    dodge,
    etc
}

[System.Serializable]
public class CardData // ī�� �����Ͱ� ��ũ���ͺ� ������Ʈ�� ����Ǵ� Ŭ����
{
    [SerializeField] private string name;
    [SerializeField] private int cost;
    [SerializeField] private string type;
    public Sprite sprite;
    [SerializeField] private CardBehaviorType behavior_type;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int[] minPowerOfLevel;
    [SerializeField] private int[] maxPowerOfLevel;
    [SerializeField] private skillcard_code code;
    [SerializeField] private int level;
    [SerializeField] private List<skill_target> targets;
    [SerializeField] private bool isDirectUsable; // ĳ���Ϳ� ���� ����� �������� ����
    [SerializeField] private bool dontShowPowerRollResult; // ��ų �� �� ���� ���� ��� �� ���̰� �Ϸ��� üũ
    [SerializeField] private CardRangeType rangeType;
    [SerializeField] private List<coordinate> use_range; // ��� ����
    [SerializeField] private List<SkillEffect_label> effect_labels;

    public string Name => name;
    public int Cost => cost;
    public string Type => type;
    public CardBehaviorType BehaviorType => behavior_type;
    public bool IsUnlocked => isUnlocked;
    public int[] MinPowerOfLevel => minPowerOfLevel;
    public int[] MaxPowerOfLevel => maxPowerOfLevel;
    public skillcard_code Code => code;
    public int Level => level;
    public bool IsDirectUsable => isDirectUsable;
    public List<skill_target> Targets => targets;
    public bool DontShowPowerRollResult => dontShowPowerRollResult;
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
    public List<SkillEffect_label> skillEffect_Labels => effect_labels;
}

[CreateAssetMenu(fileName = "CardsDataSO", menuName = "Scriptable_Objects_CardData")]
public class CardDataSO : ScriptableObject 
{
    public CardDataSoDictonary CardData_dict;
}


[System.Serializable]
public class CardDataSoDictonary : SerializableDictionary<skillcard_code, CardData>{}