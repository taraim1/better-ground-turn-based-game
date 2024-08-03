using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using UnityEditor.Experimental.GraphView;
using JetBrains.Annotations;

public enum character_code
{
    // 플레이어블 캐릭터
    kimchunsik,
    test,
    fire_mage,

    // 적
    test_enemy
}

public abstract class Character : MonoBehaviour
{
    /* 
    * 필드 및 접근용 메소드들
    */

    private string character_name;
    public string Character_name { get { return character_name; } }
    private string description;
    public string Description { get { return description; } }
    protected character_code code;
    public character_code Code { get { return code; } set { code = value; } }
    private int level;
    public int Level
    {
        get { return level; }
        set
        {
            if (value <= 0)
            {
                print("오류 : 캐릭터 레벨은 0 이하가 될 수 없습니다");
                value = 1;
            }
            level = value;
        }
    }
    private List<int> max_healthes_of_level;
    public int get_max_health()
    {
        return max_healthes_of_level[level];
    }
    private List<int> max_willpowers_of_level;
    public int get_max_willpower()
    {
        return max_willpowers_of_level[level];
    }
    private List<skillcard_code> deck;
    public List<skillcard_code> Get_deck_copy() 
    { 
        List<skillcard_code> deck_copy = new List<skillcard_code>();
        foreach (skillcard_code code in deck) { deck_copy.Add(code); }
        return deck_copy;
    }
    private string SPUM_datapath;
    public string SPUM_Datapath { get { return SPUM_datapath; } }
    private List<coordinate> move_range;
    private int current_health;
    public int Current_health { get { return current_health; } }
    private int current_willpower;
    public int Current_willpower { get { return current_willpower; } }
    private int character_index; // 전투 중에서의 캐릭터 번호
    public int Character_index
    { 
        get { return character_index; }
        set 
        { 
            if (value < 0) { print("오류 : 캐릭터 인덱스에 음수 대입이 발생했습니다"); } 
            character_index = value; 
        }
    }
    protected bool isPanic;
    public bool IsPanic {  get { return isPanic; } }
    private int remaining_panic_turn;
    protected coordinate coordinate;
    public coordinate Coordinate 
    { 
        get { return coordinate; } 
        set 
        {
            if (value.x < 0 || value.y < 0) 
            {
                print("오류 : 캐릭터 좌표에 음수 대입이 발생했습니다");
            }
            coordinate = value;
        } 
    }
    protected List<BattleGridManager.boardCell> moveFilter = new List<BattleGridManager.boardCell> 
    { 
        BattleGridManager.boardCell.enemy, 
        BattleGridManager.boardCell.player, 
        BattleGridManager.boardCell.obstacle 
    };
    protected bool isMovable;
    public bool IsMovable { get { return isMovable; } }
    protected List<coordinate> current_movable_tiles;

    protected CharacterDataSO DataSO;
    public CharacterDataSO Data_SO { set { DataSO = value; } }
    public Action<int> health_changed;
    public Action<int> willpower_changed;
    public Action<skillcard_code> skillData_used;
    public Action panicked;
    public Action out_of_panic;
    public Action health_damaged;
    public Action health_healed;
    public Action willpower_damaged;
    public Action willpower_healed;
    public Action<int> show_power_meter;
    public Action<character_effect_code, character_effect_setType, int> got_effect;
    public Action<character_effect_code> destroy_effect;


    /* 
     * 메소드
     */

    public virtual void Save_data() 
    {
        BasicCharacterData data = DataSO.BasicData[code];
        data.character_name = character_name;
        data.description = description;
        data.level = level;
        data.deck = deck;
        data.SPUM_datapath = SPUM_datapath;
    }

    public virtual void Load_data()
    {
        BasicCharacterData data = DataSO.BasicData[code];
        character_name = data.character_name;
        description = data.description;
        level = data.level;
        max_healthes_of_level = data.max_healthes_of_level;
        max_willpowers_of_level = data.max_willpowers_of_level;
        deck = data.deck;
        SPUM_datapath = data.SPUM_datapath;
        move_range = data.move_range;

        current_health = get_max_health();
        current_willpower = get_max_willpower();
    }

    public virtual void Kill() 
    {
        ActionManager.character_died?.Invoke(this);
        Destroy(gameObject);
    }

