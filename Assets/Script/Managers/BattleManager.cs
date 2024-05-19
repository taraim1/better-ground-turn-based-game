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


    // ���� ���� �÷��̾�� ĳ������ ���ӿ�����Ʈ ����Ʈ
    public List<GameObject> playable_characters = new List<GameObject>();

    // ���� ���� �÷��̾�� ĳ���͵��� ������ ����Ʈ
    public List<Character> playable_character_data = new List<Character>();

    // ���� ���� �÷��̾�� ĳ���͵��� �� ����Ʈ
    public List<List<card>> hand_data = new List<List<card>>();

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
        hand_data.Clear();
        

        // ĳ���� ������Ʈ �� Character �ν��Ͻ� ����
        CharacterManager.instance.spawn_character();

        // ī�� �� ����
        CardManager.instance.Setup_all();

        // ���� ü��, ���ŷ� �ʱ�ȭ
        for (int i = 0; i < playable_character_data.Count; i++) 
        {
            playable_character_data[i].current_health = playable_character_data[i].get_max_health_of_level(playable_character_data[i].level);
            playable_character_data[i].current_willpower = playable_character_data[i].get_max_willpower_of_level(playable_character_data[i].level);
        }

        // ü�¹�, ���ŷ¹� �ʱ�ȭ
        BattleUI_Manager.instance.clear_all_bar_list();


        for (int i = 0; i < playable_characters.Count; i++) 
        {
            // ü�¹�, ���ŷ¹� ����
            BattleUI_Manager.instance.summon_UI_bar(BattleUI_Manager.UI_bars.health_bar, playable_characters[i]);
            BattleUI_Manager.instance.summon_UI_bar(BattleUI_Manager.UI_bars.willpower_bar, playable_characters[i]);

            // ü�¹�, ���ŷ¹� �ʱⰪ ����
            BattleUI_Manager.instance.set_bar_property(BattleUI_Manager.UI_bars.health_bar, i, BattleUI_Manager.UI_bars_properties.max_value, playable_character_data[i].get_max_health_of_level(playable_character_data[i].level));
            BattleUI_Manager.instance.set_bar_property(BattleUI_Manager.UI_bars.health_bar, i, BattleUI_Manager.UI_bars_properties.current_value, playable_character_data[i].current_health);
            BattleUI_Manager.instance.set_bar_property(BattleUI_Manager.UI_bars.willpower_bar, i, BattleUI_Manager.UI_bars_properties.max_value, playable_character_data[i].get_max_willpower_of_level(playable_character_data[i].level));
            BattleUI_Manager.instance.set_bar_property(BattleUI_Manager.UI_bars.willpower_bar, i, BattleUI_Manager.UI_bars_properties.current_value, playable_character_data[i].current_willpower);
        }






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
