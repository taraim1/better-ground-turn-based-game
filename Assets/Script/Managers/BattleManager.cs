using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singletone<BattleManager> //싱글톤임
{
    public int current_turn = 0; // 현재 턴
    public enum current_phase // 현재 턴에서의 페이즈
    { 
        turn_start_effect_phase,
        enemy_skill_setting_phase,
        player_skill_setting_phase,
        fight_phase,
        turn_end_effect_phase
    }
}
