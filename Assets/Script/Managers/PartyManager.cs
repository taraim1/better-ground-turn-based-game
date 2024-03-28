using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PartyManager : Singletone<PartyManager>, IJson
{

    public static int party_member_count = 4;
    public void read_Json_file() //파티 데이터를 Json파일에서 읽기
    {
        
    }
    public void write_Json_file() //파티 데이터를 Json파일로 저장
    {
        string output = JsonUtility.ToJson(PartyDataContainer.instance, true);
        File.WriteAllText(Application.dataPath + "/Data/party_data.txt", output);
    }

    //파티의 인덱스 (0~)번째 자리에 캐릭터 인스턴스를 넣고 파티를 업데이트하는 메소드
    public void set_character_to_party(Playable_Character character, int index) 
    {
        PartyDataContainer.instance.party_data[index] = character;
        PartyDataContainer.instance.update_party_data();
    }

    //파티의 인덱스번째의 자리에 있는 캐릭터 인스턴스를 얻어내는 메소드
    public Playable_Character get_character_of_party(int index) 
    { 
        Playable_Character character = PartyDataContainer.instance.party_data[index];
        return character;
    }
     
        
    
}
