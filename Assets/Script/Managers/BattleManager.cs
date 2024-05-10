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


    //전투 중인 캐릭터의 게임오브젝트 리스트
    public List<GameObject> playable_characters = new List<GameObject>();
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
