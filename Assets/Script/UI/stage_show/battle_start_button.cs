using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class battle_start_button : MonoBehaviour
{
    public GameObject empty_party_text;
    public TMP_Text holy_water_text;
    public TMP_Text holy_water_text2;
    public string SceneName;
    private int holy_water_requirement;

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
        if (PartyManager.instance.get_party_member_count() != 0 && ResourceManager.instance.Water >= holy_water_requirement) 
        {
            ResourceManager.instance.Water -= holy_water_requirement;
            SceneManager.LoadScene(SceneName);
        }
    }

    private void Start()
    {
        holy_water_requirement = StageManager.instance.get_holy_water_requirement();
        Check_empty_party();
        ActionManager.party_member_changed += Check_empty_party;
        holy_water_text.text = string.Format("성수 {0} 소모", holy_water_requirement);
        holy_water_text2.text = string.Format("({0} 보유)", ResourceManager.instance.Water);
        if (ResourceManager.instance.Water < holy_water_requirement) 
        {
            holy_water_text2.text = "<color=#FF1212>" + holy_water_text2.text + "</color>";
        }
    }

    private void OnDestroy()
    {
        ActionManager.party_member_changed -= Check_empty_party;
    }
}
