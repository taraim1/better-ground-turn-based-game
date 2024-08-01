using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Char_UI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text character_name, description, level, health, willpower;
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
        
        character_name.text = character.Character_name; //�̸��� �־����
        level.text = "Lv." + character.Level.ToString();
        health.text = "ü�� : " + character.get_max_health().ToString();
        willpower.text = "���ŷ� : " + character.get_max_willpower().ToString();
        GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_Datapath);
        GameObject SPUM_unit_obj = Instantiate(spPrefab, Character_port.transform);
        SPUM_unit_obj.transform.localPosition = new Vector3(0f, 0f, 1);//�˸°� ������ �ּ���
        SPUM_unit_obj.transform.localScale = new Vector3(300f, 300f, 1);


    }
}
