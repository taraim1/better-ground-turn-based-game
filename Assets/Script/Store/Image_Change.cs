using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Image_Change : MonoBehaviour
{
    [SerializeField]
    private Sprite[] Images;
    [SerializeField]
    private Image this_image;
    [SerializeField]
    private TextMeshProUGUI Text;

    private string[] Texts_ =
    {
        "캐릭터를 뽑는 창 입니다.",
        "스킬을 뽑는 창 입니다."
    };

    // Start is called before the first frame update
    void Start()
    {
        // 이미지와 텍스트 컴포넌트를 올바르게 할당 받았는지 확인
        this_image = GetComponent<Image>();
        Text = GetComponent<TextMeshProUGUI>();

        // Text와 Image가 null이 아닌지 확인
        if (this_image == null)
        {
            Debug.LogError("Image component is not assigned.");
        }

        /*if (Text == null)
        {
            Debug.LogError("TextMeshProUGUI component is not assigned.");
        }*/

        Panel_Change();
    }

    public void Panel_Change()
    {
        // Images 배열이 비어있지 않은지 확인
        if (Images != null && Images.Length > 0)
        {
            this_image.sprite = Images[BtnManager.Current_Scene];
        }
        else
        {
            Debug.LogError("Images array is not assigned or empty.");
        }
    }

    void Update()
    {
        /*
        // Text가 null이 아닌지 확인 후 업데이트
        if (Text != null)
        {
            Text.text = Texts_[BtnManager.Current_Scene];
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component is not assigned.");
        }8*/    
    }
}
