using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BattleManager : Singletone<BattleManager> //�̱�����
{
    [SerializeField]
    private int current_turn = 0; // ���� ��
    [SerializeField]
    private int current_phase = 0; // ���� ������
    [SerializeField]
    private bool is_Charaters_spawned = false; // ���� ���۽� �÷����̾�� �� ĳ���Ͱ� �����Ǿ��°�?
    public enum phases // �� ���� ������ ����
    { 
        turn_start_effect_phase,
        enemy_skill_setting_phase,
        player_skill_setting_phase,
        fight_phase,
        turn_end_effect_phase
    }
    
}
