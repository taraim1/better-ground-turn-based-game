using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singletone<BattleManager> //�̱�����
{
    public int current_turn = 0; // ���� ��
    public enum current_phase // ���� �Ͽ����� ������
    { 
        turn_start_effect_phase,
        enemy_skill_setting_phase,
        player_skill_setting_phase,
        fight_phase,
        turn_end_effect_phase
    }
}
