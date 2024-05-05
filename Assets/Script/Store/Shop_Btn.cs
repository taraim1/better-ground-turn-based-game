using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))] //��ư�� ���ٸ� �߰�
public class Shop_Btn : MonoBehaviour
{
    //�г� ��������
    [SerializeField] GameObject panel;
    Image btnImage;
    
    public GameObject GetPanel => panel; //������ �г� �޼��� ����

    BtnManager parent; //Btnmanager�ڵ带 �θ�� �ϱ�
    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(SwithchTab);//�����ʿ� �Լ��� �ڵ�� �Ҵ��Ű��
        parent = transform.parent.GetComponent<BtnManager>();//BtnManager ���� �θ� �ҷ�����
        btnImage = btn.image; //��ư�� �Ҵ�� �̹��� ��������
    }

    void SwithchTab()//��ư Ŭ���� �θ𿡰� �˸�
    {
        parent.SwithchTab(this);
    }
    public void ChangeBtnImg(Sprite B_sprite)
    {
        if (btnImage == null) return; //�̹����� ������ ���� ������ ����
        if (btnImage.sprite != B_sprite)//��ư�� �Ҵ�� �̹����� ��û�� �̹����� �ٸ��ٸ�
            btnImage.sprite = B_sprite; //�̹��� �ٲٱ�
        
    }
}
