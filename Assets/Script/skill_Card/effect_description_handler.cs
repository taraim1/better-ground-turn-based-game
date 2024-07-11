using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class effect_description_handler : MonoBehaviour
{
    [SerializeField] private GameObject effect_description_prefab;
    [SerializeField] private Camera cam;
    private int description_count = 0; // ���� �����ִ� ����â ����
    private int max_description_count = 0; // ���� �����ִ� ����â �� ���� ���߿� ���� �� �� ��°�� ���� ���� ����

    // TMP ��ũ Ŭ�� �޴� �޼ҵ�
    private void get_click(string TMPlink) 
    {
        show_description(TMPlink);
    }

    // ����â ���� �޼ҵ�
    private void show_description(string TMPlink) 
    {
        character_effect_code code = Util.StringToEnum<character_effect_code>(TMPlink);
        Vector2 mousepos;
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ����
        if (mousepos.y < -2) { mousepos.y = -2f; }
        else if (mousepos.y > 2) { mousepos.y = 2f; }

        GameObject effect_description_obj = Instantiate(effect_description_prefab);

        effect_description_obj.transform.position = new Vector3(mousepos.x, mousepos.y, -2f);
        
        effect_description description = effect_description_obj.GetComponent<effect_description>();
        description.set_effect_text(code, cam);
        description.set_order(max_description_count*5);

        description_count += 1;
        max_description_count += 1;

    }

    // ����â ���� ���ҽ�Ű�� �޼ҵ�
    public void reduce_count() 
    {
        description_count -= 1;

        if (description_count == 0) 
        {
            max_description_count = 0;
        }
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
