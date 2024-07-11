using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class enemy_skillCard_slot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image illust;
    public GameObject card_obj;
    public GameObject enemy_Obj;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Image frame;

    // PC환경 때문에 쓰는 변수
    private bool isHighlightedByClick = false;
    bool isMouseOnThis = false;

    bool isBattleEnded;

    // 전투 끝났을때
    private void OnBattleEnd(bool victory) 
    {
        isBattleEnded = true;
    }

    // 위치 두 개를 주면 라인렌더러를 그려줌
    public IEnumerator Set_line(Vector3 target) 
    {
        yield return new WaitForSeconds(0.001f); // 레이아웃그룹 적용 시간 이슈때문에 약간 지연시킴
        Vector3 objPos = transform.position;
        lineRenderer.SetPosition(0, new Vector3(objPos.x, objPos.y, -1f));
        lineRenderer.SetPosition(1, new Vector3(target.x, target.y, 1f));
        lineRenderer.enabled = true;
        yield break;
    }

    // 라인렌더러 지움
    public void Remove_line() 
    {
        lineRenderer.enabled = false;
    }

    // 이 UI를 눌렀을 때
    public void OnPointerDown(PointerEventData eventData) 
    {
        if (isBattleEnded) { return; }

        // 왼쪽 클릭을 하고 현재 페이즈가 플레이어 스킬 사용 페이즈여만 작동됨
        if (eventData.button == PointerEventData.InputButton.Left && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase)
        {
            // 적 카드 강조 해제
            ActionManager.enemy_skill_card_deactivate?.Invoke();
            // 이 슬롯의 카드를 활성화 위치로
            CardManager.instance.highlight_enemy_card(card_obj);
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
            ActionManager.enemy_skill_card_deactivate?.Invoke();
            // 이 슬롯의 카드를 활성화 위치로
            CardManager.instance.highlight_enemy_card(card_obj);
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
            ActionManager.enemy_skill_card_deactivate?.Invoke();
        }

        isMouseOnThis = false;
    }

    // 스킬이 사용되면 시작되면 실행됨
    private void skill_used() 
    {
        // 나중에 채워질 예정
    }

    // 적 카드 하이라이트 해제시 실행
    private void Card_diactivated() 
    {
        isHighlightedByClick = false;
    }

    private void Awake()
    {
        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        ActionManager.skill_used += skill_used;
        ActionManager.enemy_skill_card_deactivate += Card_diactivated;
        ActionManager.battle_ended += OnBattleEnd;

        isBattleEnded = false;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
        ActionManager.skill_used -= skill_used;
        ActionManager.enemy_skill_card_deactivate -= Card_diactivated;
        ActionManager.battle_ended -= OnBattleEnd;
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
