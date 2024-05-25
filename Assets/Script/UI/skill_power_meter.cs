using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class skill_power_meter : MonoBehaviour
{
    public TMP_Text tmp;
    public UI_hook_up_object hook;

    public void Setup(GameObject target_obj) // ó�� ������ ������
    {
        hook.target_object = target_obj;
        tmp.text = "";
    }

    public IEnumerator Show(string value) // ���� �Է��� ��� ������
    {
        tmp.text = value;
        yield return new WaitForSeconds(1f);

        tmp.text = "";
        yield break;
    }
}
