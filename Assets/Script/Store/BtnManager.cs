using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnManager : MonoBehaviour
{
    [SerializeField] Sprite btnNormal; //��ư ���ý� �̹���
    [SerializeField] Sprite btnSelected; //��ư ���ý� �̹���

    Shop_Btn[] tabs; //��ư���� ������ �� �迭��

    // Start is called before the first frame update
    void Start()
    {
        tabs = GetComponentsInChildren<Shop_Btn>();
        SwithchTab(tabs[0]); //�� ù��° ���� ���� �Ѵ�
    }

    public void SwithchTab(Shop_Btn S_Button) //���� Ȱ��ȭ/��Ȱ��ȭ �Ѵ�. ������ ����鼭.
    {
        for (int i = 0;  i < tabs.Length; i++)
        {
            bool isActiveTab = S_Button == tabs[i]; //i ��° ���� ������ ������ �Ǵ�.
            tabs[i].GetPanel.SetActive(isActiveTab);//��� ���� �� ���ش�.
            tabs[i].ChangeBtnImg(isActiveTab? btnSelected : btnNormal); //���׿����ڷ� �̹��� �ٲٱ�
        }
    }

     
}
