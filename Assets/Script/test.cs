using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    private void Start()
    {
        ResourceManager.instance.get_resource(ResourceManager.resources.gold); //��� �� ��������
        ResourceManager.instance.add_resource(ResourceManager.resources.gold, 10); //��� ���ϱ�
        //ResourceManager�� �ִ� �ٸ� �޼ҵ���� �Ű� �� �ᵵ �ǰ� �������� �����͸� ���� ��
    }

}


