using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourceManager : Singletone<ResourceManager>
{
    public enum resource_code 
    { 
        gold,
        gem,
        water
    }


    [SerializeField]
    private int gold;
    public int Gold
    {
        get { return gold; }

        set 
        {
            if (value >= 0) // ��尪�� ���� ���ְ� �ϱ�
            { 
                gold = value;
                write_Json_file();
            } 
            else { Debug.Log("���� ������ �� �� �����ϴ�. ��� ������ ��ҵǾ����ϴ�."); };
        }
    
    }

    [SerializeField]
    private int gem;
    public int Gem
    {
        get { return gem; }

        set 
        {
            if (value >= 0) // ��尪�� ���� ���ְ� �ϱ�
            { 
                gem = value;
                write_Json_file();
            } 
            else { Debug.Log("���� ������ �� �� �����ϴ�. ��� ������ ��ҵǾ����ϴ�."); };
        }
    
    }

    [SerializeField]
    private int water;
    public int Water
    {
        get { return water; }

        set 
        {
            if (value >= 0) // ��尪�� ���� ���ְ� �ϱ�
            { 
                water = value;
                write_Json_file();
            } 
            else { Debug.Log("���� ������ �� �� �����ϴ�. ��� ������ ��ҵǾ����ϴ�."); };
        }
    
    }

    // 코드로 값 변경
    public void SetResourceValue(resource_code resource, int value)
    {
        switch (resource)
        {
            case resource_code.gold:
                Gold = value;
                break;
            case resource_code.gem:
                Gem = value;
                break;
            case resource_code.water:
                Water = value;
                break;
        }
    }

    // 코드로 값 불러오기
    public int GetResourceValue(resource_code resource)
    {
        switch (resource)
        {
            case resource_code.gold:
                return Gold;
            case resource_code.gem:
                return Gem;
            case resource_code.water:
                return Water;
        }

        return 0;
    }

    // 코드로 자원 이름 불러오기
    public string GetResourceName(resource_code resource)
    {
        switch (resource)
        {
            case resource_code.gold:
                return "골드";
            case resource_code.gem:
                return "보석";
            case resource_code.water:
                return "성수";
        }

        return "";
    }

    public void read_Json_file() //�����͸� Json���Ͽ��� �ҷ���
    {
        JsonUtility.FromJsonOverwrite(File.ReadAllText(Application.dataPath + "/Data/resource_data.json"), instance);
    }

    public void write_Json_file() //�����͸� Json���Ϸ� ����
    {
        string output = JsonUtility.ToJson(instance, true);
        File.WriteAllText(Application.dataPath + "/Data/resource_data.json", output);
    }


    private void Start()
    {
        read_Json_file();
    }
}
