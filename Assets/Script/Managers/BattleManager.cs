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
    private int enemy_skill_set_count = 0; // ���� ��ų ���� �Ϸ�� �þ
    private int remaining_cost = 0;
    public static int MAX_COST = 4;

    // ���� ���� �÷��̾�� ĳ���� ����Ʈ
    public List<Character> playable_characters = new List<Character>();
    // ���� ���� �÷��̾�� ĳ���͵��� �� ����Ʈ
    public List<List<card>> hand_data = new List<List<card>>();
    // ���� ���� �� ĳ���� ����Ʈ
    public List<Character> enemy_characters = new List<Character>();
    // ���� �̹� �Ͽ� ��� ���� ī�� ����Ʈ
    public List<card> enemy_cards = new List<card>();

    [SerializeField] private GameObject battle_result_canvas;
    [SerializeField] private StageSettingSO StageSettingSO;

    public IEnumerator current_phase_coroutine;
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
        playable_characters.Clear();
        enemy_characters.Clear();
        hand_data.Clear();
        enemy_cards.Clear();
        battle_result_canvas = GameObject.Find("Battle Result Canvas");
        battle_result_canvas.SetActive(false);

        // ���� ��� ����
        Instantiate(StageSettingSO.stage_Settings[stage_index].background_prefab);

        // ĳ���� ������Ʈ �� Character �ν��Ͻ� ���� (�Ʊ� / �� ���)
        CharacterManager.instance.spawn_character(stage_index);

        // ī�� �� ����
        CardManager.instance.Setup_all();

        // �ʱ� �� ���� (���⼭ 1�� + turn_start_phase 1�� ����) �� �ִ� ���� : 7��
        give_card_to_all_playable_characters(1);

        // �ʱ� �ڽ�Ʈ ����
        fill_cost();


        // �� ����
        current_phase_coroutine = turn_start_phase();
        StartCoroutine(current_phase_coroutine);

        yield break;
    
    }

    // �� ���� ������
    IEnumerator turn_start_phase() 
    {
        current_phase = phases.turn_start_phase;

        // ī�� 1�� ����
        give_card_to_all_playable_characters(1);

        // �ڽ�Ʈ ����
        fill_cost();

        // �� ���� �̺�Ʈ ����
        ActionManager.turn_start_phase?.Invoke();

        // �� ��ų ���� ����
        current_phase_coroutine = enemy_skill_setting_phase();
        StartCoroutine(current_phase_coroutine);

        yield break;

    }

    // �� (�̵� ��) ��ų ���� ������
    IEnumerator enemy_skill_setting_phase() 
    {
        enemy_skill_set_count = 0;
        current_phase = phases.enemy_skill_setting_phase;
        yield return new WaitForSeconds(0.01f); // �д� ���� ���� ������ ��� ��

        ActionManager.enemy_skill_setting_phase?.Invoke();

        // ��ų ���� ������ �� ����
        yield return new WaitUntil(() => enemy_skill_set_count == enemy_characters.Count);

        current_phase_coroutine = player_skill_phase();
        StartCoroutine(current_phase_coroutine);
        yield break;

    }

    // �÷��̾� ��ų ��� ������
    IEnumerator player_skill_phase()
    {
        current_phase = phases.player_skill_phase;
        current_phase_coroutine = null;
        yield break;
    }

    // �� ��ų ��� ������
    public IEnumerator enemy_skill_phase()
    {
        current_phase = phases.enemy_skill_phase;

        // ī�� ���̶���Ʈ�� ����
        CardManager.instance.Change_active_hand(-1);
        ActionManager.enemy_skillcard_deactivate?.Invoke();

        // ���� ���� ī����� ������� ���
        while (enemy_cards.Count > 0) 
        {

            card card = enemy_cards[0];

            if (card.Data.IsDirectUsable) // ���� ��� ������ ī��� ���
            {
                BattleCalcManager.instance.set_using_card(card);
                BattleCalcManager.instance.set_target(card.target.GetComponent<Character>());
                BattleCalcManager.instance.Calc_enemy_turn_skill_use();
            }

            // ī�� �ı�
            card.Destroy_card();

            yield return new WaitForSeconds(0.5f);
            
        }

        // ��ų ��� ���� �ʱ�ȭ
        BattleCalcManager.instance.Clear_all();

        // �� ������ �������
        current_phase_coroutine = turn_end_phase();
        StartCoroutine(current_phase_coroutine);

        yield break;
    }

    IEnumerator turn_end_phase() 
    {
        // �� ���� ������ ����
        ActionManager.turn_end_phase?.Invoke();

        // ���� �� ���� �������
        current_phase_coroutine = turn_start_phase();
        StartCoroutine(current_phase_coroutine);
        yield break;
    }


    //Battle�� ���۽� ���� ���۵ǵ��� �ϱ�
    private void Check_battle_scene_load(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Battle") 
        { 
            battle_start_trigger = true;
        }
        
    }

    // ���� ������ �� ����
    private void OnBattleEnd(bool victory) 
    {
        // �� ���� �����
        CardManager.instance.Change_active_hand(-1);
        ActionManager.enemy_skillcard_deactivate?.Invoke();

        // ���� ���� ���� ����
        if (current_phase_coroutine != null) 
        {
            StopCoroutine(current_phase_coroutine);
        }

        // ���� ��� ���
        battle_result_canvas.SetActive(true);
        Battle_result_canvas canvas_Script = battle_result_canvas.GetComponent<Battle_result_canvas>();
        canvas_Script.Set_result(victory);

        // ���� ���� ��� �� ���
        if (victory) 
        {
            StartCoroutine(StageManager.instance.calc_and_show_battle_loot());
        }
    }

    private void give_card_to_all_playable_characters(int draw_number_of_times) 
    {
        for (int i = 0; i < playable_characters.Count; i++)
        {
            int character_index = playable_characters[i].Character_index;
            for (int j = 0; j < draw_number_of_times; j++)
            {
                if (hand_data[character_index].Count < 7)
                {
                    CardManager.instance.Summon_card(character_index);
                }
            }
        }
    }

    public int get_remaining_cost() 
    {
        return remaining_cost;
    }

    public void reduce_cost(int value) 
    {
        remaining_cost -= value;
        if (remaining_cost < 0) 
        {
            remaining_cost = 0;
        }

        ActionManager.set_cost(remaining_cost);
    }

    private void fill_cost() 
    {
        remaining_cost = MAX_COST;
        ActionManager.set_cost(remaining_cost);
    }

    private void OnCardDestroyed(card card) 
    {
        // �� ī���
        if (card.isEnemyCard)
        {
            enemy_cards.Remove(card);
        }
        // �÷��̾� ī���
        else
        {
            hand_data[card.owner.Character_index].Remove(card);
            CardManager.instance.Align_cards(CardManager.instance.active_index);
        }
    }

    private void OnCharacterDied(Character character) 
    {
        // �Ʊ��̸�
        if (!character.check_enemy()) 
        {
            // �÷��̾� ĳ���� ����Ʈ�� ���� ��� �ѱ�
            if (!playable_characters.Contains(character)) 
            {
                return;
            }

            // ĳ������ �� ���ֱ�
            for (int i = 0; i < hand_data[character.Character_index].Count; i++)
            {
                hand_data[character.Character_index][i].Destroy_card();
            }
           hand_data.RemoveAt(character.Character_index);

            // �Ʊ� ĳ���� ����Ʈ���� ���ֱ�
           playable_characters.Remove(character);

            // ���� ĳ���� �ε��� ����
            for (int i = 0; i < playable_characters.Count; i++)
            {
                Character remianing_character = playable_characters[i].GetComponent<Character>();
                remianing_character.Character_index = i;
            }
        }
        else // �� ĳ���͸�
        {
            // �� ĳ���� ����Ʈ�� ���� ��� �ѱ�
            if (!enemy_characters.Contains(character))
            {
                return;
            }

            // �� ĳ���� ����Ʈ���� ���ֱ�
            enemy_characters.Remove(character);

            // ���� ĳ���� �ε��� ����
            for (int i = 0; i < enemy_characters.Count; i++)
            {
                enemy_characters[i].GetComponent<Character>().Character_index = i;
            }
        }

        // ���� ������ �� ����
        if (enemy_characters.Count == 0)
        {
            ActionManager.battle_ended?.Invoke(true);
        }
        else if (playable_characters.Count == 0)
        {
            ActionManager.battle_ended?.Invoke(false);
        }
    }

    private void OnEnemySkillSetComplete() 
    {
        enemy_skill_set_count += 1;
    }

    private void Update()
    {
        if (battle_start_trigger) 
        {
            battle_start_trigger = false;
            current_phase_coroutine = battle(StageManager.instance.stage_index);
            StartCoroutine(current_phase_coroutine);
        }
    }

    void OnEnable()
    {
        // ��������Ʈ ü�� �߰�
        SceneManager.sceneLoaded += Check_battle_scene_load;

        // �̺�Ʈ �߰�
        ActionManager.battle_ended += OnBattleEnd;
        ActionManager.card_destroyed += OnCardDestroyed;
        ActionManager.character_died += OnCharacterDied;
        ActionManager.enemy_skill_set_complete += OnEnemySkillSetComplete;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Check_battle_scene_load;
        ActionManager.battle_ended -= OnBattleEnd;
        ActionManager.card_destroyed -= OnCardDestroyed;
        ActionManager.character_died -= OnCharacterDied;
        ActionManager.enemy_skill_set_complete -= OnEnemySkillSetComplete;
    }
}
