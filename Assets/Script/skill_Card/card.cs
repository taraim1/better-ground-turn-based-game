using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;
using Unity.VisualScripting;
using System;
using skillEffect;

public class card : MonoBehaviour, Iclickable
{
    public SpriteRenderer illust;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text costTMP;
    //[SerializeField] TMP_Text typeTMP;
    [SerializeField] TMP_Text behavior_typeTMP;
    [SerializeField] TMP_Text value_rangeTMP;
    [SerializeField] int index;

    Sprite origin_sprite;


    SpriteRenderer spriteRenderer;

    public GameObject drag_pointer;
    public Character target;

    public Character owner; // ī�� ������ �ִ� ĳ����
    public int minpower;
    public int maxpower;

    [DoNotSerialize]
    private Coroutine running_drag = null;

    public bool isEnemyCard = false;
    public bool _isShowingRange = false;
    private bool isDestroyed = false;
    public List<coordinate> usable_tiles;
    private List<SkillEffect> effects = new List<SkillEffect>();
    public List<SkillEffect> Effects => effects;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        origin_sprite = spriteRenderer.sprite;
        ActionManager.enemy_skillcard_deactivate += OnEnemyCardDeactivate;
    }
    public enum current_mode 
    { 
        normal,
        dragging,
        highlighted_enemy_card
    }

    public current_mode state = current_mode.normal;

    // ī���� ���� ��ġ, ȸ��, �������� ����
    public PRS originPRS;

    private CardData data;
    public CardData Data => data;

    public Action<card, Character> OnUsed;
    public Action<card, Character> OnDirectUsed;
    public Action<card, Character> OnClashWin;

    public void Setup(CardData data, int index) 
    {
        this.data = data;

        illust.sprite = Data.sprite;
        nameTMP.text = Data.Name;
        costTMP.text = Data.Cost.ToString();
        //typeTMP.text = Data.Type;

        string behaviorText = "";
        switch (Data.BehaviorType) 
        {
            case CardBehaviorType.attack:
                behaviorText = "����";
                break;
            case CardBehaviorType.defend:
                behaviorText = "���";
                break;
            case CardBehaviorType.dodge:
                behaviorText = "ȸ��";
                break;
            case CardBehaviorType.etc:
                behaviorText = "��Ÿ";
                break;
        }
        behavior_typeTMP.text = behaviorText;
        minpower = Data.MinPowerOfLevel[Data.Level-1];
        maxpower = Data.MaxPowerOfLevel[Data.Level-1];
        value_rangeTMP.text = string.Format("{0} - {1}", minpower, maxpower); 
        this.index = index;


        // Ư��ȿ�� �����

        // ��� ���� ��� ���� ȿ��
        Effects.Add(new TargetLimitEffect(skill_effect_code.target_limit, this, null, data.Targets));

        // ������ ȿ�� �����
        MakeEffects();
    }

    // ī�� Ư��ȿ�� �����
    private void MakeEffects() 
    {
        foreach (SkillEffect_label label in Data.skillEffect_Labels)
        {
            switch (label.Code)
            {
                case skill_effect_code.willpower_consumption:
                    Effects.Add(new Willpower_Consumtion(label.Code, this, label.Parameters));
                    break;
                case skill_effect_code.willpower_recovery:
                    Effects.Add(new Willpower_Recovery(label.Code, this, label.Parameters));
                    break;
                case skill_effect_code.ignition:
                    Effects.Add(new Ignition(label.Code, this, label.Parameters));
                    break;
                case skill_effect_code.fire_enchantment:
                    Effects.Add(new Fire_Enchantment(label.Code, this, label.Parameters));
                    break;
                case skill_effect_code.bleeding:
                    Effects.Add(new Bleeding(label.Code, this, label.Parameters));
                    break;
                case skill_effect_code.confusion:
                    Effects.Add(new Confusion(label.Code, this, label.Parameters));
                    break;
                case skill_effect_code.prepare_counterattack:
                    Effects.Add(new Prepare_counterAttack(label.Code, this, label.Parameters));
                    break;
                case skill_effect_code.dubble_attack:
                    Effects.Add(new DubbleAttack(label.Code, this, label.Parameters));
                    break;
            }
        }
    }


    public void OnClick() 
    {
        // �Ʊ� ī���̸�
        if (isEnemyCard == false)
        {
            // ī�� �巡�� ���� ����
            start_drag_detection(false);

            // �� ī�� ���� ����
            ActionManager.enemy_skillcard_deactivate?.Invoke();
        }
        // ���� ī���
        else
        {
            // �� ī�� ���� ����
            if (state == current_mode.highlighted_enemy_card)
            {
                ActionManager.enemy_skillcard_deactivate?.Invoke();
            }
        }
    }

    public void Destroy_card() 
    {
        if (running_drag != null) 
        {
            StopCoroutine(running_drag);
        }
        transform.DOKill();
        isDestroyed = true;
        foreach (SkillEffect effect in effects) 
        {
            effect.OnDestroy();
        }

        Destroy(gameObject);
    }

    // �־��� PRS�� Dotween ����� �̵� or �׳� �̵�
    public void MoveTransform(PRS prs, bool use_Dotween, float DotweenTime) 
    {
        if (isDestroyed) return;

        // �̵� �ʱ�ȭ
        transform.DOKill();

        if (use_Dotween) 
        {
            transform.DOMove(prs.pos, DotweenTime);
            transform.DORotateQuaternion(prs.rot, DotweenTime);
            transform.DOScale(prs.scale, DotweenTime);
            return;
        }

        transform.position = prs.pos;
        transform.rotation = prs.rot;
        transform.localScale = prs.scale;
    }

    // Ư�� ��ǥ�� �� ī�带 �� �� �ִ��� ��ȯ
    public bool check_usable_coordinate(coordinate coordinate) 
    {
        if (Data.RangeType == CardRangeType.limited)
        {
            return get_use_range().Contains(coordinate);
        }

        // unlimited�� ���
        return true;
    }

    public void start_drag_detection(bool keepHighLightedFlag)
    {
        running_drag = StartCoroutine(detect_drag(keepHighLightedFlag));
    }

    // ī�� �巡�� ���� (���� �ð� �̻� ��� �־�߸� �巡�׷� �Ǻ�)
    private IEnumerator detect_drag(bool keepHighLightedFlag) 
    {
        float dragging_time = 0;
        bool isDraggingStarted = false;
        Character ownerCharacter = owner.GetComponent<Character>();

        while (true) 
        {
            // ���콺�� �� �� ���·� ���� �ð��� ������ �巡�� ��� ���� (�д��� �ƴϾ�� ��)
            if (dragging_time >= Util.drag_time_standard && !isDraggingStarted && !ownerCharacter.IsPanic)
            {
                // ��� ī�带 ���� order�� 
                CardManager.instance.Set_origin_order(CardManager.instance.active_index);
                isDraggingStarted = true;
                // ���̶���Ʈ�� ī�� ����
                CardManager.instance.clear_highlighted_card();
                drag_card();
            }

            // ���콺 ���� (��ųī�� ���� ���� ��� �巡������ �� ����ġ�θ� ��)
            if (Input.GetMouseButton(0) == false) 
            {   
                if (!keepHighLightedFlag)
                {
                    // ��� ī�带 ���� order�� 
                    CardManager.instance.Set_origin_order(CardManager.instance.active_index);
                }

                // �巡�� ���̾�����
                if (state == current_mode.dragging)
                {
                    OnDragEnd();

                }
                // �巡�� ���� �ƴϸ�
                else 
                {
                    if (keepHighLightedFlag)
                    {
                        yield break;
                    }

                     // ī�� ���̶���Ʈ or ���̶���Ʈ ����
                    if (CardManager.instance.highlightedData != this)
                    {
                        CardManager.instance.highlight_card(this);
                    }
                    else
                    {
                        CardManager.instance.clear_highlighted_card();
                    }

                    // ���̶���Ʈ�� ī�� order����
                    if (CardManager.instance.highlightedData != null)
                    {

                        CardManager.instance.highlightedData.gameObject.GetComponent<element_order>().Set_highlighted_order();
                    }

                    // ī�� ��ġ ��� �� ����
                    CardManager.instance.Align_cards(CardManager.instance.active_index);
                }
                yield break;
            }

            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    void drag_card() // ī�� �巡�׽� �����
    {
        state = current_mode.dragging;
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject dragPointer = Instantiate(drag_pointer, new Vector3(mousepos.x, mousepos.y, -2), Quaternion.identity);
        dragPointer.GetComponent<SpriteRenderer>().sortingOrder = 200;
        // ī�� ������� �巡�� �ϴ� ���̶�� ����, ī�� ������ �Ѱ���
        BattleCalcManager.instance.set_using_card(this);

        CardManager.instance.Align_cards(CardManager.instance.active_index);

        // ��� ������ �ִ� ��ų�̸�
        if (Data.RangeType == CardRangeType.limited)
        {
            // �� �� �ִ� Ÿ�� �Ǻ�
            usable_tiles = get_use_range();

            // �� Ÿ�ϵ��� �ʷϻ�����
            foreach (coordinate coordinate in usable_tiles) 
            {
                BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.green);
            }
            _isShowingRange = true;
        }
    }

    void OnDragEnd() 
    {
        // ��� ������ �ִ� ��ų�̸�
        if (Data.RangeType == CardRangeType.limited)
        {
            // ��� ���� Ÿ�ϵ��� ���� ������
            foreach (coordinate coordinate in usable_tiles)
            {
                BattleGridManager.instance.revert_tile_color_to_original(coordinate);
            }
            _isShowingRange = false;
        }

        state = current_mode.normal;
        CardManager.instance.clear_highlighted_card();
        CardManager.instance.Align_cards(CardManager.instance.active_index);
    }

    public List<coordinate> get_use_range() 
    {
        List<coordinate> relative_coors = Data.get_copy_of_use_range();
        List<coordinate> result = new List<coordinate>();

        foreach (coordinate coor in relative_coors) 
        {
            result.Add(owner.Coordinate + coor);
        }

        return result;
    }

    public bool check_card_usable(card card, Character character) 
    {
        foreach (SkillEffect skillEffect in Effects) 
        {
            if (!skillEffect.check_card_usable(card, character)) 
            {
                return false;
            }
        }

        return true;
    }

    // ���� ������
    public int PowerRoll() 
    {
        int power = UnityEngine.Random.Range(minpower, maxpower + 1);

        // ĳ���� ���� ���� ���� / ����� ����
        foreach (CharacterEffect.character_effect effect in owner.effects) 
        {
            power += effect.Get_power_change(this);
        }

        // ��ų ��� �� ���� �����ִ°�
        if (!Data.DontShowPowerRollResult) { show_power_roll_result(power); }

        return power;
    }

    // ��ų ��� �� ���� �����ִ°�
    private void show_power_roll_result(int power)
    {
        owner.show_power_meter?.Invoke(power);
    }

    private void OnEnemyCardDeactivate() 
    {
        if (isEnemyCard) 
        {
            transform.DOKill();
            transform.position = originPRS.pos;
            transform.localScale = originPRS.scale;
        }
    }

    private void OnDestroy()
    {
        ActionManager.card_destroyed?.Invoke(this);
        ActionManager.enemy_skillcard_deactivate -= OnEnemyCardDeactivate;
    }
}
