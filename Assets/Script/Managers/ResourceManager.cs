using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourceManager : Singletone<ResourceManager>
{
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
