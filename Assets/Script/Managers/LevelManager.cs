using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelManager : Singletone<LevelManager>
{
    [SerializeField]
    private int goldCaveLevel;

    public int GoldCaveLevel
    {
        get { return goldCaveLevel; }

        set 
        {
                goldCaveLevel = value;
                write_Json_file();
        }
    
    }
        [SerializeField]
    private int gemCaveLevel;

    public int GemCaveLevel
    {
        get { return gemCaveLevel; }

        set 
        {
                gemCaveLevel = value;
                write_Json_file();
        }
    
    }
        [SerializeField]
    private int wStatueLevel;

    public int WStatueLevel
    {
        get { return wStatueLevel; }

        set 
        {
                wStatueLevel = value;
                write_Json_file();
        }
    
    }

    public void read_Json_file()
    {
        JsonUtility.FromJsonOverwrite(File.ReadAllText(Application.dataPath + "/Data/level_data.json"), instance);
    }

    public void write_Json_file()
    {
        string output = JsonUtility.ToJson(instance, true);
        File.WriteAllText(Application.dataPath + "/Data/level_data.json", output);
    }


    private void Start()
    {
        write_Json_file();
        read_Json_file();
    }
}
