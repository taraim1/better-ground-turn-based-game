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

    public Character owner; // 카드 가지고 있는 캐릭터
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

    // 카드의 원래 위치, 회전, 스케일을 저장
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

    // 주어진 PRS로 Dotween 사용한 이동 or 그냥 이동
    public void MoveTransform(PRS prs, bool use_Dotween, float DotweenTime) 
    {
        if (isDestroyed) return;

        // 이동 초기화
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

    // 특정 좌표에 이 카드를 쓸 수 있는지 반환
    public bool check_usable_coordinate(coordinate coordinate) 
    {
        if (Data.RangeType == CardRangeType.limited)
        {
            return get_use_range(owner.Coordinate).Contains(coordinate);
        }

        // unlimited인 경우
        return true;
    }

    // 카드 드래그 감지 (일정 시간 이상 잡고 있어야만 드래그로 판별)
    public IEnumerator detect_drag(bool isDescription) 
    {
        float dragging_time = 0;
        bool isDraggingStarted = false;

        while (true) 
        {
            

            // 마우스 떼면 (스킬카드 설명 누른 경우 드래그중일 때 원위치로만 감)
            if (Input.GetMouseButton(0) == false) 
            {   
                if (isDescription) 
                {
                    // 드래그 중이었으면
                    if (state == current_mode.dragging)
                    {
                        OnDragEnd();
                    }
                    yield break; 
                }

                // 모든 카드를 원래 order로 
                CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                // 드래그 중이었으면
                if (state == current_mode.dragging)
                {
                    OnDragEnd();

                }
                // 드래그 중이 아니면
                else 
                {
                    // 카드 하이라이트 or 하이라이트 해제
                    if (CardManager.instance.highlightedData != this)
                    {
                        CardManager.instance.highlightData(this);
                    }
                    else
                    {
                        CardManager.instance.clear_highlighted_card();
                    }

                    // 하이라이트된 카드 order설정
                    if (CardManager.instance.highlightedData != null)
                    {

                        CardManager.instance.highlightedData.gameObject.GetComponent<element_order>().Set_highlighted_order();
                    }

                    // 카드 위치 계산 및 정렬
                    CardManager.instance.Align_cards(CardManager.instance.active_index);
                }
                yield break;
            }
            
            // 마우스를 안 뗀 상태로 일정 시간이 지나면 드래그 기능 시작 (패닉이 아니어야 함)
            if (dragging_time >= Util.drag_time_standard && !isDraggingStarted && !owner.GetComponent<Character>().IsPanic) 
            {
                // 모든 카드를 원래 order로 
                CardManager.instance.Set_origin_order(CardManager.instance.active_index);
                isDraggingStarted = true;
                // 하이라이트된 카드 해제
                CardManager.instance.clear_highlighted_card();
                drag_card();
            }

            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    void drag_card() // 카드 드래그시 실행됨
    {
        state = current_mode.dragging;
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject dragPointer = Instantiate(drag_pointer, new Vector3(mousepos.x, mousepos.y, -2), Quaternion.identity);
        dragPointer.GetComponent<SpriteRenderer>().sortingOrder = 200;
        // 카드 판정기로 드래그 하는 중이라는 정보, 카드 데이터 넘겨줌
        BattleCalcManager.instance.set_using_card(this);

        CardManager.instance.Align_cards(CardManager.instance.active_index);

        // 사용 범위가 있는 스킬이면
        if (Data.RangeType == CardRangeType.limited)
        {
            // 쓸 수 있는 타일 판별
            Character using_character = BattleManager.instance.playable_characters[CardManager.instance.active_index].GetComponent<Character>();
            usable_tiles = get_use_range(using_character.Coordinate);

            // 그 타일들을 초록색으로
            foreach (coordinate coordinate in usable_tiles) 
            {
                BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.green);
            }
            _isShowingRange = true;
        }
    }

    void OnDragEnd() 
    {
        // 사용 범위가 있는 스킬이면
        if (Data.RangeType == CardRangeType.limited)
        {
            // 사용 범위 타일들을 원래 색으로
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
