using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class BattleManager : Singletone<BattleManager> // 싱글톤임
{
    
    public int current_turn = 0; // 현재 턴
    public phases current_phase; // 현재 페이즈
    public bool battle_start_trigger = false; // 전투를 시작시키는 트리거
    public bool is_Characters_spawned = false; // 전투 시작시 플레이이어와 적 캐릭터가 스폰되었는가?
    public bool is_Characters_loaded = false; // 전투 시작시 캐릭터 데이터가 불러와졌는가?
    public int enemy_skill_set_count = 0; // 적의 스킬 설정 완료시 늘어남


    public Vector3[] playable_character_position_settings = new Vector3[4]; //플레이어블 캐릭터 스폰 위치
    public Vector3[] enemy_character_position_settings = new Vector3[4]; //적 캐릭터 스폰 위치

    private GameObject cost_meter; // 코스트 양 보여주는 오브젝트

    // 전투 중인 플레이어블 캐릭터의 게임오브젝트 리스트
    public List<GameObject> playable_characters = new List<GameObject>();
    // 전투 중인 플레이어블 캐릭터들의 패 리스트
    public List<List<card>> hand_data = new List<List<card>>();
    // 전투 중인 적 캐릭터의 게임오브젝트 리스트
    public List<GameObject> enemy_characters = new List<GameObject>();

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
        enemy_characters.Clear();
        hand_data.Clear();
        current_phase = phases.turn_start_effect_phase;

        // 캐릭터 오브젝트 및 Character 인스턴스 생성 (아군 / 적 모두)
        CharacterManager.instance.spawn_character(0);

        // 카드 덱 세팅
        CardManager.instance.Setup_all();

        // 초기 패 세팅 (2장 + turn_start_phase 1장 뽑음)
        for (int i = 0; i < hand_data.Count; i++) 
        {
            for (int j = 0; j < 1; j++) 
            {
                CardManager.instance.Summon_card(i);
            }
        }

        // 초기 코스트 설정
        cost_meter = GameObject.Find("cost_meter");
        cost_meter.GetComponent<cost_meter>().Setup(4, 4);

        // 캐릭터들의 현재 체력, 정신력 초기화
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




        // 턴 시작
        StartCoroutine(turn_start_phase());

        yield break;
    
    }

    // 턴 시작 페이즈
    IEnumerator turn_start_phase() 
    {
        current_phase = phases.turn_start_effect_phase;

        // 카드 1장 뽑음
        for (int i = 0; i < hand_data.Count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                CardManager.instance.Summon_card(i);
            }
        }

        // 적 스킬 설정 시작
        StartCoroutine(enemy_skill_setting_phase());
        yield break;
    }

    // 적 스킬 설정 페이즈
    IEnumerator enemy_skill_setting_phase() 
    {
        enemy_skill_set_count = 0;
        current_phase = phases.enemy_skill_setting_phase;
        BattleEventManager.Trigger_event("Enemy_skill_setting_phase");

        // 스킬 설정 끝나는 거 감지
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

    // 플레이어 스킬 사용 페이즈
    IEnumerator player_skill_phase()
    {
        current_phase = phases.player_skill_phase;
        yield break;
    }

    // 적 스킬 사용 페이즈
    IEnumerator enemy_skill_phase()
    {
        current_phase = phases.enemy_skill_phase;
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
