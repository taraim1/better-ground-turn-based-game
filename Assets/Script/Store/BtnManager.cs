using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnManager : MonoBehaviour
{
    [SerializeField] Sprite btnNormal; //버튼 비선택시 이미지
    [SerializeField] Sprite btnSelected; //버튼 선택시 이미지

    Shop_Btn[] tabs; //버튼들을 보관해 둘 배열들

    // Start is called before the first frame update
    void Start()
    {
        tabs = GetComponentsInChildren<Shop_Btn>();
        SwithchTab(tabs[0]); //맨 첫번째 탭을 열게 한다
    }

    public void SwithchTab(Shop_Btn S_Button) //탭을 활성화/비활성화 한다. 일일이 세어가면서.
    {
        for (int i = 0;  i < tabs.Length; i++)
        {
            bool isActiveTab = S_Button == tabs[i]; //i 번째 탭이 눌려진 탭인지 판단.
            tabs[i].GetPanel.SetActive(isActiveTab);//모든 탭을 다 꺼준다.
            tabs[i].ChangeBtnImg(isActiveTab? btnSelected : btnNormal); //삼항연산자로 이미지 바꾸기
        }
    }

     
}
