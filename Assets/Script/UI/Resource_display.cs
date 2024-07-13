using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



[RequireComponent(typeof(TMP_Text))]

public class Resource_display : MonoBehaviour
{
    // �ڿ� �ڵ带 ���س����� �ڿ��� �ҷ��ͼ� ǥ������
    [SerializeField] private ResourceManager.resource_code _code;
    private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        update_text();
    }

    private void update_text()
    {
        _text.text = ResourceManager.instance.GetResourceName(_code).ToString() + " : " + ResourceManager.instance.GetResourceValue(_code).ToString();
    }

    private void OnResourceChange(ResourceManager.resource_code code) 
    {
        if (code == _code) 
        { 
            update_text();
        }
    }

    private void OnEnable()
    {
        ActionManager.resource_changed += OnResourceChange;
    }

    private void OnDisable()
    {
        ActionManager.resource_changed -= OnResourceChange;
    }
}
