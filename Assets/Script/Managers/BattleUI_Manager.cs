using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class BattleUI_Manager : Singletone<BattleUI_Manager>
{
    public GameObject health_bar_prefab;
    public GameObject willpower_bar_prefab;

    public enum UI_bars 
    { 
        health_bar,
        willpower_bar
    }

    public enum UI_bars_properties 
    { 
        max_value,
        current_value
    }

    private List<GameObject> health_bars = new List<GameObject>();
    private List<GameObject> willpower_bars = new List<GameObject>(); 

    void clear_bar_list(UI_bars bar_type) 
    {
        switch (bar_type) 
        { 
            case UI_bars.health_bar:
                health_bars.Clear();
                break;
            case UI_bars.willpower_bar:
                willpower_bars.Clear();
                break;
        }
    }

    void clear_all_bar_list() 
    {
        health_bars.Clear();
        willpower_bars.Clear();
    }

    void summon_UI_bar(UI_bars bar_type, GameObject character_obj) //ĳ���� ü�¹� ��ȯ
    {
        GameObject bar;
        switch (bar_type) 
        {
            case UI_bars.health_bar:
                bar = Instantiate(health_bar_prefab, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
                bar.GetComponent<UI_hook_up_object>().target_object = character_obj; //ĳ���Ϳ� ü�¹� ���̱�
                health_bars.Add(bar);
                break;
            case UI_bars.willpower_bar:
                bar = Instantiate(willpower_bar_prefab, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
                bar.GetComponent<UI_hook_up_object>().target_object = character_obj; //ĳ���Ϳ� ���ŷ¹� ���̱�
                willpower_bars.Add(bar);
                break;

        }
 
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
                if (obj.transform.Find("value_marker") != null) // �� �����ִ� ���ںκ� ����
                {
                    obj.transform.Find("value_marker").gameObject.GetComponent<UI_bar_value_marker>().update_value_marker(value.ToString());
                }
                break;
        }
    }

    public void set_bar_property(UI_bars bar_type, int index, UI_bars_properties property, float value) //Ư�� UI ����� ���� ����
    { 
        switch (bar_type) 
        {
            case UI_bars.health_bar:
                set_UI_slider_property_of_object(health_bars[index], property, value);
                break;
            case UI_bars.willpower_bar:
                set_UI_slider_property_of_object(willpower_bars[index], property, value);
                break;

        }
    }


    public void Setup_health_and_willpower_bars() // ���� ���۽� �� ����
    {
        // �� ����Ʈ �ʱ�ȭ
        clear_all_bar_list();

        for (int i = 0; i < BattleManager.instance.playable_characters.Count; i++)
        {
            // ü�¹�, ���ŷ¹� ����
            summon_UI_bar(UI_bars.health_bar, BattleManager.instance.playable_characters[i]);
            summon_UI_bar(UI_bars.willpower_bar, BattleManager.instance.playable_characters[i]);

            // ü�¹�, ���ŷ¹� �ʱⰪ ����
            instance.set_bar_property(UI_bars.health_bar, i, UI_bars_properties.max_value, BattleManager.instance.playable_character_data[i].get_max_health_of_level(BattleManager.instance.playable_character_data[i].level));
            instance.set_bar_property(UI_bars.health_bar, i, UI_bars_properties.current_value, BattleManager.instance.playable_character_data[i].current_health);
            instance.set_bar_property(UI_bars.willpower_bar, i, UI_bars_properties.max_value, BattleManager.instance.playable_character_data[i].get_max_willpower_of_level(BattleManager.instance.playable_character_data[i].level));
            instance.set_bar_property(UI_bars.willpower_bar, i, UI_bars_properties.current_value, BattleManager.instance.playable_character_data[i].current_willpower);
        }
        for (int i = 0; i < BattleManager.instance.enemy_characters.Count; i++)
        {
            // ü�¹�, ���ŷ¹� ����
            instance.summon_UI_bar(UI_bars.health_bar, BattleManager.instance.enemy_characters[i]);
            instance.summon_UI_bar(UI_bars.willpower_bar, BattleManager.instance.enemy_characters[i]);

            // ü�¹�, ���ŷ¹� �ʱⰪ ����
            instance.set_bar_property(UI_bars.health_bar, BattleManager.instance.playable_characters.Count + i, UI_bars_properties.max_value, BattleManager.instance.enemy_character_data[i].get_max_health_of_level(BattleManager.instance.enemy_character_data[i].level));
            instance.set_bar_property(UI_bars.health_bar, BattleManager.instance.playable_characters.Count + i, UI_bars_properties.current_value, BattleManager.instance.enemy_character_data[i].current_health);
            instance.set_bar_property(UI_bars.willpower_bar, BattleManager.instance.playable_characters.Count + i, UI_bars_properties.max_value, BattleManager.instance.enemy_character_data[i].get_max_willpower_of_level(BattleManager.instance.enemy_character_data[i].level));
            instance.set_bar_property(UI_bars.willpower_bar, BattleManager.instance.playable_characters.Count + i, UI_bars_properties.current_value, BattleManager.instance.enemy_character_data[i].current_willpower);
        }
    }

}
