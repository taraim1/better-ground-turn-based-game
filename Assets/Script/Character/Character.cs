using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;


[System.Serializable]
public class Character : MonoBehaviour
{
    /*
        캐릭터 데이터가 저장되는 클래스
        캐릭터 데이터를 json 파일로 저장하는 역할도 함
    */

    public string character_name;
    public string description;
    public CharacterManager.character_code code;
    public CharacterManager.enemy_code enemy_code;
    public int level;
    [SerializeField]
    private List<int> max_health = new List<int>() { 0, 30 };
    [SerializeField]
    private List<int> max_willpower = new List<int>() { 0, 15 };
    public int current_health;
    public int current_willpower;
    public bool is_character_unlocked;
    [SerializeField]
    public skillcard_code[] deck = new skillcard_code[6];
    public string SPUM_datapath;
    [SerializeField] private List<int> move_range_x;
    [SerializeField] private List<int> move_range_y;


    public class CharacterData_NOT_use_JSON
    {
        // 아래는 JSON 저장하면 안 되는 것들
        public int current_health;
        public int current_willpower;
        public GameObject health_bar;
        public GameObject willpower_bar;
        public UI_bar_slider health_slider;
        public UI_bar_slider willpower_slider;
        public skill_power_meter skill_power_meter;
        public bool isEnemyCharacter;
        public Coroutine running_drag = null;
        // 전투시 캐릭터 오브젝트의 번호
        public int Character_index;
        public panic_sign panic_Sign;
        public GameObject skill_layoutGroup;
        public bool isPanic;
        public int remaining_panic_turn;
        public GameObject SPUM_unit_obj; // 캐릭터 spum 오브젝트
        public bool is_in_battle;
        public List<character_effect_container> effect_Containers = new List<character_effect_container>(); // 캐릭터가 현재 가지고 있는 효과들 컨테이너 (버프 / 디버프)
        public bool isDragging = false;
        public GameObject effect_container_prefab;
        public GameObject effects_layoutGroup_obj;
        public Tuple<int, int> _coordinate = Tuple.Create(0, 0);
        public List<BattleGridManager.boardCell> _moveFilter = new List<BattleGridManager.boardCell> { BattleGridManager.boardCell.enemy, BattleGridManager.boardCell.player, BattleGridManager.boardCell.obstacle };
    }

    public CharacterData_NOT_use_JSON data = new CharacterData_NOT_use_JSON();

    

    public int get_max_health_of_level(int level)
    {
        if (level > max_health.Count)
        {
            Debug.Log("오류: 입력된 레벨의 체력 데이터가 없습니다.");
            return -1;
        }
        else
        {
            return max_health[level];
        }
    }

    public int get_max_willpower_of_level(int level)
    {
        if (level > max_willpower.Count)
        {
            Debug.Log("오류: 입력된 레벨의 정신력 데이터가 없습니다.");
            return -1;
        }
        else
        {
            return max_willpower[level];
        }
    }

    public void Set_UI_bars() // 캐릭터의 체력바, 정신력바 초기세팅
    {
        data.health_slider = data.health_bar.GetComponent<UI_bar_slider>();
        data.willpower_slider = data.willpower_bar.GetComponent<UI_bar_slider>();
        data.health_slider.value_tmp.text = max_health[level].ToString();
        data.willpower_slider.value_tmp.text = max_willpower[level].ToString();
        data.health_slider.slider.maxValue = max_health[level];
        data.willpower_slider.slider.maxValue = max_willpower[level];
        data.health_slider.slider.value = data.health_slider.slider.maxValue;
        data.willpower_slider.slider.value = data.willpower_slider.slider.maxValue;
    }

    public void Damage_health(int value) // 체력 대미지 주는 메소드
    {
        data.current_health -= value;

        if (data.current_health <= 0) // 사망
        {
            CharacterManager.instance.kill_character(this);
        }

        // 체력바 업데이트
        data.health_slider.slider.value = data.current_health;
        data.health_slider.value_tmp.text = data.current_health.ToString();

    }

    public void Heal_health(int value) 
    {
        data.current_health += value;
        if (data.current_health > get_max_health_of_level(level)) 
        {
            data.current_health = get_max_health_of_level(level);
        }
        // 체력바 업데이트
        data.health_slider.slider.value = data.current_health;
        data.health_slider.value_tmp.text = data.current_health.ToString();
    }

    public void Damage_willpower(int value) // 정신력 대미지 주는 메소드
    {

        data.current_willpower -= value;

        if (data.current_willpower <= 0) // 패닉
        {
            data.current_willpower = 0;
            data.isPanic = true;
            data.panic_Sign.show();
            data.remaining_panic_turn = 1;

            if (data.isEnemyCharacter) // 적이면 쓰는 스킬 다 제거
            {
                gameObject.GetComponent<EnemyAI>().clear_skills();
            }
        }

        // 정신력바 업데이트
        data.willpower_slider.slider.value = data.current_willpower;
        data.willpower_slider.value_tmp.text = data.current_willpower.ToString();
    }

