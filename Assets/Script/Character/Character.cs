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
    public List<skillcard_code> Deck => deck;


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
    public Action<skillcard_code> skillcard_used;
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

