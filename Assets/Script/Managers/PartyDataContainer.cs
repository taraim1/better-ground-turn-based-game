using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PartyDataContainer
{

    public static string[] names = new string[PartyManager.party_member_count];
    public static int[] max_health = new int[PartyManager.party_member_count];
    public static int[] max_willpower = new int[PartyManager.party_member_count];
    public static int[] number_of_skill_slots = new int[PartyManager.party_member_count];

    //json ���� �б�/����� ����Ʈ, static�� ����ȭ�� �� �Ǵ� ��� �����ؼ� ���� ��
    public string[] names_temp = new string[PartyManager.party_member_count];
    public int[] max_health_temp = new int[PartyManager.party_member_count];
    public int[] max_willpower_temp = new int[PartyManager.party_member_count];
    public int[] number_of_skill_slots_temp = new int[PartyManager.party_member_count];
}
