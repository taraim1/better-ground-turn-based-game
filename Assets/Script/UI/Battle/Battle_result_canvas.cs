using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Battle_result_canvas : MonoBehaviour
{
    // �� ĵ���� Ȱ��ȭ / ��Ȱ��ȭ �ϴ� �� BattleManager�� ����

    [SerializeField] private TMP_Text result_text;

    // ���� ����� �޾Ƽ� ���
    public void Set_result(bool victory) 
    {
        if (victory)
        {
            result_text.text = "���� ���\n\n<size=200%>�¸�</size>";
        }

        else 
        {
            result_text.text = "���� ���\n\n<size=200%>�й�</size>";
        }
    }

}
