using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_bar_slider : MonoBehaviour
{
    public TMP_Text value_tmp;
    public Slider slider;

    private enum bar_type 
    {
        health_bar,
        willpower_bar
    }
    [SerializeField] private Character _character;
    [SerializeField] private bar_type _type;

    private void Awake()
    {
        switch (_type)
        { 
            case bar_type.health_bar:
                _character.data.health_bar = gameObject;
                break;
            case bar_type.willpower_bar:
                _character.data.willpower_bar = gameObject;
                break;
        }
    }
}
