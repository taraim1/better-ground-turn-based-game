using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class battle_start_button : MonoBehaviour
{
    public GameObject empty_party_text;
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
    }

    private void OnDestroy()
    {
        BattleEventManager.party_member_changed -= Check_empty_party;
    }
}
