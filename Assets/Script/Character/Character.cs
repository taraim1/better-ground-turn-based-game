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
        ĳ���� �����Ͱ� ����Ǵ� Ŭ����
        ĳ���� �����͸� json ���Ϸ� �����ϴ� ���ҵ� ��
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
        // �Ʒ��� JSON �����ϸ� �� �Ǵ� �͵�
        public int current_health;
        public int current_willpower;
        public GameObject health_bar;
        public GameObject willpower_bar;
        public UI_bar_slider health_slider;
        public UI_bar_slider willpower_slider;
        public skill_power_meter skill_power_meter;
        public bool isEnemyCharacter;
        public Coroutine running_drag = null;
        // ������ ĳ���� ������Ʈ�� ��ȣ
        public int Character_index;
        public panic_sign panic_Sign;
        public GameObject skill_layoutGroup;
        public bool isPanic;
        public int remaining_panic_turn;
        public GameObject SPUM_unit_obj; // ĳ���� spum ������Ʈ
        public bool is_in_battle;
        public List<character_effect_container> effect_Containers = new List<character_effect_container>(); // ĳ���Ͱ� ���� ������ �ִ� ȿ���� �����̳� (���� / �����)
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
            Debug.Log("����: �Էµ� ������ ü�� �����Ͱ� �����ϴ�.");
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
            Debug.Log("����: �Էµ� ������ ���ŷ� �����Ͱ� �����ϴ�.");
            return -1;
        }
        else
        {
            return max_willpower[level];
        }
    }

    public void Set_UI_bars() // ĳ������ ü�¹�, ���ŷ¹� �ʱ⼼��
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

    public void Damage_health(int value) // ü�� ����� �ִ� �޼ҵ�
    {
        data.current_health -= value;

        if (data.current_health <= 0) // ���
        {
            CharacterManager.instance.kill_character(this);
        }

        // ü�¹� ������Ʈ
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
        // ü�¹� ������Ʈ
        data.health_slider.slider.value = data.current_health;
        data.health_slider.value_tmp.text = data.current_health.ToString();
    }

    public void Damage_willpower(int value) // ���ŷ� ����� �ִ� �޼ҵ�
    {

        data.current_willpower -= value;

        if (data.current_willpower <= 0) // �д�
        {
            data.current_willpower = 0;
            data.isPanic = true;
            data.panic_Sign.show();
            data.remaining_panic_turn = 1;

            if (data.isEnemyCharacter) // ���̸� ���� ��ų �� ����
            {
                gameObject.GetComponent<EnemyAI>().clear_skills();
            }
        }

        // ���ŷ¹� ������Ʈ
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
        // ���ŷ¹� ������Ʈ
        data.willpower_slider.slider.value = data.current_willpower;
        data.willpower_slider.value_tmp.text = data.current_willpower.ToString();
    }

    // ī�� ���� Ÿ�� ����, Ÿ�� ������ DetectingRay�� ����
    private void OnMouseExit()
    {
        if (data.is_in_battle)
        {
            BattleCalcManager.instance.clear_target_character();
        }
    }

    // ����/����� �߰��ϴ� �޼ҵ�
    public void give_effect(character_effect_code code, character_effect_setType type, int power) 
    {
        // �̹� ������ �ִ� ���� Ȯ��
        foreach (character_effect_container container in data.effect_Containers) 
        {
            if (container.Get_effect_code() == code) 
            {
                container.updateEffect(power, type); // ���� ���� or �߰�
                return;
            }
        }

        // ���°Ÿ� ���� �߰�
        GameObject obj = Instantiate(data.effect_container_prefab, data.effects_layoutGroup_obj.transform);
        character_effect_container obj_container = obj.GetComponent<character_effect_container>();
        data.effect_Containers.Add(obj_container);
        obj_container.Set(buffNdebuffManager.instance.get_effect(code, power), this);

        // ����Ʈ ���̾ƿ��׷� ������Ʈ
        data.effects_layoutGroup_obj.GetComponent<effectsLayoutGroup>().set_size(data.effect_Containers.Count);
    }

    // ���� / ����� (�����̳�) ���ִ� �޼ҵ�
    public void remove_effect(character_effect_container target) 
    {
        if (target != null)
        {
            data.effect_Containers.Remove(target);
            target.clear_delegate_and_destroy();
        }

        // ����Ʈ ���̾ƿ��׷� ������Ʈ
        data.effects_layoutGroup_obj.GetComponent<effectsLayoutGroup>().set_size(data.effect_Containers.Count);
    }



    // �� ���۽� �ߵ��Ǵ� �޼ҵ�
    private void turn_start() 
    {
        // �д� ���� or �д� �� ����
        if (data.isPanic)
        {

            if (data.remaining_panic_turn == 0)
            {
                data.isPanic = false;
                data.panic_Sign.hide();
                Damage_willpower(-((get_max_willpower_of_level(level) + 1) / 2)); // ���ŷ� ȸ��
            }
            else 
            {
                data.remaining_panic_turn -= 1;
            }

        }

        
    }

    // �巡�� ����
    public IEnumerator detect_drag()
    {
        float dragging_time = 0;

        while (true)
        {
            

            // ���콺 ���
            if (Input.GetMouseButton(0) == false)
            {
                // �巡�� ���̸�
                if (data.isDragging) 
                {
                    // ���� ����� �� ĭ ��ǥ�� ã�� (���� ĭ ����)
                    Tuple<int, int> nearest_tile = BattleGridManager.instance.get_nearest_tile(gameObject.transform.position, data._moveFilter);

                    // �� ĭ�� ĳ���� ĭ����
                    BattleGridManager.instance.set_tile(nearest_tile.Item1, nearest_tile.Item2, BattleGridManager.boardCell.player);

                    // ���� ĭ ����
                    data._coordinate = Tuple.Create(nearest_tile.Item1, nearest_tile.Item2);

                    data.isDragging = false;
                    ActionManager.character_drag_ended?.Invoke();
                }
                yield break;
            }

            // ���콺�� �� �� ���·� ���� �ð��� ������ �巡�� ��� ���� (�д��� �ƴϾ�� ��)
            if (dragging_time >= Util.drag_time_standard && !data.isPanic && !data.isDragging)
            {
                // ���� ĭ�� �� ĭ����
                BattleGridManager.instance.set_tile(data._coordinate.Item1, data._coordinate.Item2, BattleGridManager.boardCell.empty);
                data.isDragging = true;
                ActionManager.character_drag_started?.Invoke();

            }

            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);

        }
    }

    // ��ǥ �����ϴ� �޼ҵ�
    public void set_coordinate(int x, int y) 
    {
        data._coordinate = Tuple.Create(x, y);
    }

    // ��ǥ �˾Ƴ��� �޼ҵ�
    public Tuple<int, int> get_coordinate() 
    {
        return data._coordinate;
    }

    private void Update()
    {
        // ��Ʋ���� �巡������ ��� ���콺 �����Ϳ� ���� ����� Ÿ�Ϸ� �̵�
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
