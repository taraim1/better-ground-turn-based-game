using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;
using Unity.VisualScripting;
using System;

public class card : MonoBehaviour
{
    public SpriteRenderer illust;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text costTMP;
    [SerializeField] TMP_Text typeTMP;
    [SerializeField] TMP_Text behavior_typeTMP;
    [SerializeField] TMP_Text value_rangeTMP;
    [SerializeField] int index;

    Sprite origin_sprite;


    SpriteRenderer spriteRenderer;

    public GameObject drag_pointer;
    public GameObject target;

    public Character owner; // ī�� ������ �ִ� ĳ����
    public int minpower;
    public int maxpower;

    [DoNotSerialize]
    public Coroutine running_drag = null;

    public bool isEnemyCard = false;
    public bool _isShowingRange = false;
    private bool isDestroyed = false;
    public List<coordinate> usable_tiles;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        origin_sprite = spriteRenderer.sprite;
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

    public void Setup(CardData data, int index) 
    {
        this.data = data;

        illust.sprite = Data.sprite;
        nameTMP.text = Data.Name;
        costTMP.text = Data.Cost.ToString();
        typeTMP.text = Data.Type;
        behavior_typeTMP.text = Data.BehaviorType;
        minpower = Data.MinPowerOfLevel[Data.Level-1];
        maxpower = Data.MaxPowerOfLevel[Data.Level-1];
        value_rangeTMP.text = string.Format("{0} - {1}", minpower, maxpower); 
        this.index = index;

    }

    public void Destroy_card() 
    {
        StopCoroutine(running_drag);
        transform.DOKill();
        isDestroyed = true;
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
            return get_use_range(owner.Coordinate).Contains(coordinate);
        }

        // unlimited�� ���
        return true;
    }

    // ī�� �巡�� ���� (���� �ð� �̻� ��� �־�߸� �巡�׷� �Ǻ�)
    public IEnumerator detect_drag(bool isDescription) 
    {
        float dragging_time = 0;
        bool isDraggingStarted = false;

        while (true) 
        {
            

            // ���콺 ���� (��ųī�� ���� ���� ��� �巡������ �� ����ġ�θ� ��)
            if (Input.GetMouseButton(0) == false) 
            {   
                if (isDescription) 
                {
                    // �巡�� ���̾�����
                    if (state == current_mode.dragging)
                    {
                        OnDragEnd();
                    }
                    yield break; 
                }

                // ��� ī�带 ���� order�� 
                CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                // �巡�� ���̾�����
                if (state == current_mode.dragging)
                {
                    OnDragEnd();

                }
                // �巡�� ���� �ƴϸ�
                else 
                {
                    // ī�� ���̶���Ʈ or ���̶���Ʈ ����
                    if (CardManager.instance.highlightedData != this)
                    {
                        CardManager.instance.highlightData(this);
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
            
            // ���콺�� �� �� ���·� ���� �ð��� ������ �巡�� ��� ���� (�д��� �ƴϾ�� ��)
            if (dragging_time >= Util.drag_time_standard && !isDraggingStarted && !owner.GetComponent<Character>().IsPanic) 
            {
                // ��� ī�带 ���� order�� 
                CardManager.instance.Set_origin_order(CardManager.instance.active_index);
                isDraggingStarted = true;
                // ���̶���Ʈ�� ī�� ����
                CardManager.instance.clear_highlighted_card();
                drag_card();
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
            Character using_character = BattleManager.instance.playable_characters[CardManager.instance.active_index].GetComponent<Character>();
            usable_tiles = get_use_range(using_character.Coordinate);

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
                BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.original);
            }
            _isShowingRange = false;
        }

        state = current_mode.normal;
        CardManager.instance.clear_highlighted_card();
        CardManager.instance.Align_cards(CardManager.instance.active_index);
    }

    public List<coordinate> get_use_range(coordinate character_coordinate) 
    {
        List<coordinate> relative_coors = Data.get_copy_of_use_range();
        List<coordinate> result = new List<coordinate>();

        foreach (coordinate coor in relative_coors) 
        {
            result.Add(character_coordinate + coor);
        }

        return result;
    }

    private void OnDestroy()
    {
        ActionManager.card_destroyed?.Invoke(this);
    }
}
