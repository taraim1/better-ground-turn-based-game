using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class BattleManager : Singletone<BattleManager> // �̱�����
{
    
    public int current_turn = 0; // ���� ��
    public phases current_phase; // ���� ������
    public bool battle_start_trigger = false; // ������ ���۽�Ű�� Ʈ����
    public bool is_Characters_spawned = false; // ���� ���۽� �÷����̾�� �� ĳ���Ͱ� �����Ǿ��°�?
    public bool is_Characters_loaded = false; // ���� ���۽� ĳ���� �����Ͱ� �ҷ������°�?
    public int enemy_skill_set_count = 0; // ���� ��ų ���� �Ϸ�� �þ


    public Vector3[] playable_character_position_settings = new Vector3[4]; //�÷��̾�� ĳ���� ���� ��ġ
    public Vector3[] enemy_character_position_settings = new Vector3[4]; //�� ĳ���� ���� ��ġ

    private GameObject cost_meter; // �ڽ�Ʈ �� �����ִ� ������Ʈ

    // ���� ���� �÷��̾�� ĳ������ ���ӿ�����Ʈ ����Ʈ
    public List<GameObject> playable_characters = new List<GameObject>();
    // ���� ���� �÷��̾�� ĳ���͵��� �� ����Ʈ
    public List<List<card>> hand_data = new List<List<card>>();
    // ���� ���� �� ĳ������ ���ӿ�����Ʈ ����Ʈ
    public List<GameObject> enemy_characters = new List<GameObject>();

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
        enemy_characters.Clear();
        hand_data.Clear();
        current_phase = phases.turn_start_effect_phase;

        // ĳ���� ������Ʈ �� Character �ν��Ͻ� ���� (�Ʊ� / �� ���)
        CharacterManager.instance.spawn_character(0);

        // ī�� �� ����
        CardManager.instance.Setup_all();

        // �ʱ� �� ���� (2�� + turn_start_phase 1�� ����)
        for (int i = 0; i < hand_data.Count; i++) 
        {
            for (int j = 0; j < 1; j++) 
            {
                CardManager.instance.Summon_card(i);
            }
        }

        // �ʱ� �ڽ�Ʈ ����
        cost_meter = GameObject.Find("cost_meter");
        cost_meter.GetComponent<cost_meter>().Setup(4, 4);

        // ĳ���͵��� ���� ü��, ���ŷ� �ʱ�ȭ
        for (int i = 0; i < playable_characters.Count; i++) 
        {
            Character cha = playable_characters[i].GetComponent<Character>();
            cha.current_health = cha.get_max_health_of_level(cha.level);
            cha.current_willpower = cha.get_max_willpower_of_level(cha.level);
        }
        for (int i = 0; i < enemy_characters.Count; i++)
        {
            Character cha = enemy_characters[i].GetComponent<Character>();
            cha.current_health = cha.get_max_health_of_level(cha.level);
            cha.current_willpower = cha.get_max_willpower_of_level(cha.level);
        }




        // �� ����
        StartCoroutine(turn_start_phase());

        yield break;
    
    }

    // �� ���� ������
    IEnumerator turn_start_phase() 
    {
        current_phase = phases.turn_start_effect_phase;

        // ī�� 1�� ����
        for (int i = 0; i < hand_data.Count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                CardManager.instance.Summon_card(i);
            }
        }

        // �� ��ų ���� ����
        StartCoroutine(enemy_skill_setting_phase());
        yield break;
    }

    // �� ��ų ���� ������
    IEnumerator enemy_skill_setting_phase() 
    {
        enemy_skill_set_count = 0;
        current_phase = phases.enemy_skill_setting_phase;
        BattleEventManager.Trigger_event("Enemy_skill_setting_phase");

        // ��ų ���� ������ �� ����
        while (true) 
        {
            if (enemy_skill_set_count == enemy_characters.Count) 
            {
                StartCoroutine(player_skill_phase());
                yield break;
            }

            yield return new WaitForSeconds(0.02f);
        }
        
    }

    // �÷��̾� ��ų ��� ������
    IEnumerator player_skill_phase()
    {
        current_phase = phases.player_skill_phase;
        yield break;
    }

    // �� ��ų ��� ������
    IEnumerator enemy_skill_phase()
    {
        current_phase = phases.enemy_skill_phase;
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
