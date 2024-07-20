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
public class Cards // 카드 데이터가 스크립터블 오브젝트로 저장되는 클래스
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

    public bool isDirectUsable; // 캐릭터에 직접 사용이 가능한지 저장
    public bool isSelfUsableOnly; // 자기한테 쓰는 카드인지 저장
    public bool isFriendlyOnly; // 아군한테 쓰는 카드인지 저장
    public bool DontShowPowerRollResult; // 스킬 쓸 때 위력 판정 결과 안 보이게 하려면 체크

    public CardRangeType rangeType;
    [SerializeField] private List<coordinate> use_range; // 사용 범위

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