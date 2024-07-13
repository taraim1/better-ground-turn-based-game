using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionManager : MonoBehaviour
{


    // ��Ʋ���� ��
    public static Action enemy_skill_setting_phase;
    public static Action enemy_skill_card_deactivate;
    public static Action skill_used;
    public static Action<Character, List<Character>> attacked; // <���� ��ü, ���� ����>
    public static Action turn_start_phase;
    public static Action turn_end_phase;
    public static Action player_character_died; 
    public static Action<bool> battle_ended; // true���̸� �̱� ��, false�� �� ��

    // �������� ������ �� ��
    public static Action character_drag_started_on_stageShow;
    public static Action character_drag_finished_on_stageShow;

    // ����
    public static Action party_member_changed;
    public static Action<string> TMP_link_clicked;
    public static Action<ResourceManager.resource_code> resource_changed;

}
