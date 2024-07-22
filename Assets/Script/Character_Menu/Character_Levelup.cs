using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Character_Levelup : MonoBehaviour
{
    [SerializeField] private Character _character;
    public GameObject ErrorMasage;
    public TMP_Text NeedMoney;
    [SerializeField] TMP_Text _level_display_tmp;

    public void setup(Character character)
    {
        _character = character;
        NeedMoney.text = (_character.level * 100).ToString();
    }
    public void OnClick()
    {
       if(ResourceManager.instance.Gold >= _character.level*100)
        {
            ResourceManager.instance.Gold -= _character.level * 100;
            _character.level += 1;
            NeedMoney.text = (_character.level * 100).ToString();
            _level_display_tmp.text = _character.level.ToString();
            CharacterManager.instance.save_character_to_json(_character);
        }
       else
        {
            ErrorMasage.SetActive(true);
        }
        
    }
}
