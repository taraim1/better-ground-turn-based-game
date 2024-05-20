using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class BattleManager : Singletone<BattleManager> // 싱글톤임
{
    
    public int current_turn = 0; // 현재 턴
    public int current_phase = 0; // 현재 페이즈
    public bool battle_start_trigger = false; // 전투를 시작시키는 트리거
    public bool is_Characters_spawned = false; // 전투 시작시 플레이이어와 적 캐릭터가 스폰되었는가?
    public bool is_Characters_loaded = false; // 전투 시작시 캐릭터 데이터가 불러와졌는가?

    public Vector3[] playable_character_position_settings = new Vector3[4]; //플레이어블 캐릭터 스폰 위치
    public Vector3[] enemy_character_position_settings = new Vector3[4]; //적 캐릭터 스폰 위치


    // 전투 중인 플레이어블 캐릭터의 게임오브젝트 리스트
    public List<GameObject> playable_characters = new List<GameObject>();
    // 전투 중인 플레이어블 캐릭터들의 데이터 리스트
    public List<Character> playable_character_data = new List<Character>();
    // 전투 중인 플레이어블 캐릭터들의 패 리스트
    public List<List<card>> hand_data = new List<List<card>>();
    // 전투 중인 적 캐릭터의 게임오브젝트 리스트
    public List<GameObject> enemy_characters = new List<GameObject>();
    // 전투 중인 적 캐릭터들의 데이터 리스트
    public List<Character> enemy_character_data = new List<Character>();

    public enum phases // 한 턴의 페이즈 모음
    { 
        turn_start_effect_phase,
        enemy_skill_setting_phase,
        player_skill_phase,
        enemy_skill_phase,
        turn_end_effect_phase
    }

    IEnumerator battle() //전투 코루틴
    {
        //변수 초기화
        is_Characters_spawned = false;
        playable_characters.Clear();
        playable_character_data.Clear();
        enemy_characters.Clear();
        enemy_character_data.Clear();
        hand_data.Clear();
        

        // 캐릭터 오브젝트 및 Character 인스턴스 생성 (아군 / 적 모두)
        CharacterManager.instance.spawn_character(0);

        // 카드 덱 세팅
        CardManager.instance.Setup_all();

        // 초기 패 세팅 (3장 뽑음)
        for (int i = 0; i < hand_data.Count; i++) 
        {
            for (int j = 0; j < 3; j++) 
            {
                CardManager.instance.Summon_card(i);
            }
        }

        // 캐릭터들의 현재 체력, 정신력 초기화
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

        // 체력바, 정신력바 초기화 및 생성
        BattleUI_Manager.instance.Setup_health_and_willpower_bars();

        yield break;
    
    }

    void OnEnable()
    {
        // 델리게이트 체인 추가
        SceneManager.sceneLoaded += Check_battle_scene_load;
    }

    //Battle씬 시작시 전투 시작되도록 하기
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
