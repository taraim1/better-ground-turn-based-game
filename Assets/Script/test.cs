using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    private void Start()
    {
        print(ResourceManager.instance.Gold); //��� �� �����ͼ� �ַܼ� ���

        ResourceManager.instance.Gold = 100; //��� �����ϱ�
        print(ResourceManager.instance.Gold);

        ResourceManager.instance.Gold = -100; //�̰� ������ ���� �� ��

        //ResourceManager.instance.Gold�� ����ó�� ���� ��


        /* 
            ResourceManager���� �����ϴ� ��� ��� 
            
            1. ��� ���� json���Ϸ� ���� (�ڵ����� �ǵ��� ���������� �Ű�x �ص� ��)
            2. ��� ���� json���Ͽ��� �ҷ����� (�ڵ����� ��)
            3. ��� ���� ����ó�� ����ϱ�
            4. ��� ���� ������ �Ǵ� �� �ڵ����� ����
              
         */
    }

}


