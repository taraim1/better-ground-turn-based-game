using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCalcManager : Singletone<BattleManager>
{
    // ī�� ���� ������ ���
    static card card1;
    static card card2;

    // ���� ī�� ���� �޼ҵ�
    public static void set_card(card card, int index) 
    { 
        switch (index) 
        {
            case 1: card1 = card; break;
            case 2: card2 = card; break;
        }
    }
}
