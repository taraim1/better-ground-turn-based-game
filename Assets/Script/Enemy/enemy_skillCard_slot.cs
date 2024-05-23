using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class enemy_skillCard_slot : MonoBehaviour, IPointerClickHandler, IPointerUpHandler
{
    public Image illust;
    public GameObject card_obj;
    [SerializeField] private LineRenderer lineRenderer;

    // 위치 두 개를 주면 라인렌더러를 그려줌
    public IEnumerator Set_line(Vector3 target) 
    {
        yield return new WaitForSeconds(0.001f); // 레이아웃그룹 적용 시간 이슈때문에 약간 지연시킴
        Vector3 objPos = transform.position;
        lineRenderer.SetPosition(0, new Vector3(objPos.x, objPos.y, 1f));
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
    public void OnPointerClick(PointerEventData eventData) 
    {
        // 왼쪽 클릭을 하면
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 적 카드 강조 해제
            BattleEventManager.Trigger_event("enemy_skill_card_deactivate");
            // 이 슬롯의 카드를 활성화 위치로
            CardManager.instance.highlight_enemy_card(card_obj);
        }
    }

    // 이 UI 위에서 마우스 클릭을 해제했을 때
    public void OnPointerUp(PointerEventData eventData) 
    { 
    
    }

    private void Awake()
    {
        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;

    }
}
