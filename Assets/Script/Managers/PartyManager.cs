using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Rendering.VirtualTexturing;

public class PartyManager : Singletone<PartyManager>, IJson
{
   


    public static int party_member_count = 4;
    private TextAsset party_data_txt;
    public void read_Json_file() //��Ƽ �����͸� Json���Ͽ��� �а� PartyDataContainer�� ���ε�
    {
        PartyDataContainer container = new PartyDataContainer();
        container = JsonUtility.FromJson<PartyDataContainer>(File.ReadAllText(Application.dataPath + "/Data/party_data.txt"));
        PartyDataContainer.names = container.names_temp;
        PartyDataContainer.max_health = container.max_health_temp;
        PartyDataContainer.max_willpower = container.max_willpower_temp;
        PartyDataContainer.number_of_skill_slots = container.number_of_skill_slots_temp;
    }
    public void write_Json_file() //��Ƽ �����͸� Json���Ϸ� ����
    {
        PartyDataContainer container = new PartyDataContainer();
        
        container.names_temp = PartyDataContainer.names;
        container.max_health_temp = PartyDataContainer.max_health;
        container.max_willpower_temp = PartyDataContainer.max_willpower;
        container.number_of_skill_slots_temp = PartyDataContainer.number_of_skill_slots;
        string output = JsonUtility.ToJson(container, true);
        File.WriteAllText(Application.dataPath + "/Data/party_data.txt", output);
    }

    //ĳ���͸� �޾Ƽ� �ε����� ���� ��Ƽ ������ �����̳ʿ� ����
    public void set_character_to_party(Playable_Character character, int index) 
    {
        PartyDataContainer.names[index] = character.get_character_name();
        PartyDataContainer.max_health[index] = character.get_character_int_property(CharacterManager.character_int_properties.max_health);
        PartyDataContainer.max_willpower[index] = character.get_character_int_property(CharacterManager.character_int_properties.max_willpower);
        PartyDataContainer.number_of_skill_slots[index] = character.get_character_int_property(CharacterManager.character_int_properties.number_of_skill_slots);
    }

    //��Ƽ�� �ε�����°�� �ڸ��� �ִ� ĳ������ �ν��Ͻ��� ���� �޼ҵ�
    public Playable_Character get_character_of_party(int index) 
    { 
        Playable_Character character = new Playable_Character();
        character.set_character_name(PartyDataContainer.names[index]);
        character.set_character_int_property(CharacterManager.character_int_properties.max_health, PartyDataContainer.max_health[index]);
        character.set_character_int_property(CharacterManager.character_int_properties.current_health, PartyDataContainer.max_health[index]);
        character.set_character_int_property(CharacterManager.character_int_properties.max_willpower, PartyDataContainer.max_willpower[index]);
        character.set_character_int_property(CharacterManager.character_int_properties.current_willpower, PartyDataContainer.max_willpower[index]);
        character.set_character_int_property(CharacterManager.character_int_properties.number_of_skill_slots, PartyDataContainer.number_of_skill_slots[index]);
        return character;
    }

    


}
