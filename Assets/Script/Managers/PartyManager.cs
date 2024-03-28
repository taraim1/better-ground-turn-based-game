using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PartyManager : Singletone<PartyManager>, IJson
{

    public static int party_member_count = 4;
    public void read_Json_file() //��Ƽ �����͸� Json���Ͽ��� �б�
    {
        
    }
    public void write_Json_file() //��Ƽ �����͸� Json���Ϸ� ����
    {
        string output = JsonUtility.ToJson(PartyDataContainer.instance, true);
        File.WriteAllText(Application.dataPath + "/Data/party_data.txt", output);
    }

    //��Ƽ�� �ε��� (0~)��° �ڸ��� ĳ���� �ν��Ͻ��� �ְ� ��Ƽ�� ������Ʈ�ϴ� �޼ҵ�
    public void set_character_to_party(Playable_Character character, int index) 
    {
        PartyDataContainer.instance.party_data[index] = character;
        PartyDataContainer.instance.update_party_data();
    }

    //��Ƽ�� �ε�����°�� �ڸ��� �ִ� ĳ���� �ν��Ͻ��� ���� �޼ҵ�
    public Playable_Character get_character_of_party(int index) 
    { 
        Playable_Character character = PartyDataContainer.instance.party_data[index];
        return character;
    }
     
        
    
}
