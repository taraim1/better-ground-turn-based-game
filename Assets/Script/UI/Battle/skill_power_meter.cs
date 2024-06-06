using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Search;
using Unity.Mathematics;
using Unity.VisualScripting;

public class skill_power_meter : MonoBehaviour
{
    public TMP_Text tmp;
    public UI_hook_up_object hook;

    [DoNotSerialize]
    public Coroutine running_show = null;

    public void Setup(GameObject target_obj) // ó�� ������ ������
    {
        hook.target_object = target_obj;
        tmp.text = "";
    }

    public IEnumerator Show(string value) // ���� �Է��� ��� ������
    {
        tmp.text = value;
        yield return new WaitForSeconds(1f);

        running_show = null;
        hide();
        yield break;
    }

    private void hide()
    {
        tmp.text = "";
    }

    private void Awake()
    {
        BattleEventManager.turn_start_phase += hide;
    }
    private void OnDisable()
    {
        BattleEventManager.turn_start_phase -= hide;
    }
}
