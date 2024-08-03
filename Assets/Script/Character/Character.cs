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
    // �÷��̾�� ĳ����
    kimchunsik,
    test,
    fire_mage,

    // ��
    test_enemy
}

public abstract class Character : MonoBehaviour
{
    /* 
    * �ʵ� �� ���ٿ� �޼ҵ��
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
                print("���� : ĳ���� ������ 0 ���ϰ� �� �� �����ϴ�");
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
    private int character_index; // ���� �߿����� ĳ���� ��ȣ
    public int Character_index
    { 
        get { return character_index; }
        set 
        { 
            if (value < 0) { print("���� : ĳ���� �ε����� ���� ������ �߻��߽��ϴ�"); } 
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
                print("���� : ĳ���� ��ǥ�� ���� ������ �߻��߽��ϴ�");
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
     * �޼ҵ�
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

    public void Damage_health(int value) // ü�� ����� �ִ� �޼ҵ�
    {
        // ���� ����
        if (value < 0) 
        {
            print("���� : ������� ������ �� �� �����ϴ�");
            return;
        }

        current_health -= value;

        if (current_health <= 0) // ���
        {
            current_health = 0;
            Kill();
        }

        health_damaged?.Invoke();
        health_changed?.Invoke(current_health);
    }

    public void Heal_health(int value)
    {
        // ���� ����
        if (value < 0)
        {
            print("���� : ȸ������ ������ �� �� �����ϴ�");
            return;
        }

        current_health += value;

        // �ִ� ü�� �ѱ� ����
        if (current_health > get_max_health())
        {
            current_health = get_max_health();
        }

        health_healed?.Invoke();
        health_changed?.Invoke(current_health);
 
    }

    public void Damage_willpower(int value) // ���ŷ� ����� �ִ� �޼ҵ�
    {
        // ���� ����
        if (value < 0)
        {
            print("���� : ������� ������ �� �� �����ϴ�");
            return;
        }

        current_willpower -= value;

        if (current_willpower <= 0) // �д�
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
        // ���� ����
        if (value < 0)
        {
            print("���� : ȸ������ ������ �� �� �����ϴ�");
            return;
        }

        current_willpower += value;

        // �ִ� ���ŷ� �ѱ� ����
        if (current_willpower > get_max_willpower())
        {
            current_willpower = get_max_willpower();
        }

        // ���ŷ¹� ������Ʈ
        willpower_healed?.Invoke();
        willpower_changed?.Invoke(current_willpower);
    }

    // ���� / ����� �߰��ϴ� �޼ҵ�
    public void give_effect(character_effect_code code, character_effect_setType type, int power)
    {
        got_effect?.Invoke(code, type, power);
    }

    // ���� / ����� (�����̳�) ���ִ� �޼ҵ�
    public void remove_effect(character_effect_code code)
    {
        destroy_effect?.Invoke(code);
    }

    // �� ���۽� �ߵ��Ǵ� �޼ҵ�
    protected virtual void turn_start()
    {
        // �д� ���� or �д� �� ����
        if (isPanic)
        {

            if (remaining_panic_turn <= 0)
            {
                isPanic = false;
                Heal_willpower((get_max_willpower() + 1) / 2); // ���ŷ� ȸ��
                out_of_panic?.Invoke();
            }
            else
            {
                remaining_panic_turn -= 1;
            }

        }


    }

    // ī�� ���� Ÿ�� ����, Ÿ�� ������ DetectingRay�� ����
    private void OnMouseExit()
    {
        BattleCalcManager.instance.clear_target_character();
    }

    // �̵� ������ ĭ ����Ʈ ��ȯ�ϴ� �޼ҵ�
    public List<coordinate> get_movable_tiles()
    {
        List<coordinate> result = new List<coordinate>();

        foreach (coordinate coord in move_range)
        {

            int absolute_x = coordinate.x + coord.x;
            int absolute_y = coordinate.y + coord.y;
            coordinate absolute_coordinate = new coordinate { x = absolute_x, y = absolute_y };
            // ��ȿ�� ĭ���� �˻�
            
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
     * �ʵ� �� ���ٿ� �޼ҵ��
     */

    private bool is_character_unlocked;
    public bool Is_character_unlocked { get { return is_character_unlocked; } }

    /* 
    * �޼ҵ�
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
        // �̵� ������ ĭ���� ���� ������ ǥ��
        foreach (coordinate coordinate in current_movable_tiles)
        {
            BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.original);
        }

        // ���� ĭ�� ���� ������ ǥ��
        BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.original);

        // ���� ����� �� ĭ ��ǥ�� ã�� (���� ĭ ����)
        coordinate nearest_tile = BattleGridManager.instance.get_nearest_tile(gameObject.transform.position, moveFilter, current_movable_tiles);

        // �� ĭ�� ĳ���� ĭ����
        BattleGridManager.instance.set_tile_type(nearest_tile, BattleGridManager.boardCell.player);

        // ���� ĭ ���� (���� ĭ�� �ٸ��� �̵� �Ұ� ���·�)
        if (coordinate != nearest_tile)
        {
            isMovable = false;
        }
        coordinate = nearest_tile;

        ActionManager.character_drag_ended?.Invoke();
    }

    // �巡�� ����
    private IEnumerator detect_drag_start()
    {
        float dragging_time = 0;

        if (isPanic) { yield break; } // �д��̸� ����
        if (!isMovable) { yield break; } // �̵� �Ұ��� ����

        while (true)
        {
            // ���콺 ���� ���� (�������� �� ���� �� �־ getmousebutton���� ��)
            if (!Input.GetMouseButton(0))
            {
                yield break;
            }

            // ���콺�� �� �� ���·� ���� �ð��� ������ �巡�� ��� ����
            if (dragging_time >= Util.drag_time_standard)
            {
                // �̵� ������ ĭ ����
                current_movable_tiles = get_movable_tiles();
                current_movable_tiles.Add(coordinate);

                // �̵� ������ ĭ���� �ʷϻ����� ǥ��
                foreach (coordinate coordinate in current_movable_tiles)
                {
                    BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.green);
                }


                // ���� ĭ�� �� ĭ���� ����� �ʷϻ�����
                BattleGridManager.instance.set_tile_type(coordinate, BattleGridManager.boardCell.empty);
                BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.green);
                ActionManager.character_drag_started?.Invoke();


                // �巡�� ����
                StartCoroutine(Ondrag());

                yield break;
            }

            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);

        }
    }

    // �巡�� ���� ����
    private IEnumerator Ondrag()
    {
        while (true) 
        {
            // ���콺 �����Ϳ� ���� ����� Ÿ�Ϸ� �̵� (�̵��� �� �ִ� ĭ�� �߿���)
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            coordinate nearest_tile = BattleGridManager.instance.get_nearest_tile(mousePos, moveFilter, current_movable_tiles);
            var tileCoor = BattleGridManager.instance.get_tile_pos(nearest_tile);
            transform.position = new Vector3(tileCoor[0], tileCoor[1], transform.position.z);

            // �巡�� ���� ����
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
     * �ʵ� �� ���ٿ� �޼ҵ��
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
    * �޼ҵ�
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