using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))] //버튼이 없다면 추가
public class Shop_Btn : MonoBehaviour
{
    //패널 가져오기
    [SerializeField] GameObject panel;
    Image btnImage;
    
    public GameObject GetPanel => panel; //가져온 패널 메서드 연결

    BtnManager parent; //Btnmanager코드를 부모로 하기
    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(SwithchTab);//리스너에 함수를 코드로 할당시키기
        parent = transform.parent.GetComponent<BtnManager>();//BtnManager 가진 부모 불러오기
        btnImage = btn.image; //버튼에 할당된 이미지 가져오기
    }

    void SwithchTab()//버튼 클릭시 부모에게 알림
    {
        parent.SwithchTab(this);
    }
    public void ChangeBtnImg(Sprite B_sprite)
    {
        if (btnImage == null) return; //이미지가 없을때 실행 망가짐 방지
        if (btnImage.sprite != B_sprite)//버튼에 할당된 이미지가 요청한 이미지와 다르다면
            btnImage.sprite = B_sprite; //이미지 바꾸기
        
    }
}
