using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnCharactor : Singletone<SpawnCharactor>
{
   
    public void spawn_playerble_charactor(GameObject character) 
    {
        //플레이어블 캐릭터가 소환될 위치를 불러옴
        Vector3 spawn_pos = CharacterManager.instance.playable_character_position_settings[CharacterManager.instance.playable_character_spawn_count];

        //플레이어블 캐릭터 생성
        GameObject obj = Instantiate(character, spawn_pos, Quaternion.identity);

        CharacterManager.instance.playable_character_spawn_count ++;
    }

  
}
