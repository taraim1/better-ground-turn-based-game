using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleEventManager : MonoBehaviour
{


    // ��Ʋ���� ��
    public static Action enemy_skill_setting_phase;
    public static Action enemy_skill_card_deactivate;
    public static Action skill_used;
    public static Action turn_start_phase;
    public static Action player_character_died;

    // �������� ������ �� ��
    public static Action character_drag_started_on_stageShow;
    public static Action character_drag_finished_on_stageShow;

    // ����
    public static Action party_member_changed;

}
