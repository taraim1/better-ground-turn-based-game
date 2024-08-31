using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] private resource_code resource_code;
    [SerializeField] private TMP_Text TMP_Text;

    private void Awake()
    {
        TMP_Text.text = ResourceManager.instance.GetResourceValue(resource_code).ToString();
        ActionManager.resource_changed += OnResourceChanged;
    }

    private void OnDestroy()
    {
        ActionManager.resource_changed -= OnResourceChanged;
    }

    private void OnResourceChanged(resource_code code, int value) 
    {
        if (code != resource_code) return;
        if (TMP_Text == null) return;

        // °»½Å
        TMP_Text.text = value.ToString();
    }
}
