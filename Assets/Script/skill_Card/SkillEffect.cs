using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum skill_effect_code
{
    none,
    willpower_consumption,
    willpower_recovery
}

public enum skill_effect_timing
{
    immediate,
    in_clash,
    after_clash,
}

[System.Serializable]
public struct SkillEffect
{
    public skill_effect_code code;
    public skill_effect_timing timing;
    public List<float> parameters;

}
