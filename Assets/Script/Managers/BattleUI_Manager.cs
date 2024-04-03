using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI_Manager : Singletone<BattleUI_Manager>
{
    public GameObject health_bar_prefab;

    private List<GameObject> health_bars = new List<GameObject>(); //0��~3�� : �÷��̾�� ĳ���� ü�¹�

    public void clear_health_bar_list() 
    {
        health_bars.Clear();
    }
    public void summon_health_bar(GameObject character_obj) //ĳ���� ü�¹� ��ȯ
    {
        for (int i = 0; i < PartyManager.party_member_count; i++) 
        {
            GameObject bar = Instantiate(health_bar_prefab, new Vector3(0,0,0), Quaternion.identity, GameObject.Find("Canvas").transform);
            bar.GetComponent<UI_hook_up_object>().target_object = character_obj; //ĳ���Ϳ� ü�¹� ���̱�
            health_bars.Add(bar);
        }
 
    }

}
