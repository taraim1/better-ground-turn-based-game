using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;


public class PartyManager : Singletone<PartyManager>
{

    // ���� ��Ƽ �����Ͱ� ���� ��
    private PartyDataContainer PartyData = new PartyDataContainer();



    [System.Serializable]
    private class PartyDataContainer 
    {
        public int party_member_count;
        private int party_member_max = 4;
        public int party_member_Max { get { return party_member_max; } set { party_member_max = value; }}
        public List<CharacterManager.character_code> party_codes = new List<CharacterManager.character_code>();
    }

    // ���� ��Ƽ �����͸� json ���Ϸ� ����
    private void save_party_to_json() 
    {
        string output = JsonUtility.ToJson(PartyData, true);
        File.WriteAllText(Application.dataPath + "/Data/PartyData.json", output);
    }

    // ���� ��Ƽ �����͸� json ���Ͽ��� �ҷ��� �����
    private void load_party_from_json()
    {
        string output = File.ReadAllText(Application.dataPath + "/Data/PartyData.json");
        PartyData = JsonUtility.FromJson<PartyDataContainer>(output);
    }

    // ĳ���� �ڵ带 ��Ƽ�� �߰�
    public void add_character_to_party(CharacterManager.character_code code) 
    {
        if (PartyData.party_member_count >= PartyData.party_member_Max)
        {
            Debug.Log("����: ��Ƽ �ִ� �ο� ���� �����Ͽ� �� �̻� �߰��� �Ұ����մϴ�.");
            return;
        }
 
        PartyData.party_codes.Add(code);
        PartyData.party_member_count++;
        save_party_to_json();

    }

    // ĳ���� �ڵ带 ��Ƽ���� ����
    public void remove_character_from_party(CharacterManager.character_code code)
    {
        if (PartyData.party_member_count == 0)
        {
            Debug.Log("����: ��Ƽ�� ��� �־� ������ �Ұ����մϴ�.");
            return;
        }

        if (!PartyData.party_codes.Remove(code)) 
        {
            Debug.Log("����: ��Ƽ�� �����Ϸ��� ĳ���Ͱ� �����ϴ�.");
            return;
        }

        PartyData.party_member_count--;
        save_party_to_json();
    }


    private void Start()
    {
        load_party_from_json();
    }
}
