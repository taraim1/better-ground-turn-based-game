using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class test : MonoBehaviour
{
    private void Start()
    {
        // ��ų ��� Ȯ��
        print(CardManager.instance.check_unlocked(CardManager.skillcard_code.simple_attack));

        // ��ų ��� ���� (json �ڵ������, true�� ����Ȱ� false�� ��� �ȵȰ�)
        CardManager.instance.set_unlocked(CardManager.skillcard_code.simple_defend, false);
    }

}


