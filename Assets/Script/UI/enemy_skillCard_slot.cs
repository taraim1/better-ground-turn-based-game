using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class enemy_skillCard_slot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SpriteRenderer illust;
    public GameObject card_obj;
    public GameObject enemy_Obj;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Image frame;

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
        // 왼쪽 클릭을 하고 현재 페이즈가 플레이어 스킬 사용 페이즈여만 작동됨
        if (eventData.button == PointerEventData.InputButton.Left && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase)
        {
            // 적 카드 강조 해제
            BattleEventManager.Trigger_event("enemy_skill_card_deactivate");
            // 이 슬롯의 카드를 활성화 위치로
            CardManager.instance.highlight_enemy_card(card_obj);
        }
    }

    // 이 UI 위에 마우스를 대면
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (BattleCalcManager.instance.IsUsingCard) // 카드 사용 중이면
        {
            // 이 슬롯의 카드를 카드 판정 대상으로
            BattleCalcManager.instance.set_target(card_obj.GetComponent<card>());
            // 적 카드 강조 해제
            BattleEventManager.Trigger_event("enemy_skill_card_deactivate");
            // 이 슬롯의 카드를 활성화 위치로
            CardManager.instance.highlight_enemy_card(card_obj);
        }

    }

    // 이 UI 위에 마우스를 대었다가 나가면
    public void OnPointerExit(PointerEventData eventData)
    {
        if (BattleCalcManager.instance.IsUsingCard) // 카드 사용 중이면
        {
            // 타겟 설정 해제
            BattleCalcManager.instance.clear_target();
            // 적 카드 강조 해제
            BattleEventManager.Trigger_event("enemy_skill_card_deactivate");
        }

    }

    // 스킬이 사용되면 시작되면 실행됨
    private void skill_used() 
    {
        // 나중에 채워질 예정
    }


    private void Awake()
    {
        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        BattleEventManager.skill_used += skill_used;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
        BattleEventManager.skill_used -= skill_used;
    }
}
