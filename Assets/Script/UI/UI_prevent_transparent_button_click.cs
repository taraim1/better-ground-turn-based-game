using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_prevent_transparent_button_click : MonoBehaviour
{
    [SerializeField] private float minimum_alpha_hit_threshold = 0.1f;

    private void Awake()
    {
        var img = GetComponent<Image>();
        img.alphaHitTestMinimumThreshold = minimum_alpha_hit_threshold;
    }
}
