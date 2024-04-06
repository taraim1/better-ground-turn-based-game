using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI_Manager : Singletone<BattleUI_Manager>
{
    public GameObject health_bar_prefab;

    public enum UI_bars 
    { 
        health_bar
    }

    public enum UI_bars_properties 
    { 
        max_value,
        current_value
    }

    private List<GameObject> health_bars = new List<GameObject>(); //0��~3�� : �÷��̾�� ĳ���� ü�¹�

    public void clear_health_bar_list() 
    {
        health_bars.Clear();
    }
    public void summon_health_bar(GameObject character_obj) //ĳ���� ü�¹� ��ȯ
    {

        GameObject bar = Instantiate(health_bar_prefab, new Vector3(0,0,0), Quaternion.identity, GameObject.Find("Canvas").transform);
        bar.GetComponent<UI_hook_up_object>().target_object = character_obj; //ĳ���Ϳ� ü�¹� ���̱�
        health_bars.Add(bar);

 
    }

    private void set_UI_slider_property_of_object(GameObject obj, UI_bars_properties property, float value) //�� ������Ʈ�� �����̴��� ����
    {
        switch (property) 
        { 
            case UI_bars_properties.max_value:
                obj.GetComponent<Slider>().maxValue = value;
                break;

            case UI_bars_properties.current_value:
                obj.GetComponent<Slider>().value = value;
                break;
        }   
    }

    public void set_UI_slider_property_of_UIelement(UI_bars bar_type, int index, UI_bars_properties property, float value) //Ư�� UI ����� ���� ����
    { 
        switch (bar_type) 
        {
            case UI_bars.health_bar:
                set_UI_slider_property_of_object(health_bars[index], property, value);
                break;
        
        }
    }

}
