using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slider_bar : MonoBehaviour
{
    Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void set_sldier_value(float value)
    {
        slider.value = value;
    }
    public void set_sldier_max_value(float value)
    {
        slider.maxValue = value;
    }
}

