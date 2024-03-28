using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PartyDataContainer : Singletone<PartyDataContainer>
{


    public Playable_Character[] party_data = new Playable_Character[PartyManager.party_member_count]; //파티 데이터 담아두는 곳

    //Json에 들어가는 부분들
    [SerializeField]
    private string[] names = new string[PartyManager.party_member_count];
    [SerializeField]
    private int[] max_health = new int[PartyManager.party_member_count];
    [SerializeField]
    private int[] max_willpower = new int[PartyManager.party_member_count];
    [SerializeField]
    private int[] number_of_skill_slots = new int[PartyManager.party_member_count];


    public void update_party_data() //파티 데이터를 Json에 들어가는 부분들로 옮김
    {
        for (int i = 0; i < PartyManager.party_member_count; i++)
        {
            if (party_data[i] != null) 
            {
                names[i] = party_data[i].get_character_name();
                max_health[i] = party_data[i].get_character_int_property(CharacterManager.character_int_properties.max_health);
                max_willpower[i] = party_data[i].get_character_int_property(CharacterManager.character_int_properties.max_willpower);
                number_of_skill_slots[i] = party_data[i].get_character_int_property(CharacterManager.character_int_properties.number_of_skill_slots);

            }     
        }

    }




}