    public void Heal_willpower(int value)
    {
        data.current_willpower += value;
        if (data.current_willpower > get_max_willpower_of_level(level))
        {
            data.current_willpower = get_max_willpower_of_level(level);
        }
        // 정신력바 업데이트
        data.willpower_slider.slider.value = data.current_willpower;
        data.willpower_slider.value_tmp.text = data.current_willpower.ToString();
    }

    // 카드 사용시 타깃 해제, 타깃 설정은 DetectingRay에 있음
    private void OnMouseExit()
    {
        if (data.is_in_battle)
        {
            BattleCalcManager.instance.clear_target_character();
        }
    }

    // 버프/디버프 추가하는 메소드
    public void give_effect(character_effect_code code, character_effect_setType type, int power) 
    {
        // 이미 가지고 있는 건지 확인
        foreach (character_effect_container container in data.effect_Containers) 
        {
            if (container.Get_effect_code() == code) 
            {
                container.updateEffect(power, type); // 위력 갱신 or 추가
                return;
            }
        }

        // 없는거면 새로 추가
        GameObject obj = Instantiate(data.effect_container_prefab, data.effects_layoutGroup_obj.transform);
        character_effect_container obj_container = obj.GetComponent<character_effect_container>();
        data.effect_Containers.Add(obj_container);
        obj_container.Set(buffNdebuffManager.instance.get_effect(code, power), this);

        // 이펙트 레이아웃그룹 업데이트
        data.effects_layoutGroup_obj.GetComponent<effectsLayoutGroup>().set_size(data.effect_Containers.Count);
    }

    // 버프 / 디버프 (컨테이너) 없애는 메소드
    public void remove_effect(character_effect_container target) 
    {
        if (target != null)
        {
            data.effect_Containers.Remove(target);
            target.clear_delegate_and_destroy();
        }

        // 이펙트 레이아웃그룹 업데이트
        data.effects_layoutGroup_obj.GetComponent<effectsLayoutGroup>().set_size(data.effect_Containers.Count);
    }



    // 턴 시작시 발동되는 메소드
    private void turn_start() 
    {
        // 패닉 해제 or 패닉 턴 감소
        if (data.isPanic)
        {

            if (data.remaining_panic_turn == 0)
            {
                data.isPanic = false;
                data.panic_Sign.hide();
                Damage_willpower(-((get_max_willpower_of_level(level) + 1) / 2)); // 정신력 회복
            }
            else 
            {
                data.remaining_panic_turn -= 1;
            }

        }

        
    }

    // 드래그 감지
    public IEnumerator detect_drag()
    {
        float dragging_time = 0;

        while (true)
        {
            

            // 마우스 뗴면
            if (Input.GetMouseButton(0) == false)
            {
                // 드래그 중이면
                if (data.isDragging) 
                {
                    // 가장 가까운 빈 칸 좌표를 찾음 (원래 칸 포함)
                    Tuple<int, int> nearest_tile = BattleGridManager.instance.get_nearest_tile(gameObject.transform.position, data._moveFilter);

                    // 그 칸을 캐릭터 칸으로
                    BattleGridManager.instance.set_tile(nearest_tile.Item1, nearest_tile.Item2, BattleGridManager.boardCell.player);

                    // 현재 칸 변경
                    data._coordinate = Tuple.Create(nearest_tile.Item1, nearest_tile.Item2);

                    data.isDragging = false;
                    ActionManager.character_drag_ended?.Invoke();
                }
                yield break;
            }

            // 마우스를 안 뗀 상태로 일정 시간이 지나면 드래그 기능 시작 (패닉이 아니어야 함)
            if (dragging_time >= Util.drag_time_standard && !data.isPanic && !data.isDragging)
            {
                // 현재 칸을 빈 칸으로
                BattleGridManager.instance.set_tile(data._coordinate.Item1, data._coordinate.Item2, BattleGridManager.boardCell.empty);
                data.isDragging = true;
                ActionManager.character_drag_started?.Invoke();

            }

            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);

        }
    }

    // 좌표 설정하는 메소드
    public void set_coordinate(int x, int y) 
    {
        data._coordinate = Tuple.Create(x, y);
    }

    // 좌표 알아내는 메소드
    public Tuple<int, int> get_coordinate() 
    {
        return data._coordinate;
    }

    private void Update()
    {
        // 배틀에서 드래그중인 경우 마우스 포인터에 가장 가까운 타일로 이동
        if (data.is_in_battle && data.isDragging) 
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Tuple<int, int> nearest_tile = BattleGridManager.instance.get_nearest_tile(mousePos, data._moveFilter);
            var tileCoor = BattleGridManager.instance.get_tile_pos(nearest_tile.Item1, nearest_tile.Item2);
            transform.position = new Vector3(tileCoor[0], tileCoor[1], transform.position.z);
        }
    }


    private void Awake()
    {
        data.effect_container_prefab = CharacterManager.instance.effect_container_prefab;
        ActionManager.turn_start_phase += turn_start;
    }
    private void OnDisable()
    {
        ActionManager.turn_start_phase -= turn_start;
    }
}
