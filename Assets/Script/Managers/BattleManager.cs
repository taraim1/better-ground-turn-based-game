using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class BattleManager : Singletone<BattleManager> //싱글톤임
{
    
    public int current_turn = 0; // 현재 턴
    public int current_phase = 0; // 현재 페이즈
    public bool battle_start_trigger = false; // 전투를 시작시키는 트리거
    public bool is_Characters_spawned = false; // 전투 시작시 플레이이어와 적 캐릭터가 스폰되었는가?
    public bool is_Characters_loaded = false; // 전투 시작시 캐릭터 데이터가 불러와졌는가?

    public Vector3[] playable_character_position_settings = new Vector3[4]; //플레이어블 캐릭터 스폰 위치

    //전투 중인 캐릭터의 데이터 인스턴스 리스트
    public List<Playable_Character> playable_Characters_data = new List<Playable_Character>();

    //전투 중인 캐릭터의 게임오브젝트 리스트
    public List<GameObject> playable_characters = new List<GameObject>();
    public enum phases // 한 턴의 페이즈 모음
    { 
        turn_start_effect_phase,
        enemy_skill_setting_phase,
        player_skill_setting_phase,
        fight_phase,
        turn_end_effect_phase
    }

    IEnumerator battle() //전투 코루틴
    {
        //변수 초기화
        is_Characters_spawned = false;
        playable_Characters_data.Clear();
        playable_characters.Clear();

        //파티 데이터 불러오기
        PartyManager.instance.read_Json_file();

        //전투 중인 캐릭터의 Playable Character 인스턴스 리스트를 불러오기 (정보저장용)
        for (int i = 0; i < PartyManager.party_member_count; i++) 
        { 
            playable_Characters_data.Add(PartyManager.instance.get_character_of_party(i));
        }
 
        //캐릭터 오브젝트 생성
        CharacterManager.instance.spawn_character();

        //캐릭터 오브젝트 이름 설정
        for (int i = 0; i < PartyManager.party_member_count; i++) 
        {
            playable_characters[i].name = playable_Characters_data[i].get_character_name();
        }

        //체력바 리스트 초기화
        BattleUI_Manager.instance.clear_health_bar_list();

        //체력바 소환
        for (int i = 0; i < PartyManager.party_member_count; i++) 
        {
            BattleUI_Manager.instance.summon_health_bar(playable_characters[i]);
        }

        //체력바 초기값 설정
        for (int i = 0; i < PartyManager.party_member_count; i++)
        {
            int max_health = playable_Characters_data[i].get_character_int_property(CharacterManager.character_int_properties.max_health);
            BattleUI_Manager.instance.set_UI_slider_property_of_UIelement(BattleUI_Manager.UI_bars.health_bar, i, BattleUI_Manager.UI_bars_properties.max_value, max_health);
            BattleUI_Manager.instance.set_UI_slider_property_of_UIelement(BattleUI_Manager.UI_bars.health_bar, i, BattleUI_Manager.UI_bars_properties.current_value, max_health);
        }


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
