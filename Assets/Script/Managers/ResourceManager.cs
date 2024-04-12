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

    public void read_Json_file() //데이터를 Json파일에서 불러옴
    {
        JsonUtility.FromJsonOverwrite(File.ReadAllText(Application.dataPath + "/Data/resource_data.json"), instance);
    }

    public void write_Json_file() //데이터를 Json파일로 저장
    {
        string output = JsonUtility.ToJson(instance, true);
        File.WriteAllText(Application.dataPath + "/Data/resource_data.json", output);
    }

    public int get_resource(resources type) // 자원 수 얻기
    {
        switch (type)
        {
            case resources.gold:
                return gold;
            default:
                Debug.Log("찾는 값이 메소드에서 정의되지 않았습니다");
                return 0;
        }
    }

    private void set_resource(resources type, int value) // 자원 값 수정 
    {
        switch (type)
        {
            case resources.gold:
                if (value >= 0) { gold = value; } // 골드값에 음수 못넣게 하기
                else { Debug.Log("골드는 음수가 될 수 없습니다. 골드 변경이 취소되었습니다."); };
                break;
        }
        write_Json_file(); // json으로 저장
    }

    public void add_resource(resources type, int value) //자원 수 더하기
    {
        switch (type)
        {
            case resources.gold:
                set_resource(resources.gold, gold + value);
                break;
            default:
                Debug.Log("찾는 값이 메소드에서 정의되지 않았습니다");
                break;
        }
    }

    private void Start()
    {
        read_Json_file();
    }
}
