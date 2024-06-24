using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum skill_effect_code
{
    none,
    willpower_consumption,
    willpower_recovery,
    ignition
}

public enum skill_effect_timing
{
    immediate,
    after_use,
}

[System.Serializable]
public struct SkillEffect
{
    public skill_effect_code code;
    public skill_effect_timing timing;
    public List<int> parameters;

}
