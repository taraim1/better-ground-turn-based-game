using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using UnityEditor.Experimental.GraphView;

public enum character_code
{
    not_a_playable_character,
    kimchunsik,
    test,
    fire_mage
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
    public character_code Code { get { return code; } }
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
    public int get_max_health(int level)
    {
        if (level <= 0 || level >= max_healthes_of_level.Count)
        {
            print("���� : �ִ� ü�� ����Ʈ �ε��� ������ �߻��߽��ϴ�");
            return -1;
        }
        return max_healthes_of_level[level];
    }
    private List<int> max_willpowers_of_level;
    public int get_max_willpower(int level)
    {
        if (level <= 0 || level >= max_willpowers_of_level.Count)
        {
            print("���� : �ִ� ���ŷ� ����Ʈ �ε��� ������ �߻��߽��ϴ�");
            return -1;
        }
        return max_willpowers_of_level[level];
    }
    private List<skillcard_code> deck;
    private string SPUM_datapath;
    public string SPUM_Datapath { get { return SPUM_datapath; } }
    private List<BasicCharacterData.coordinate> move_range;
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
    protected Tuple<int, int> coordinate;
    public Tuple<int, int> Coordinate 
    { 
        get { return coordinate; } 
        set 
        {
            if (value.Item1 < 0 || value.Item2 < 0) 
            {
                print("���� : ĳ���� ��ǥ�� ���� ������ �߻��߽��ϴ�");
            }
            coordinate = Tuple.Create(value.Item1, value.Item2);
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
    protected List<Tuple<int, int>> current_movable_tiles;

    protected CharacterDataSO DataSO;

    public Action<int> health_changed;
    public Action<int> willpower_changed;
    public Action<skillcard_code> skill_card_used;
    public Action panicked;
    public Action out_of_panic;
    public Action health_damaged;
    public Action health_healed;
    public Action willpower_damaged;
    public Action willpower_healed;
    public Action<character_effect_code, character_effect_setType, int> got_effect;
    public Action<character_effect_code> destroy_effect;


    /* 
     * ������ �� �޼ҵ�
     */


    public Character(character_code code, CharacterDataSO DataSO) 
    { 
        this.code = code;
        this.DataSO = DataSO;
        Load_data();
    }

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
    }

    public virtual void Kill() 
    {
        ActionManager.character_going_to_die?.Invoke(this);
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
        if (current_health > get_max_health(level))
        {
            current_health = get_max_health(level);
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
        if (current_willpower > get_max_willpower(level))
        {
            current_willpower = get_max_willpower(level);
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
                Heal_willpower((get_max_willpower(level) + 1) / 2); // ���ŷ� ȸ��
                out_of_panic?.Invoke();
            }
            else
            {
                remaining_panic_turn -= 1;
            }

        }


    }

    // ��ǥ �����ϴ� �޼ҵ�
    public void set_coordinate(int x, int y)
    {
        coordinate = Tuple.Create(x, y);
    }

    // ��ǥ �˾Ƴ��� �޼ҵ�
    public Tuple<int, int> get_coordinate()
    {
        return Tuple.Create(coordinate.Item1, coordinate.Item2);
    }

    // ī�� ���� Ÿ�� ����, Ÿ�� ������ DetectingRay�� ����
    private void OnMouseExit()
    {
        BattleCalcManager.instance.clear_target_character();
    }

    public virtual bool check_enemy() { return false; }

    private void Awake()
    {
        ActionManager.turn_start_phase += turn_start;
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
    * ������ �� �޼ҵ�
    */

    public PlayableCharacter(character_code code, CharacterDataSO DataSO) : base(code, DataSO)
    {
        Load_data();
    }

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

    public void start_drag_detecting()
    {
        StartCoroutine(detect_drag_start());
    }

    private void On_drag_end() 
    {
        // �̵� ������ ĭ���� ���� ������ ǥ��
        foreach (Tuple<int, int> coordinate in current_movable_tiles)
        {
            BattleGridManager.instance.set_tile_color(coordinate.Item1, coordinate.Item2, Tile.TileColor.original);
        }

        // ���� ĭ�� ���� ������ ǥ��
        BattleGridManager.instance.set_tile_color(coordinate.Item1, coordinate.Item2, Tile.TileColor.original);

        // ���� ����� �� ĭ ��ǥ�� ã�� (���� ĭ ����)
        Tuple<int, int> nearest_tile = BattleGridManager.instance.get_nearest_tile(gameObject.transform.position, moveFilter, current_movable_tiles);

        // �� ĭ�� ĳ���� ĭ����
        BattleGridManager.instance.set_tile_type(nearest_tile.Item1, nearest_tile.Item2, BattleGridManager.boardCell.player);

        // ���� ĭ ���� (���� ĭ�� �ٸ��� �̵� �Ұ� ���·�)
        if (coordinate.Item1 != nearest_tile.Item1 || coordinate.Item2 != nearest_tile.Item2)
        {
            isMovable = false;
        }
        coordinate = Tuple.Create(nearest_tile.Item1, nearest_tile.Item2);

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
                current_movable_tiles.Add(get_coordinate());

                // �̵� ������ ĭ���� �ʷϻ����� ǥ��
                foreach (Tuple<int, int> coordinate in current_movable_tiles)
                {
                    BattleGridManager.instance.set_tile_color(coordinate.Item1, coordinate.Item2, Tile.TileColor.green);
                }


                // ���� ĭ�� �� ĭ���� ����� �ʷϻ�����
                BattleGridManager.instance.set_tile_type(coordinate.Item1, coordinate.Item2, BattleGridManager.boardCell.empty);
                BattleGridManager.instance.set_tile_color(coordinate.Item1, coordinate.Item2, Tile.TileColor.green);
                ActionManager.character_drag_started?.Invoke();

                yield break;
            }

            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);

        }
    }

    // �巡�� ���� ����
    private IEnumerator detect_drag_end()
    {
        // �巡�� �߿� ���콺 ���� (�������� �� ���� �� �־ getmousebutton���� ��)
        if (!Input.GetMouseButton(0))
        {
            On_drag_end();
            yield break;
        }
    }
}

[System.Serializable]
public class EnemyCharacter : Character 
{
    /* 
     * �ʵ� �� ���ٿ� �޼ҵ��
     */

    public Action<skillcard_code> skill_reserved;

    /* 
    * ������ �� �޼ҵ�
    */

    public EnemyCharacter(character_code code, CharacterDataSO DataSO) : base(code, DataSO)
    {
        Load_data();
    }
}