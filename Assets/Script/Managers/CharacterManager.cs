using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterManager : Singletone<CharacterManager>
{


    public GameObject playable_character_base;


    public List<GameObject> playerble_Characters = new List<GameObject>(); // �÷��̾�� ĳ���͵� ����Ʈ

    public void spawn_character()
    {
       for (int i = 0; i < 4; i++) 
        {

            //�÷��̾�� ĳ���Ͱ� ��ȯ�� ��ġ�� �ҷ���
            Vector3 spawn_pos = BattleManager.instance.playable_character_position_settings[i];

            //�÷��̾�� ĳ���� ����
            GameObject obj = Instantiate(playable_character_base, spawn_pos, Quaternion.identity);

            //�÷��̾�� ĳ���� ������Ʈ�� BattleManager�� ����Ʈ�� �ֱ�
            BattleManager.instance.playable_characters.Add(obj);

        }

        BattleManager.instance.is_Characters_spawned = true;
    }
}
