using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEditor;

public class enemy_skillCard_slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image illust;
    public GameObject card_obj;
    public GameObject enemy_Obj;
    public GameObject target_obj;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Image frame;

    Color red = new Color(1f, 0f, 0f, 1f);
    Color grey = new Color(0.7f, 0.7f, 0.7f, 1f);

    public Coroutine running_drag = null;

    // PC환경 때문에 쓰는 변수
    private bool isHighlightedByClick = false;
    bool isMouseOnThis = false;

    bool isBattleEnded;
    bool isDragging = false;

    private List<Tuple<int, int>> current_range;

    // 전투 끝났을때
    private void OnBattleEnd(bool victory) 
    {
        isBattleEnded = true;
    }

    // 위치를 주면 라인렌더러를 그려줌
    public IEnumerator Set_line(Vector3 target) 
    {
        yield return new WaitForSeconds(0.001f); // 레이아웃그룹 적용 시간 이슈때문에 약간 지연시킴

        card using_card = card_obj.GetComponent<card>();
        Character target_character = target_obj.GetComponent<Character>();
        Character using_character = using_card.owner.GetComponent<Character>();

        // 색상 설정
        set_lineRenderer_color(red);
        if (using_card.Data.RangeType == CardRangeType.limited) 
        {

            if (!BattleCalcManager.instance.check_limited_range_usable(target_character.get_coordinate(), using_card.get_use_range(using_character.get_coordinate()))) 
            {
                set_lineRenderer_color(grey);
            }
        }
        

        Vector3 objPos = transform.position;
        lineRenderer.SetPosition(0, new Vector3(objPos.x, objPos.y, -1f));
        lineRenderer.SetPosition(1, new Vector3(target.x, target.y, 1f));
        lineRenderer.enabled = true;
        yield break;
    }

    // 라인렌더러 색 설정
    private void set_lineRenderer_color(Color color) 
    {
        lineRenderer.material.color = color;
    }

    // 라인렌더러 지움
    public void Remove_line() 
    {
        lineRenderer.enabled = false;
    }

    // 이 UI를 눌렀을 때 발동
    private void OnPointerDown() 
    {
        if (isBattleEnded) { return; }

        // 현재 페이즈가 플레이어 스킬 사용 페이즈여만 작동됨
        if (BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase)
        {
            // 적 카드 강조 해제
            ActionManager.enemy_skillData_deactivate?.Invoke();
            // 이 슬롯의 카드를 활성화 위치로
            CardManager.instance.highlight_enemyData(card_obj);
            isHighlightedByClick = true;
        }
    }

    // 이 UI 위에 마우스를 대면 (모바일상에선 드래그 중에)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isBattleEnded) { return; }

        if (BattleCalcManager.instance.IsUsingCard) // 카드 사용 중이면
        {
            // 이 슬롯의 카드를 카드 판정 대상으로
            BattleCalcManager.instance.set_target(card_obj.GetComponent<card>());
            // 적 카드 강조 해제
            ActionManager.enemy_skillData_deactivate?.Invoke();
            // 이 슬롯의 카드를 활성화 위치로
            CardManager.instance.highlight_enemyData(card_obj);
            isHighlightedByClick = false;
        }

        isMouseOnThis = true;

    }

    // 드래그 중 이 UI 위에 들어왔다 나가면
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isBattleEnded) { return; }

        if (!isHighlightedByClick) // 클릭으로 하이라이트된 게 아니면
        {
            // 타겟 설정 해제
            BattleCalcManager.instance.clear_target_card();
            // 적 카드 강조 해제
            ActionManager.enemy_skillData_deactivate?.Invoke();
        }

        isMouseOnThis = false;
    }

    // 드래그 감지
    public IEnumerator detect_drag()
    {
        card card = card_obj.GetComponent<card>();

        float dragging_time = 0;
        while (true)
        {


            // 마우스 뗴면
            if (Input.GetMouseButton(0) == false)
            {
                // 드래그 아닌 경우
                if (!isDragging)
                {
                    OnPointerDown();
                    yield break;
                }

                // 드래그 중인 경우

                // 스킬 사정거리 보이기 해제
                if (card.Data.RangeType == CardRangeType.limited)
                {
                    foreach (Tuple<int, int> coordinate in current_range)
                    {
                        BattleGridManager.instance.set_tile_color(coordinate.Item1, coordinate.Item2, Tile.TileColor.original);
                    }
                }
                isDragging = false;
                yield break;
            }

            // 마우스를 안 뗀 상태로 일정 시간이 지나면 드래그 기능 시작
            if (dragging_time >= Util.drag_time_standard && !isDragging)
            {
                // 적 카드 강조 해제
                ActionManager.enemy_skillData_deactivate?.Invoke();

                // 스킬 사정거리 보이기
                if (card.Data.RangeType == CardRangeType.limited) 
                {
                    current_range = card.get_use_range(card.owner.GetComponent<Character>().get_coordinate());
                    foreach (Tuple<int, int> coordinate in current_range) 
                    {
                        BattleGridManager.instance.set_tile_color(coordinate.Item1, coordinate.Item2, Tile.TileColor.red);
                    }
                }
                isDragging = true;
            }

            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);

        }
    }

    // 스킬이 사용되면 실행됨
    private void skill_used() 
    {
        // 나중에 채워질 예정
    }

    // 적 카드 하이라이트 해제시 실행
    private void Card_diactivated() 
    {
        isHighlightedByClick = false;
    }

    private void On_character_drag_started() 
    {
        Remove_line();
    }

    private void On_character_drag_ended() 
    {
        if (target_obj != null) 
        {
            StartCoroutine(Set_line(target_obj.transform.position));
        }
    }

    private void Awake()
    {
        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        ActionManager.skill_used += skill_used;
        ActionManager.enemy_skillData_deactivate += Card_diactivated;
        ActionManager.battle_ended += OnBattleEnd;
        ActionManager.character_drag_started += On_character_drag_started;
        ActionManager.character_drag_ended += On_character_drag_ended;

        isBattleEnded = false;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
        ActionManager.skill_used -= skill_used;
        ActionManager.enemy_skillData_deactivate -= Card_diactivated;
        ActionManager.battle_ended -= OnBattleEnd;
        ActionManager.character_drag_started -= On_character_drag_started;
        ActionManager.character_drag_ended -= On_character_drag_ended;
    }

    private void Update()
    {
        if (isMouseOnThis && BattleCalcManager.instance.IsUsingCard) 
        {
            // 이 슬롯의 카드를 카드 판정 대상으로 (드래그 타이밍 때문에 버그나는거 수정)
            BattleCalcManager.instance.set_target(card_obj.GetComponent<card>());
        }

        // 스킬을 썼는데도 슬롯이 안 없어지는 버그 수정
        try 
        {
            Transform trans = card_obj.transform;
        }
        catch(MissingReferenceException e) 
        {
            Destroy(gameObject);
        }

    }
}
