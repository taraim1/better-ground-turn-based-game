using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionManager : MonoBehaviour
{


    // 배틀에서 씀
    public static Action enemy_skill_setting_phase;
    public static Action enemy_skillData_deactivate;
    public static Action<Character, skillcard_code> skill_used;
    public static Action<Character, List<Character>> attacked; // <공격 주체, 공격 대상들>
    public static Action turn_start_phase;
    public static Action turn_end_phase;
    public static Action<Character> character_died;
    public static Action<bool> battle_ended; // true값이면 이긴 거, false면 진 거
    public static Action character_drag_started;
    public static Action character_drag_ended;
    public static Action<card> card_destroyed;


    // 스테이지 보여줄 때 씀
    public static Action character_drag_started_on_stageShow;
    public static Action character_drag_finished_on_stageShow;

    // 공용
    public static Action party_member_changed;
    public static Action<string> TMP_link_clicked;

}
