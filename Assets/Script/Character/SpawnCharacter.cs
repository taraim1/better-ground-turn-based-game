using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnCharactor : Singletone<SpawnCharactor>
{
   
    public void spawn_playerble_charactor(GameObject character) 
    {
        //�÷��̾�� ĳ���Ͱ� ��ȯ�� ��ġ�� �ҷ���
        Vector3 spawn_pos = CharacterManager.instance.playable_character_position_settings[CharacterManager.instance.playable_character_spawn_count];

        //�÷��̾�� ĳ���� ����
        GameObject obj = Instantiate(character, spawn_pos, Quaternion.identity);

        CharacterManager.instance.playable_character_spawn_count ++;
    }

  
}
