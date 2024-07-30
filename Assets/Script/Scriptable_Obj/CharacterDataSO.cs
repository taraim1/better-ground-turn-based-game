using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicCharacterData
{

    public string character_name;
    public string description;
    public int level;
    public List<int> max_healthes_of_level;
    public List<int> max_willpowers_of_level;
    public List<skillcard_code> deck;
    public string SPUM_datapath;
    public List<coordinate> move_range;
}

public class PlayableCharacterData 
{
    public bool is_character_unlocked;
}

[System.Serializable]
public class BasicCharacterData_dictionary : SerializableDictionary<character_code, BasicCharacterData> { }

[System.Serializable]
public class PlayableCharacterData_dictionary : SerializableDictionary<character_code, PlayableCharacterData> { }


[CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Scriptable_Objects_CharacterData")]
public class CharacterDataSO : ScriptableObject
{
    public BasicCharacterData_dictionary BasicData;
    public PlayableCharacterData_dictionary PlayerData;
}
