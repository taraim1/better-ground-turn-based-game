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

    public Vector3[] playable_character_position_settings = new Vector3[4]; //�÷��̾�� ĳ���� ���� ��ġ

    //���� ���� ĳ������ ������ �ν��Ͻ� ����Ʈ
    public List<Playable_Character> playable_Characters_data = new List<Playable_Character>();

    //���� ���� ĳ������ ���ӿ�����Ʈ ����Ʈ
    public List<GameObject> playable_characters = new List<GameObject>();
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
        playable_Characters_data.Clear();
        playable_characters.Clear();

        //��Ƽ ������ �ҷ�����
        PartyManager.instance.read_Json_file();

        //���� ���� ĳ������ Playable Character �ν��Ͻ� ����Ʈ�� �ҷ����� (���������)
        for (int i = 0; i < PartyManager.party_member_count; i++) 
        { 
            playable_Characters_data.Add(PartyManager.instance.get_character_of_party(i));
        }
 
        //ĳ���� ������Ʈ ����
        CharacterManager.instance.spawn_character();

        //ĳ���� ������Ʈ �̸� ����
        for (int i = 0; i < PartyManager.party_member_count; i++) 
        {
            playable_characters[i].name = playable_Characters_data[i].get_character_name();
        }

        //ü�¹� ����Ʈ �ʱ�ȭ
        BattleUI_Manager.instance.clear_health_bar_list();

        //ü�¹� ��ȯ
        for (int i = 0; i < PartyManager.party_member_count; i++) 
        {
            BattleUI_Manager.instance.summon_health_bar(playable_characters[i]);
        }

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
