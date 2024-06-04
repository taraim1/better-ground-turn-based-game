using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class battle_start_button : MonoBehaviour
{
    public GameObject empty_party_text;
    public TMP_Text holy_water_text;
    public string SceneName;

    void Check_empty_party() 
    {
        if (PartyManager.instance.get_party_member_count() == 0)
        {
            empty_party_text.SetActive(true);
        }
        else 
        {
            empty_party_text.SetActive(false);
        }
    }

    public void OnButtonClick()
    {
        if (PartyManager.instance.get_party_member_count() != 0) 
        {
            SceneManager.LoadScene(SceneName);
        }
    }

    private void Start()
    {
        Check_empty_party();
        BattleEventManager.party_member_changed += Check_empty_party;
        holy_water_text.text = string.Format("성수 () 소모\n{0} 보유", ResourceManager.instance.Water);
    }

    private void OnDestroy()
    {
        BattleEventManager.party_member_changed -= Check_empty_party;
    }
}