    public void Damage_health(int value) // 체력 대미지 주는 메소드
    {
        // 음수 방지
        if (value < 0) 
        {
            print("오류 : 대미지는 음수가 될 수 없습니다");
            return;
        }

        current_health -= value;

        if (current_health <= 0) // 사망
        {
            current_health = 0;
            Kill();
        }

        health_damaged?.Invoke();
        health_changed?.Invoke(current_health);
    }

    public void Heal_health(int value)
    {
        // 음수 방지
        if (value < 0)
        {
            print("오류 : 회복량은 음수가 될 수 없습니다");
            return;
        }

        current_health += value;

        // 최대 체력 넘기 방지
        if (current_health > get_max_health())
        {
            current_health = get_max_health();
        }

        health_healed?.Invoke();
        health_changed?.Invoke(current_health);
 
    }

    public void Damage_willpower(int value) // 정신력 대미지 주는 메소드
    {
        // 음수 방지
        if (value < 0)
        {
            print("오류 : 대미지는 음수가 될 수 없습니다");
            return;
        }

        current_willpower -= value;

        if (current_willpower <= 0) // 패닉
        {
            current_willpower = 0;
            isPanic = true;
            remaining_panic_turn = 1;

            panicked?.Invoke();
        }

        willpower_damaged?.Invoke();
        willpower_changed?.Invoke(current_willpower);

    }

    public void Heal_willpower(int value)
    {
        // 음수 방지
        if (value < 0)
        {
            print("오류 : 회복량은 음수가 될 수 없습니다");
            return;
        }

        current_willpower += value;

        // 최대 정신력 넘기 방지
        if (current_willpower > get_max_willpower())
        {
            current_willpower = get_max_willpower();
        }

        // 정신력바 업데이트
        willpower_healed?.Invoke();
        willpower_changed?.Invoke(current_willpower);
    }

    // 버프 / 디버프 추가하는 메소드
    public void give_effect(character_effect_code code, character_effect_setType type, int power)
    {
        got_effect?.Invoke(code, type, power);
    }

    // 버프 / 디버프 (컨테이너) 없애는 메소드
    public void remove_effect(character_effect_code code)
    {
        destroy_effect?.Invoke(code);
    }

    // 턴 시작시 발동되는 메소드
    protected virtual void turn_start()
    {
        // 패닉 해제 or 패닉 턴 감소
        if (isPanic)
        {

            if (remaining_panic_turn <= 0)
            {
                isPanic = false;
                Heal_willpower((get_max_willpower() + 1) / 2); // 정신력 회복
                out_of_panic?.Invoke();
            }
            else
            {
                remaining_panic_turn -= 1;
            }

        }


    }

    // 카드 사용시 타깃 해제, 타깃 설정은 DetectingRay에 있음
    private void OnMouseExit()
    {
        BattleCalcManager.instance.clear_target_character();
    }

    // 이동 가능한 칸 리스트 반환하는 메소드
    public List<coordinate> get_movable_tiles()
    {
        List<coordinate> result = new List<coordinate>();

        foreach (coordinate coord in move_range)
        {

            int absolute_x = coordinate.x + coord.x;
            int absolute_y = coordinate.y + coord.y;
            coordinate absolute_coordinate = new coordinate { x = absolute_x, y = absolute_y };
            // 유효한 칸인지 검사
            
            BattleGridManager.boardCell type = BattleGridManager.instance.get_tile(absolute_coordinate);
            if (type == BattleGridManager.boardCell.empty)
            {
                result.Add(new coordinate(absolute_x, absolute_y));
            }

        }

        return result;
    }

    public virtual bool check_enemy() { return false; }

    private void Awake()
    { 
        ActionManager.turn_start_phase += turn_start;
    }

    private void OnDisable()
    {
        ActionManager.turn_start_phase -= turn_start;
    }
}



[System.Serializable]
public class PlayableCharacter : Character
{
    /* 
     * 필드 및 접근용 메소드들
     */

    private bool is_character_unlocked;
    public bool Is_character_unlocked { get { return is_character_unlocked; } }

    /* 
    * 메소드
    */


    public override void Save_data()
    {
        base.Save_data();
        PlayableCharacterData playableData = DataSO.PlayerData[code];
        playableData.is_character_unlocked = is_character_unlocked;
    }

    public override void Load_data()
    {
        base.Load_data();
        PlayableCharacterData playableData = DataSO.PlayerData[code];
        is_character_unlocked = playableData.is_character_unlocked;
    }

    protected override void turn_start()
    {
        base.turn_start();
        isMovable = true;
    }

    public override bool check_enemy()
    {
        return false;
    }

