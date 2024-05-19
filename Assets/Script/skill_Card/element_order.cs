using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class element_order : MonoBehaviour
{
    [SerializeField] Renderer[] back_renderers;
    [SerializeField] Renderer[] middle_renderers;
    [SerializeField] string sorting_layer_name;
    int origin_order;

    public void Set_origin_order(int origin_order)
    {
        this.origin_order = origin_order * 5;
        Set_order(this.origin_order);
    }

    public void Set_Most_front_order() 
    {
        Set_order(100);
    }

    public void Set_order(int order) 
    {

        foreach (var renderer in back_renderers) 
        { 
            renderer.sortingLayerName = sorting_layer_name;
            renderer.sortingOrder = order;
        }

        foreach (var renderer in middle_renderers)
        {
            renderer.sortingLayerName = sorting_layer_name;
            renderer.sortingOrder = order + 1;
        }
    }

    public void Return_to_origin_order() 
    {
        Set_order(origin_order);
    }

}
