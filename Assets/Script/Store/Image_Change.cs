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
        "ĳ���͸� �̴� â �Դϴ�.",
        "��ų�� �̴� â �Դϴ�."
    };

    // Start is called before the first frame update
    void Start()
    {
        // �̹����� �ؽ�Ʈ ������Ʈ�� �ùٸ��� �Ҵ� �޾Ҵ��� Ȯ��
        this_image = GetComponent<Image>();
        Text = GetComponent<TextMeshProUGUI>();

        // Text�� Image�� null�� �ƴ��� Ȯ��
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
        // Images �迭�� ������� ������ Ȯ��
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
        // Text�� null�� �ƴ��� Ȯ�� �� ������Ʈ
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
