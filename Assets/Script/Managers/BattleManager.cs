using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BattleManager : Singletone<BattleManager> //�̱�����
{
    
    public int current_turn = 0; // ���� ��
    public int current_phase = 0; // ���� ������
    public bool battle_start_trigger = false; // ������ ���۽�Ű�� Ʈ����
    public bool is_Characters_spawned = false; // ���� ���۽� �÷����̾�� �� ĳ���Ͱ� �����Ǿ��°�?
    public bool is_Characters_loaded = false; // ���� ���۽� ĳ���� �����Ͱ� �ҷ������°�?
    public enum phases // �� ���� ������ ����
    { 
        turn_start_effect_phase,
        enemy_skill_setting_phase,
        player_skill_setting_phase,
        fight_phase,
        turn_end_effect_phase
    }

    IEnumerator battle() //���� �ڷ�ƾ
    {
        //���� �ʱ�ȭ
        is_Characters_spawned = false;

        //ĳ���� ����
        CharacterManager.instance.spawn_character();

        yield break;
    
    }

    private void Update()
    {
        if (battle_start_trigger) 
        {
            battle_start_trigger = false;
            StartCoroutine(battle());
        }
    }
}
