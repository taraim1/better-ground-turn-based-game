using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterManager : Singletone<CharacterManager>
{


    public GameObject playable_character_base;


    public List<GameObject> playerble_Characters = new List<GameObject>(); // 플레이어블 캐릭터들 리스트

    public void spawn_character()
    {
       for (int i = 0; i < 4; i++) 
        {

            //플레이어블 캐릭터가 소환될 위치를 불러옴
            Vector3 spawn_pos = BattleManager.instance.playable_character_position_settings[i];

            //플레이어블 캐릭터 생성
            GameObject obj = Instantiate(playable_character_base, spawn_pos, Quaternion.identity);

            //플레이어블 캐릭터 오브젝트를 BattleManager의 리스트에 넣기
            BattleManager.instance.playable_characters.Add(obj);

        }

        BattleManager.instance.is_Characters_spawned = true;
    }
}
