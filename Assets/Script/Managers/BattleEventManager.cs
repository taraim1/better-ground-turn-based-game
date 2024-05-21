using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleEventManager : MonoBehaviour
{


    // πË∆≤ø°º≠ æ∏
    public static Action enemy_skill_setting_phase;


    public static void Trigger_event(string type)
    {
        switch (type) 
        { 
            case "Enemy_skill_setting_phase":
                enemy_skill_setting_phase?.Invoke();
                break;
        }
    }
}
