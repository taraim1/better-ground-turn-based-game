using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class effect_description_handler : MonoBehaviour
{
    [SerializeField] private GameObject effect_description_prefab;
    [SerializeField] private Camera cam;
    public int description_count = 0; // 현재 켜져있는 설명창 개수

    // TMP 링크 클릭 받는 메소드
    private void get_click(string TMPlink) 
    {
        show_description(TMPlink);
    }

    // 설명창 띄우는 메소드
    private void show_description(string TMPlink) 
    {
        character_effect_code code = Util.StringToEnum<character_effect_code>(TMPlink);
        Vector2 mousepos;
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 보정
        if (mousepos.y < -2) { mousepos.y = -2f; }
        else if (mousepos.y > 2) { mousepos.y = 2f; }

        GameObject effect_description_obj = Instantiate(effect_description_prefab);

        effect_description_obj.transform.position = new Vector3(mousepos.x, mousepos.y, -2f);
        
        effect_description description = effect_description_obj.GetComponent<effect_description>();
        description.set_effect_text(code, cam);
        description.set_order(description_count*5);

        description_count += 1;


    }

    void OnEnable()
    {
        ActionManager.TMP_link_clicked += get_click;
    }
    private void OnDisable()
    {
        ActionManager.TMP_link_clicked -= get_click;
    }


}
