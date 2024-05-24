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
            if (value >= 0) // 골드값에 음수 못넣게 하기
            { 
                gold = value;
                write_Json_file();
            } 
            else { Debug.Log("골드는 음수가 될 수 없습니다. 골드 변경이 취소되었습니다."); };
        }
    
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


    private void Start()
    {
        read_Json_file();
    }
}
