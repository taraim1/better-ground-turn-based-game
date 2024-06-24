using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;


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

    private cost_meter cost_meter; // 코스트 양 보여주는 오브젝트

    // 전투 중인 플레이어블 캐릭터의 게임오브젝트 리스트
    public List<GameObject> playable_characters = new List<GameObject>();
    // 전투 중인 플레이어블 캐릭터들의 패 리스트
    public List<List<card>> hand_data = new List<List<card>>();
    // 전투 중인 적 캐릭터의 게임오브젝트 리스트
    public List<GameObject> enemy_characters = new List<GameObject>();
    // 적이 이번 턴에 사용 중인 카드 리스트
    public List<card> enemy_cards = new List<card>();

    [SerializeField] private GameObject battle_result_canvas;

    public IEnumerator current_phase_coroutine;
    public enum phases // 한 턴의 페이즈 모음
    { 
        turn_start_phase,
        enemy_skill_setting_phase,
        player_skill_phase,
        enemy_skill_phase,
        turn_end_phase
    }

    IEnumerator battle(int stage_index) //전투 코루틴
    {

        //변수 초기화
        is_Characters_spawned = false;
        playable_characters.Clear();
        enemy_characters.Clear();
        hand_data.Clear();
        enemy_cards.Clear();
        battle_result_canvas = GameObject.Find("Battle Result Canvas");
        battle_result_canvas.SetActive(false);


        // 캐릭터 오브젝트 및 Character 인스턴스 생성 (아군 / 적 모두)
        CharacterManager.instance.spawn_character(stage_index);

        // 카드 덱 세팅
        CardManager.instance.Setup_all();

        // 초기 패 세팅 (여기서 1장 + turn_start_phase 1장 뽑음) 패 최대 개수 : 7장
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

        // 초기 코스트 설정
        cost_meter = GameObject.Find("cost_meter").GetComponent<cost_meter>();
        cost_meter.Setup(4, 4);

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
        current_phase_coroutine = turn_start_phase();
        StartCoroutine(current_phase_coroutine);

        yield break;
    
    }

    // 턴 시작 페이즈
    IEnumerator turn_start_phase() 
    {
        current_phase = phases.turn_start_phase;

        // 카드 1장 뽑음
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

        // 코스트 4 증가
        cost_meter.Current_cost += 4;

        // 턴 시작 이벤트 날림
        BattleEventManager.turn_start_phase?.Invoke();

        // 적 스킬 설정 시작
        current_phase_coroutine = enemy_skill_setting_phase();
        StartCoroutine(current_phase_coroutine);

        yield break;

    }

    // 적 스킬 설정 페이즈
    IEnumerator enemy_skill_setting_phase() 
    {
        enemy_skill_set_count = 0;
        current_phase = phases.enemy_skill_setting_phase;
        yield return new WaitForSeconds(0.01f); // 패닉 관련 버그 때문에 잠시 쉼

        BattleEventManager.enemy_skill_setting_phase?.Invoke();

        // 스킬 설정 끝나는 거 감지
        while (true) 
        {
            if (enemy_skill_set_count == enemy_characters.Count) 
            {
                current_phase_coroutine = player_skill_phase();
                StartCoroutine(current_phase_coroutine);
                yield break;
            }

            yield return new WaitForSeconds(0.01f);
        }
        
    }

    // 플레이어 스킬 사용 페이즈
    IEnumerator player_skill_phase()
    {
        current_phase = phases.player_skill_phase;
        current_phase_coroutine = null;
        yield break;
    }

    // 적 스킬 사용 페이즈
    public IEnumerator enemy_skill_phase()
    {
        current_phase = phases.enemy_skill_phase;

        // 카드 하이라이트들 해제
        CardManager.instance.Change_active_hand(-1);
        BattleEventManager.enemy_skill_card_deactivate?.Invoke();

        // 적의 남은 카드들을 순서대로 사용
        while (enemy_cards.Count > 0) 
        {

            card card = enemy_cards[0];

            if (card.Card.isDirectUsable) // 직접 사용 가능인 카드면 사용
            {
                BattleCalcManager.instance.set_using_card(card);
                BattleCalcManager.instance.set_target(card.target.GetComponent<Character>());
                BattleCalcManager.instance.Calc_enemy_turn_skill_use();
            }

            // 카드 파괴
            CardManager.instance.Destroy_card(card);

            yield return new WaitForSeconds(0.5f);
            
        }

        // 스킬 사용 판정 초기화
        BattleCalcManager.instance.Clear_all();

        // 턴 끝나는 페이즈로
        current_phase_coroutine = turn_end_phase();
        StartCoroutine(current_phase_coroutine);

        yield break;
    }

    IEnumerator turn_end_phase() 
    {
        // 턴 엔드 페이즈 날림
        BattleEventManager.turn_end_phase?.Invoke();

        // 다음 턴 시작 페이즈로
        current_phase_coroutine = turn_start_phase();
        StartCoroutine(current_phase_coroutine);
        yield break;
    }

    void OnEnable()
    {
        // 델리게이트 체인 추가
        SceneManager.sceneLoaded += Check_battle_scene_load;

        // 이벤트 추가
        BattleEventManager.battle_ended += OnBattleEnd;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Check_battle_scene_load;
        BattleEventManager.battle_ended -= OnBattleEnd;
    }

    //Battle씬 시작시 전투 시작되도록 하기
    private void Check_battle_scene_load(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Battle") 
        { 
            battle_start_trigger = true;
        }
        
    }

    // 전투 끝나는 거 감지
    private void OnBattleEnd(bool victory) 
    {
        // 패 전부 숨기기
        CardManager.instance.Change_active_hand(-1);
        BattleEventManager.enemy_skill_card_deactivate?.Invoke();

        // 진행 중인 전투 중지
        if (current_phase_coroutine != null) 
        {
            StopCoroutine(current_phase_coroutine);
        }

        // 전투 결과 띄움
        battle_result_canvas.SetActive(true);
        Battle_result_canvas canvas_Script = battle_result_canvas.GetComponent<Battle_result_canvas>();
        canvas_Script.Set_result(victory);

        // 전투 보상 계산 및 띄움
        if (victory) 
        {
            StartCoroutine(StageManager.instance.calc_and_show_battle_loot());
        }
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
}
