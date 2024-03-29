using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singletone<CharacterManager>
{
    public enum character_int_properties
    {
        max_health,
        current_health,
        max_willpower,
        current_willpower,
        number_of_skill_slots
    }

    public GameObject playable_character_base;

    public int playable_character_spawn_count = 0; //�÷��̾�� ĳ���Ͱ� �� �� �����Ǿ����� ����

    
    
    public Vector3[] playable_character_position_settings = new Vector3[4] { // �÷��̾�� ĳ���� ��ġ ����
        new Vector3(-8, 2, 0),
        new Vector3(-8, -3, 0),
        new Vector3(-3, 2, 0),
        new Vector3(-3, -3, 0)
    };


    public List<GameObject> playerble_Characters = new List<GameObject>(); // �÷��̾�� ĳ���͵� ����Ʈ

    public void spawn_character()
    {

        //�÷��̾�� ĳ���� ����
        for (int i = 0; i < 4; i++) 
        {
            
            SpawnCharactor.instance.spawn_playerble_charactor(playable_character_base);
            
        }
        BattleManager.instance.is_Characters_spawned = true;
    }
}
