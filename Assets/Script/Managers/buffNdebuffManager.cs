using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffNdebuffManager : Singletone<buffNdebuffManager>
{
    [SerializeField] private List<Sprite> character_effect_sprites;
    [SerializeField] private characterEffectSO characterEffectSO;



    public character_effect_timing get_character_effect_timing_by_code(character_effect_code code) 
    {
        switch (code) 
        {
            case character_effect_code.flame:
                return character_effect_timing.turn_ended;
        }

        return character_effect_timing.turn_started;
    }
}
