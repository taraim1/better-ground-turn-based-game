using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���ǿ� �´� ��ų���� ã���ִ� Ŭ����.
public class Skill_search : MonoBehaviour
{
    [SerializeField] private CardDataSO dataSO;
    private CardDataSoDictonary data_dict;

    // ���ǵ�
    bool unlocked_only_flag = false;

    private void Start()
    {
        data_dict = dataSO.CardData_dict;
    }

    // ��ų �˻�
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
            // ���� Ȯ��
            if (unlocked_only_flag && !data.IsUnlocked) continue;

            skillcard_Codes.Add(code);
        }

        return skillcard_Codes;
    }

    // ��� ���� ����
    public void Reset()
    {
        unlocked_only_flag = false;
    }

    // ���� ���� �޼ҵ��
    public Skill_search unlocked(bool flag) 
    { 
        unlocked_only_flag = flag;
        return this;
    }
    
}
