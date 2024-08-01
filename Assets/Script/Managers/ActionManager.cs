using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionManager : MonoBehaviour
{


    // ��Ʋ���� ��
    public static Action enemy_skill_setting_phase;
    public static Action enemy_skillData_deactivate;
    public static Action<Character, skillcard_code> skill_used;
    public static Action<Character, List<Character>> attacked; // <���� ��ü, ���� ����>
    public static Action turn_start_phase;
    public static Action turn_end_phase;
    public static Action<Character> character_died;
    public static Action<bool> battle_ended; // true���̸� �̱� ��, false�� �� ��
    public static Action character_drag_started;
    public static Action character_drag_ended;
    public static Action<card> card_destroyed;


    // �������� ������ �� ��
    public static Action character_drag_started_on_stageShow;
    public static Action character_drag_finished_on_stageShow;

    // ����
    public static Action party_member_changed;
    public static Action<string> TMP_link_clicked;

}
