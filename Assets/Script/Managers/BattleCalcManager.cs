using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCalcManager : Singletone<BattleManager>
{
    // 카드 위력 판정시 사용
    static card card1;
    static card card2;

    // 판정 카드 설정 메소드
    public static void set_card(card card, int index) 
    { 
        switch (index) 
        {
            case 1: card1 = card; break;
            case 2: card2 = card; break;
        }
    }
}
