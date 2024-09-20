using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Char_UI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text character_name;
    [SerializeField]
    private Transform Card_Char;
    [SerializeField]
    private GameObject Character_port;

    public Character character;

    public class Character_UI_Set
    {
        public string name, description, level, health, willpower;
    }
    public void Char_UI_Set()
    {
        
        character_name.text = character.Character_name; //이름을 넣어줘요
        GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_Datapath);
        GameObject SPUM_unit_obj = Instantiate(spPrefab, Character_port.transform);
        SPUM_unit_obj.transform.localPosition = new Vector3(0f, 0f, 1);//알맞게 조정해 주세요
        SPUM_unit_obj.transform.localScale = new Vector3(100f, 100f, 1);


    }
}
