using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectsLayoutGroup : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private int width;
    [SerializeField] private int icon_height;
    [SerializeField] private int gap_height;
    [SerializeField] private int icon_per_row;
    [SerializeField] private Character _character;
    public void set_size(int effect_count) 
    {
        Vector2 size = new Vector2(width, 0);

        if (effect_count == 0) { rectTransform.sizeDelta = size; return; }

        if (effect_count % icon_per_row == 0)
        {
            size.y = icon_height * (effect_count / icon_per_row) + gap_height * (effect_count / icon_per_row);
        }
        else 
        {
            size.y = icon_height * (effect_count / icon_per_row + 1) + gap_height * (effect_count / icon_per_row + 1);
        }
        
        rectTransform.sizeDelta = size;
    }

    private void Awake()
    {
        _character.Data.Effects_layoutGroup_obj = gameObject;
    }
}
