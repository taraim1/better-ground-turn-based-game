using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class deckCell_card_collection_module : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject temp_card_prefab;
    private Canvas canvas;
    private GameObject temp_card;
    private Deck_changer deck_changer;
    private skillcard_code code;
    private Character_popup character_Popup;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void Setup(Deck_changer deck_Changer, skillcard_code code, Character_popup popup) 
    {
        deck_changer = deck_Changer;
        this.code = code;
        character_Popup = popup;
    }

    private void OnDragStarted() 
    { 
        // 임시 카드 소환
        temp_card = Instantiate(temp_card_prefab, canvas.gameObject.transform);
        deckCell temp_cell = temp_card.GetComponent<deckCell>();
        temp_cell.Setup(code);
    }

    private void OnDragFinished() 
    {
        if (temp_card != null) 
        {
            // 임시 카드 삭제
            Destroy(temp_card);

            // 덱 바꾸기
            deck_changer.change();

            // 덱셀 갱신
            character_Popup.Update_character_deckCell();
        }
        
    }

    private void OnClicked() 
    { 
    
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(detect_drag());
    }

    // 드래그 시작, 끝 or 그냥 클릭 감지 (일정 시간 이상 잡고 있어야만 드래그로 판별)
    private IEnumerator detect_drag()
    {
        float dragging_time = 0;
        bool isDraggingStarted = false;

        // 덱체인저 설정
        deck_changer.set_index(-1);
        deck_changer.set_swap_code(code);

        while (true)
        {
            // 마우스를 안 뗀 상태로 일정 시간이 지나면 드래그 기능 시작
            if (dragging_time >= Util.drag_time_standard && !isDraggingStarted)
            {
                isDraggingStarted = true;
                OnDragStarted();
            }

            // 마우스 떼면
            if (Input.GetMouseButton(0) == false)
            {

                // 드래그 중이었으면
                if (isDraggingStarted)
                {
                    OnDragFinished();
                }
                // 드래그 중이 아니면
                else
                {
                    OnClicked();
                }
                yield break;
            }

            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }

}
