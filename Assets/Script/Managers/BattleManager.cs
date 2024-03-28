using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BattleManager : Singletone<BattleManager> //싱글톤임
{
    [SerializeField]
    private int current_turn = 0; // 현재 턴
    [SerializeField]
    private int current_phase = 0; // 현재 페이즈
    [SerializeField]
    private bool is_Charaters_spawned = false; // 전투 시작시 플레이이어와 적 캐릭터가 스폰되었는가?
    public enum phases // 한 턴의 페이즈 모음
    { 
        turn_start_effect_phase,
        enemy_skill_setting_phase,
        player_skill_setting_phase,
        fight_phase,
        turn_end_effect_phase
    }
    
}
