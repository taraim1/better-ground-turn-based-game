using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;


public class PartyManager : Singletone<PartyManager>
{

    // 현재 파티 데이터가 담기는 곳
    private PartyDataContainer PartyData = new PartyDataContainer();



    [System.Serializable]
    private class PartyDataContainer 
    {
        public int party_member_count;
        private int party_member_max = 4;
        public int party_member_Max { get { return party_member_max; } set { party_member_max = value; }}
        public List<CharacterManager.character_code> party_codes = new List<CharacterManager.character_code>();
    }

    // 현재 파티 데이터를 json 파일로 저장
    private void save_party_to_json() 
    {
        string output = JsonUtility.ToJson(PartyData, true);
        File.WriteAllText(Application.dataPath + "/Data/PartyData.json", output);
    }

    // 현재 파티 데이터를 json 파일에서 불러와 덮어씌움
    private void load_party_from_json()
    {
        string output = File.ReadAllText(Application.dataPath + "/Data/PartyData.json");
        PartyData = JsonUtility.FromJson<PartyDataContainer>(output);
    }

    // 캐릭터 코드를 파티에 추가
    public void add_character_to_party(CharacterManager.character_code code) 
    {
        if (PartyData.party_member_count >= PartyData.party_member_Max)
        {
            Debug.Log("오류: 파티 최대 인원 수에 도달하여 더 이상 추가가 불가능합니다.");
            return;
        }
 
        PartyData.party_codes.Add(code);
        PartyData.party_member_count++;
        save_party_to_json();

    }

    // 캐릭터 코드를 파티에서 제거
    public void remove_character_from_party(CharacterManager.character_code code)
    {
        if (PartyData.party_member_count == 0)
        {
            Debug.Log("오류: 파티가 비어 있어 삭제가 불가능합니다.");
            return;
        }

        if (!PartyData.party_codes.Remove(code)) 
        {
            Debug.Log("오류: 파티에 삭제하려는 캐릭터가 없습니다.");
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
