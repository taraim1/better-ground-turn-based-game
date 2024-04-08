using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Rendering.VirtualTexturing;

public class PartyManager : Singletone<PartyManager>, IJson
{
   


    public static int party_member_count = 4;
    private TextAsset party_data_txt;
    public void read_Json_file() //파티 데이터를 Json파일에서 읽고 PartyDataContainer에 업로드
    {
        PartyDataContainer container = new PartyDataContainer();
        container = JsonUtility.FromJson<PartyDataContainer>(File.ReadAllText(Application.dataPath + "/Data/party_data.json"));
        PartyDataContainer.names = container.names_temp;
        PartyDataContainer.max_health = container.max_health_temp;
        PartyDataContainer.max_willpower = container.max_willpower_temp;
        PartyDataContainer.number_of_skill_slots = container.number_of_skill_slots_temp;
    }
    public void write_Json_file() //파티 데이터를 Json파일로 저장
    {
        PartyDataContainer container = new PartyDataContainer();
        
        container.names_temp = PartyDataContainer.names;
        container.max_health_temp = PartyDataContainer.max_health;
        container.max_willpower_temp = PartyDataContainer.max_willpower;
        container.number_of_skill_slots_temp = PartyDataContainer.number_of_skill_slots;
        string output = JsonUtility.ToJson(container, true);
        File.WriteAllText(Application.dataPath + "/Data/party_data.json", output);
    }

    //캐릭터를 받아서 인덱스를 따라 파티 데이터 콘테이너에 저장
    public void set_character_to_party(Playable_Character character, int index) 
    {
        PartyDataContainer.names[index] = character.Character_name;
        PartyDataContainer.max_health[index] = character.Max_health;
        PartyDataContainer.max_willpower[index] = character.Max_willpower;
        PartyDataContainer.number_of_skill_slots[index] = character.Number_of_skill_slots;
    }

    //파티의 인덱스번째의 자리에 있는 캐릭터의 인스턴스를 얻어내는 메소드
    public Playable_Character get_character_of_party(int index) 
    { 
        Playable_Character character = new Playable_Character();
        character.Character_name = PartyDataContainer.names[index];
        character.Max_health = PartyDataContainer.max_health[index];
        character.Current_health = PartyDataContainer.max_health[index];
        character.Max_willpower = PartyDataContainer.max_willpower[index];
        character.Current_willpower =  PartyDataContainer.max_willpower[index];
        character.Number_of_skill_slots =  PartyDataContainer.number_of_skill_slots[index];
        return character;
    }

    


}
