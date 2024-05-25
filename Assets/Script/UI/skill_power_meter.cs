using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class skill_power_meter : MonoBehaviour
{
    public TMP_Text tmp;
    public UI_hook_up_object hook;

    public void Setup(GameObject target_obj) // 처음 생성시 설정용
    {
        hook.target_object = target_obj;
        tmp.text = "";
    }

    public IEnumerator Show(string value) // 들어온 입력을 잠시 보여줌
    {
        tmp.text = value;
        yield return new WaitForSeconds(1f);

        tmp.text = "";
        yield break;
    }
}