    public void start_drag_detection()
    {
        StartCoroutine(detect_drag_start());
    }

    private void On_drag_end() 
    {
        // 이동 가능한 칸들을 원래 색으로 표시
        foreach (coordinate coordinate in current_movable_tiles)
        {
            BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.original);
        }

        // 현재 칸을 원래 색으로 표시
        BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.original);

        // 가장 가까운 빈 칸 좌표를 찾음 (원래 칸 포함)
        coordinate nearest_tile = BattleGridManager.instance.get_nearest_tile(gameObject.transform.position, moveFilter, current_movable_tiles);

        // 그 칸을 캐릭터 칸으로
        BattleGridManager.instance.set_tile_type(nearest_tile, BattleGridManager.boardCell.player);

        // 현재 칸 변경 (원래 칸과 다르면 이동 불가 상태로)
        if (coordinate != nearest_tile)
        {
            isMovable = false;
        }
        coordinate = nearest_tile;

        ActionManager.character_drag_ended?.Invoke();
    }

    // 드래그 감지
    private IEnumerator detect_drag_start()
    {
        float dragging_time = 0;

        if (isPanic) { yield break; } // 패닉이면 멈춤
        if (!isMovable) { yield break; } // 이동 불가면 멈춤

        while (true)
        {
            // 마우스 떼면 멈춤 (프레임이 안 맞을 수 있어서 getmousebutton으로 함)
            if (!Input.GetMouseButton(0))
            {
                yield break;
            }

            // 마우스를 안 뗀 상태로 일정 시간이 지나면 드래그 기능 시작
            if (dragging_time >= Util.drag_time_standard)
            {
                // 이동 가능한 칸 갱신
                current_movable_tiles = get_movable_tiles();
                current_movable_tiles.Add(coordinate);

                // 이동 가능한 칸들을 초록색으로 표시
                foreach (coordinate coordinate in current_movable_tiles)
                {
                    BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.green);
                }


                // 현재 칸을 빈 칸으로 만들고 초록색으로
                BattleGridManager.instance.set_tile_type(coordinate, BattleGridManager.boardCell.empty);
                BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.green);
                ActionManager.character_drag_started?.Invoke();


                // 드래그 시작
                StartCoroutine(Ondrag());

                yield break;
            }

            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);

        }
    }

    // 드래그 해제 감지
    private IEnumerator Ondrag()
    {
        while (true) 
        {
            // 마우스 포인터에 가장 가까운 타일로 이동 (이동할 수 있는 칸들 중에서)
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            coordinate nearest_tile = BattleGridManager.instance.get_nearest_tile(mousePos, moveFilter, current_movable_tiles);
            var tileCoor = BattleGridManager.instance.get_tile_pos(nearest_tile);
            transform.position = new Vector3(tileCoor[0], tileCoor[1], transform.position.z);

            // 드래그 해제 감지
            if (!Input.GetMouseButton(0))
            {
                On_drag_end();
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}

[System.Serializable]
public class EnemyCharacter : Character 
{
    /* 
     * 필드 및 접근용 메소드들
     */
    private int remaining_skill_count = 0;
    public int Remaining_skill_count { get { return remaining_skill_count; } }

    private List<skillcard_code> reserved_skills;

    public Action<skillcard_code> skill_reserved;

    private EnemyAI AI;
    public void SetAI(EnemyAI AI) 
    {
        this.AI = AI;
    }

    /* 
    * 메소드
    */

    public override bool check_enemy() { return true; }

    private void OnSkillReserved(skillcard_code code) 
    {
        remaining_skill_count += 1;
    }

    private void OnSkillUsed(Character character, skillcard_code code) 
    {
        if (character == this && remaining_skill_count >= 0) 
        {
            remaining_skill_count -= 1;
        }
    }

    private void OnEnemySkillSettingPhase() 
    {
        AI.Move();
        reserved_skills = AI.Get_skills_for_current_turn();
        ActionManager.enemy_skill_set_complete?.Invoke();
    }

    private void Awake()
    {
        skill_reserved += OnSkillReserved;
        ActionManager.skill_used += OnSkillUsed;
        ActionManager.enemy_skill_setting_phase += OnEnemySkillSettingPhase;
    }

    private void OnDestroy()
    {
        skill_reserved -= OnSkillReserved;
        ActionManager.skill_used -= OnSkillUsed;
        ActionManager.enemy_skill_setting_phase -= OnEnemySkillSettingPhase;
    
    }
}   