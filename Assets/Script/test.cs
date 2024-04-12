using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    private void Start()
    {
        print(ResourceManager.instance.Gold); //골드 값 가져와서 콘솔로 출력

        ResourceManager.instance.Gold = 100; //골드 설정하기
        print(ResourceManager.instance.Gold);

        ResourceManager.instance.Gold = -100; //이건 음수라서 적용 안 됨

        //ResourceManager.instance.Gold를 변수처럼 쓰면 됨


        /* 
            ResourceManager에서 지원하는 기능 목록 
            
            1. 골드 값을 json파일로 저장 (자동으로 되도록 만들어놨으니 신경x 해도 됨)
            2. 골드 값을 json파일에서 불러오기 (자동으로 됨)
            3. 골드 값을 변수처럼 사용하기
            4. 골드 값이 음수가 되는 걸 자동으로 막기
              
         */
    }

}


