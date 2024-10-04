using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 조건에 맞는 스킬들을 찾아주는 클래스.
public class Skill_search : MonoBehaviour
{
    [SerializeField] private CardDataSO dataSO;
    private CardDataSoDictonary data_dict;

    // 조건들
    bool unlocked_only_flag = false;

    private void Start()
    {
        data_dict = dataSO.CardData_dict;
    }

    // 스킬 검색
    public List<skillcard_code> search() 
    {
        if (data_dict == null) 
        {
            data_dict = dataSO.CardData_dict;
        }

        List<skillcard_code> skillcard_Codes = new List<skillcard_code>();

        foreach (skillcard_code code in Enum.GetValues(typeof(skillcard_code))) 
        { 

            CardData data = data_dict[code];
            // 조건 확인
            if (unlocked_only_flag && !data.IsUnlocked) continue;

            skillcard_Codes.Add(code);
        }

        return skillcard_Codes;
    }

    // 모든 조건 해제
    public void Reset()
    {
        unlocked_only_flag = false;
    }

    // 조건 설정 메소드들
    public Skill_search unlocked(bool flag) 
    { 
        unlocked_only_flag = flag;
        return this;
    }
    
}
