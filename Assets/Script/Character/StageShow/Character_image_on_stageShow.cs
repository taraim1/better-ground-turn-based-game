using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_image_on_stageShow : MonoBehaviour
{
    public CharacterManager.character_code code;
    public GameObject drag_obj;

    private Sprite sprite;

    private void Start()
    {
        sprite = GetComponent<Image>().sprite;
    }

    private void OnMouseDown()
    {

    }
}
