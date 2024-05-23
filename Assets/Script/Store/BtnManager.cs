using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BtnManager : MonoBehaviour
{
    [SerializeField] Sprite btnNormal; //��ư ���ý� �̹���
    [SerializeField] Sprite btnSelected; //��ư ���ý� �̹���
    [SerializeField] Text ExText; //����â �ؽ�Ʈ
    [SerializeField] GameObject Img_Panel;// ��Ʈ ǥ�� â
    

    Shop_Btn[] tabs; //��ư���� ������ �� �迭��
    public static int Current_Scene;
    public UnityEvent Panel_Change;
    // Start is called before the first frame update
    void Start()
    {
        tabs = GetComponentsInChildren<Shop_Btn>();
        Img_Panel = GetComponent<GameObject>();
        SwithchTab(tabs[0]); //�� ù��° ���� ���� �Ѵ�
    }

    public void SwithchTab(Shop_Btn S_Button) //���� Ȱ��ȭ/��Ȱ��ȭ �Ѵ�. ������ ����鼭.
    {
        for (int i = 0;  i < tabs.Length; i++)
        {
            bool isActiveTab = S_Button == tabs[i]; //i��° ���� ���� ������ �ƴ��� �Ǵ�.
            //tabs[i].GetPanel.SetActive(isActiveTab);//��� ���� �� ���ش�.
            if (isActiveTab) 
            { 
                Current_Scene = i;
                Panel_Change.Invoke();
            }
            tabs[i].ChangeBtnImg(isActiveTab? btnSelected : btnNormal); //���׿����ڷ� �̹��� �ٲٱ�
        }
    }

     
}
