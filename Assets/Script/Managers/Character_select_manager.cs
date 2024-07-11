using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_select_manager : MonoBehaviour
{
    public List<GameObject> character_select_illusts;

    // 파티에 있는 캐릭터는 캐릭터 선택에서 빠지고 없는 캐릭터는 나오게 함
    private void Update_character_select_image() 
    {
        foreach (GameObject obj in character_select_illusts) 
        {
            CharacterManager.character_code code = obj.GetComponent<Character_image_on_stageShow>().code;
            if (PartyManager.instance.check_character_in_party(code)) { obj.SetActive(false); }
            else { obj.SetActive(true); }
        }
    }

    private void Start()
    {
        Update_character_select_image();
        ActionManager.party_member_changed += Update_character_select_image;
    }

    private void OnDestroy()
    {
        ActionManager.party_member_changed -= Update_character_select_image;
    }
}
