using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CharacterManager;

public class Char_UI : MonoBehaviour
{
    [SerializeField]
    public Text name, description, level, health, willpower;
    [SerializeField]
    public Transform Card_Char;

    [System.Serializable]
    public class Character_UI_Set
    {
        public string name, description, level, health, willpower;
    }
    public void Char_UI_Set(Character Char)
    {
        
        this.name.text = Char.character_name; //�̸��� �־����
        GameObject spPrefab = Resources.Load<GameObject>(Char.SPUM_datapath);
        Char.SPUM_unit_obj = Instantiate(spPrefab, Card_Char);
        Char.SPUM_unit_obj.transform.localPosition = new Vector3(0f, 0f, 1);//�˸°� ������ �ּ���
        Char.SPUM_unit_obj.transform.localScale = new Vector3(300f, 300f, 1);


    }
}
