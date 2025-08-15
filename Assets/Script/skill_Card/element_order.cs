using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class element_order : MonoBehaviour
{
    [SerializeField] Renderer[] back_renderers;
    [SerializeField] Renderer[] middle_renderers;
    [SerializeField] Renderer[] upper_renderers;
    int origin_order;
    int current_order;
    public void Set_origin_order(int origin_order)
    {
        this.origin_order = origin_order * 5;
        Set_order(this.origin_order);
    }

    public int Get_order()
    {
        return current_order;
    }

    public void Set_highlighted_order()
    {
        Set_order(100);
    }



    public void Set_order(int order) 
    {

        foreach (var renderer in back_renderers) 
        { 
            renderer.sortingOrder = order;
        }

        foreach (var renderer in middle_renderers)
        {
            renderer.sortingOrder = order + 1;
        }

        foreach (var renderer in upper_renderers)
        {
            renderer.sortingOrder = order + 2;
        }

        current_order = order;
    }

    public void Return_to_origin_order() 
    {
        Set_order(origin_order);
    }

}
