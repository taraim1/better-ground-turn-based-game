using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourceManager : Singletone<ResourceManager>, IJson
{
    [SerializeField]
    private int gold;

    public enum resources 
    { 
        gold
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

    public int get_resource(resources type) // �ڿ� �� ���
    {
        switch (type)
        {
            case resources.gold:
                return gold;
            default:
                Debug.Log("ã�� ���� �޼ҵ忡�� ���ǵ��� �ʾҽ��ϴ�");
                return 0;
        }
    }

    private void set_resource(resources type, int value) // �ڿ� �� ���� 
    {
        switch (type)
        {
            case resources.gold:
                if (value >= 0) { gold = value; } // ��尪�� ���� ���ְ� �ϱ�
                else { Debug.Log("���� ������ �� �� �����ϴ�. ��� ������ ��ҵǾ����ϴ�."); };
                break;
        }
        write_Json_file(); // json���� ����
    }

    public void add_resource(resources type, int value) //�ڿ� �� ���ϱ�
    {
        switch (type)
        {
            case resources.gold:
                set_resource(resources.gold, gold + value);
                break;
            default:
                Debug.Log("ã�� ���� �޼ҵ忡�� ���ǵ��� �ʾҽ��ϴ�");
                break;
        }
    }

    private void Start()
    {
        read_Json_file();
    }
}
