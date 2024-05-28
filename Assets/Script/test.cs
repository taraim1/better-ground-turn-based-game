using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    private void Start()
    {
        ResourceManager.instance.get_resource(ResourceManager.resources.gold); //골드 값 가져오기
        ResourceManager.instance.add_resource(ResourceManager.resources.gold, 10); //골드 더하기
        //ResourceManager에 있는 다른 메소드들은 신경 안 써도 되게 만들어놓음 위에것만 쓰면 됨
    }

}


