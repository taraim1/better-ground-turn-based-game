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

    [DoNotSerialize]
    public Coroutine running_show = null;

    public void Setup(GameObject target_obj) // 처음 생성시 설정용
    {
        tmp.text = "";
    }

    public IEnumerator Show(string value) // 들어온 입력을 잠시 보여줌
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
