using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class BattleManager : Singletone<BattleManager> // �̱�����
{
    
    public int current_turn = 0; // ���� ��
    public int current_phase = 0; // ���� ������
    public bool battle_start_trigger = false; // ������ ���۽�Ű�� Ʈ����
    public bool is_Characters_spawned = false; // ���� ���۽� �÷����̾�� �� ĳ���Ͱ� �����Ǿ��°�?
    public bool is_Characters_loaded = false; // ���� ���۽� ĳ���� �����Ͱ� �ҷ������°�?

    public Vector3[] playable_character_position_settings = new Vector3[4]; //�÷��̾�� ĳ���� ���� ��ġ
    public Vector3[] enemy_character_position_settings = new Vector3[4]; //�� ĳ���� ���� ��ġ


    // ���� ���� �÷��̾�� ĳ������ ���ӿ�����Ʈ ����Ʈ
    public List<GameObject> playable_characters = new List<GameObject>();
    // ���� ���� �÷��̾�� ĳ���͵��� ������ ����Ʈ
    public List<Character> playable_character_data = new List<Character>();
    // ���� ���� �÷��̾�� ĳ���͵��� �� ����Ʈ
    public List<List<card>> hand_data = new List<List<card>>();
    // ���� ���� �� ĳ������ ���ӿ�����Ʈ ����Ʈ
    public List<GameObject> enemy_characters = new List<GameObject>();
    // ���� ���� �� ĳ���͵��� ������ ����Ʈ
    public List<Character> enemy_character_data = new List<Character>();

    public enum phases // �� ���� ������ ����
    { 
        turn_start_effect_phase,
        enemy_skill_setting_phase,
        player_skill_phase,
        enemy_skill_phase,
        turn_end_effect_phase
    }

    IEnumerator battle() //���� �ڷ�ƾ
    {
        //���� �ʱ�ȭ
        is_Characters_spawned = false;
        playable_characters.Clear();
        playable_character_data.Clear();
        enemy_characters.Clear();
        enemy_character_data.Clear();
        hand_data.Clear();
        

        // ĳ���� ������Ʈ �� Character �ν��Ͻ� ���� (�Ʊ� / �� ���)
        CharacterManager.instance.spawn_character(0);

        // ī�� �� ����
        CardManager.instance.Setup_all();

        // �ʱ� �� ���� (3�� ����)
        for (int i = 0; i < hand_data.Count; i++) 
        {
            for (int j = 0; j < 3; j++) 
            {
                CardManager.instance.Summon_card(i);
            }
        }

        // ĳ���͵��� ���� ü��, ���ŷ� �ʱ�ȭ
        for (int i = 0; i < playable_character_data.Count; i++) 
        {
            playable_character_data[i].current_health = playable_character_data[i].get_max_health_of_level(playable_character_data[i].level);
            playable_character_data[i].current_willpower = playable_character_data[i].get_max_willpower_of_level(playable_character_data[i].level);
        }
        for (int i = 0; i < enemy_character_data.Count; i++)
        {
            enemy_character_data[i].current_health = enemy_character_data[i].get_max_health_of_level(enemy_character_data[i].level);
            enemy_character_data[i].current_willpower = enemy_character_data[i].get_max_willpower_of_level(enemy_character_data[i].level);
        }

        // ü�¹�, ���ŷ¹� �ʱ�ȭ �� ����
        BattleUI_Manager.instance.Setup_health_and_willpower_bars();

        yield break;
    
    }

    void OnEnable()
    {
        // ��������Ʈ ü�� �߰�
        SceneManager.sceneLoaded += Check_battle_scene_load;
    }

    //Battle�� ���۽� ���� ���۵ǵ��� �ϱ�
    private void Check_battle_scene_load(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Battle") 
        { 
            battle_start_trigger = true;
        }
        
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
