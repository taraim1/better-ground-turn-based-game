using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_bar_value_marker : MonoBehaviour
{
    public TMP_Text tmp;

    public void update_value_marker(string value)
    {
        tmp.text = value;
    }
}
