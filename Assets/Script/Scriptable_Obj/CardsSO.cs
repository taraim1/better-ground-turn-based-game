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
public class CardData // 카드 데이터가 스크립터블 오브젝트로 저장되는 클래스
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
    [SerializeField] private bool isDirectUsable; // 캐릭터에 직접 사용이 가능한지 저장
    [SerializeField] private bool isSelfUsableOnly; // 자기한테 쓰는 카드인지 저장
    [SerializeField] private bool isFriendlyOnly; // 아군한테 쓰는 카드인지 저장
    [SerializeField] private bool dontShowPowerRollResult; // 스킬 쓸 때 위력 판정 결과 안 보이게 하려면 체크
    [SerializeField] private CardRangeType rangeType;
    [SerializeField] private List<coordinate> use_range; // 사용 범위
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