using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Battle_result_canvas : MonoBehaviour
{
    // 이 캔버스 활성화 / 비활성화 하는 건 BattleManager에 있음

    [SerializeField] private TMP_Text result_text;

    // 전투 결과값 받아서 띄움
    public void Set_result(bool victory) 
    {
        if (victory)
        {
            result_text.text = "전투 결과\n\n<size=200%>승리</size>";
        }

        else 
        {
            result_text.text = "전투 결과\n\n<size=200%>패배</size>";
        }
    }

}
