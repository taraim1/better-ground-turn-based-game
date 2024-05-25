using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image_Change : MonoBehaviour
{
    [SerializeField]
    private Sprite[] Images;
    [SerializeField]
    Image this_image;
    // Start is called before the first frame update
    void Start()
    {
        this_image = GetComponent<Image>();
        Panel_Change();
    }

    public void Panel_Change()
    {
        this_image.sprite = Images[BtnManager.Current_Scene];
        
    }
}
