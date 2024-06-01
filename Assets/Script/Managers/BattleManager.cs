using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;


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

    private cost_meter cost_meter; // �ڽ�Ʈ �� �����ִ� ������Ʈ

    // ���� ���� �÷��̾�� ĳ������ ���ӿ�����Ʈ ����Ʈ
    public List<GameObject> playable_characters = new List<GameObject>();
    // ���� ���� �÷��̾�� ĳ���͵��� �� ����Ʈ
    public List<List<card>> hand_data = new List<List<card>>();
    // ���� ���� �� ĳ������ ���ӿ�����Ʈ ����Ʈ
    public List<GameObject> enemy_characters = new List<GameObject>();
    // ���� �̹� �Ͽ� ��� ���� ī�� ����Ʈ
    public List<card> enemy_cards = new List<card>();

    public enum phases // �� ���� ������ ����
    { 
        turn_start_phase,
        enemy_skill_setting_phase,
        player_skill_phase,
        enemy_skill_phase,
        turn_end_phase
    }

    IEnumerator battle(int stage_index) //���� �ڷ�ƾ
    {
        //���� �ʱ�ȭ
        is_Characters_spawned = false;
        playable_characters.Clear();
        enemy_characters.Clear();
        hand_data.Clear();
        enemy_cards.Clear();

        // ĳ���� ������Ʈ �� Character �ν��Ͻ� ���� (�Ʊ� / �� ���)
        CharacterManager.instance.spawn_character(stage_index);

        // ī�� �� ����
        CardManager.instance.Setup_all();

        // �ʱ� �� ���� (���⼭ 1�� + turn_start_phase 1�� ����) �� �ִ� ���� : 7��
        for (int i = 0; i < playable_characters.Count; i++) 
        {
            int card_draw_number_of_times = 1;
            int character_index = playable_characters[i].GetComponent<Character>().Character_index;
            for (int j = 0; j < card_draw_number_of_times; j++) 
            {
                if (hand_data[character_index].Count < 7) 
                {
                    CardManager.instance.Summon_card(character_index);
                }
            }
        }

        // �ʱ� �ڽ�Ʈ ����
        cost_meter = GameObject.Find("cost_meter").GetComponent<cost_meter>();
        cost_meter.Setup(4, 4);

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
        current_phase = phases.turn_start_phase;

        // ī�� 1�� ����
        for (int i = 0; i < playable_characters.Count; i++)
        {
            int character_index = playable_characters[i].GetComponent<Character>().Character_index;
            int card_draw_number_of_times = 1;

            for (int j = 0; j < card_draw_number_of_times; j++)
            {
                if (hand_data[character_index].Count < 7)
                {
                    CardManager.instance.Summon_card(character_index);
                }
            }
        }

        // �ڽ�Ʈ 4 ����
        cost_meter.Current_cost += 4;

        // �� ��ų ���� ����
        StartCoroutine(enemy_skill_setting_phase());

        // �� ���� �̺�Ʈ ����
        BattleEventManager.turn_start_phase?.Invoke();

        yield break;

    }

    // �� ��ų ���� ������
    IEnumerator enemy_skill_setting_phase() 
    {
        enemy_skill_set_count = 0;
        current_phase = phases.enemy_skill_setting_phase;
        yield return new WaitForSeconds(0.01f); // �д� ���� ���� ������ ��� ��

        BattleEventManager.enemy_skill_setting_phase?.Invoke();

        // ��ų ���� ������ �� ����
        while (true) 
        {
            if (enemy_skill_set_count == enemy_characters.Count) 
            {
                StartCoroutine(player_skill_phase());
                yield break;
            }

            yield return new WaitForSeconds(0.01f);
        }
        
    }

    // �÷��̾� ��ų ��� ������
    IEnumerator player_skill_phase()
    {
        current_phase = phases.player_skill_phase;
        yield break;
    }

    // �� ��ų ��� ������
    public IEnumerator enemy_skill_phase()
    {
        current_phase = phases.enemy_skill_phase;

        // ī�� ���̶���Ʈ�� ����
        CardManager.instance.Change_active_hand(-1);
        BattleEventManager.enemy_skill_card_deactivate?.Invoke();

        // ���� ���� ī����� ������� ���
        while (enemy_cards.Count > 0) 
        {

            card card = enemy_cards[0];

            if (card.Card.isDirectUsable) // ���� ��� ������ ī��� ���
            {
                BattleCalcManager.instance.set_using_card(card);
                BattleCalcManager.instance.set_target(card.target.GetComponent<Character>());
                BattleCalcManager.instance.Calc_enemy_turn_skill_use();
            }

            // ī�� �ı�
            CardManager.instance.Destroy_card(card);

            yield return new WaitForSeconds(0.5f);
            
        }

        // ��ų ��� ���� �ʱ�ȭ
        BattleCalcManager.instance.Clear_all();

        // �� ������ �������
        StartCoroutine(turn_end_phase());

        yield break;
    }

    IEnumerator turn_end_phase() 
    {
        // ���� �� ���� �������
        StartCoroutine(turn_start_phase());
        yield break;
    }

    void OnEnable()
    {
        // ��������Ʈ ü�� �߰�
        SceneManager.sceneLoaded += Check_battle_scene_load;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Check_battle_scene_load;
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
            StartCoroutine(battle(StageManager.instance.stage_index));
        }
    }
}
