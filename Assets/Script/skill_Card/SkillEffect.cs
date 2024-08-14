using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum skill_effect_code
{
    none,
    willpower_consumption,
    willpower_recovery,
    ignition,
    fire_enchantment
}

public enum skill_effect_timing
{
    immediate,
    after_use,
}

public enum skill_effect_target
{
    owner,
    target,
}


[System.Serializable]
public class SkillEffect
{
    public skill_effect_code code;
    public skill_effect_timing timing;
    public skill_effect_target target;
    public List<int> parameters;

}
