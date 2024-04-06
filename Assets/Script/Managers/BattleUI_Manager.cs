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

    private List<GameObject> health_bars = new List<GameObject>(); //0번~3번 : 플레이어블 캐릭터 체력바

    public void clear_health_bar_list() 
    {
        health_bars.Clear();
    }
    public void summon_health_bar(GameObject character_obj) //캐릭터 체력바 소환
    {

        GameObject bar = Instantiate(health_bar_prefab, new Vector3(0,0,0), Quaternion.identity, GameObject.Find("Canvas").transform);
        bar.GetComponent<UI_hook_up_object>().target_object = character_obj; //캐릭터에 체력바 붙이기
        health_bars.Add(bar);

 
    }

    private void set_UI_slider_property_of_object(GameObject obj, UI_bars_properties property, float value) //바 오브젝트의 슬라이더값 변경
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

    public void set_UI_slider_property_of_UIelement(UI_bars bar_type, int index, UI_bars_properties property, float value) //특정 UI 요소의 값을 변경
    { 
        switch (bar_type) 
        {
            case UI_bars.health_bar:
                set_UI_slider_property_of_object(health_bars[index], property, value);
                break;
        
        }
    }

}
