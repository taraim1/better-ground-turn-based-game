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
        // �ӽ� ī�� ��ȯ
        temp_card = Instantiate(temp_card_prefab, canvas.gameObject.transform);
        deckCell temp_cell = temp_card.GetComponent<deckCell>();
        temp_cell.Setup(code);
    }

    private void OnDragFinished() 
    {
        if (temp_card != null) 
        {
            // �ӽ� ī�� ����
            Destroy(temp_card);

            // �� �ٲٱ�
            deck_changer.change();

            // ���� ����
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

    // �巡�� ����, �� or �׳� Ŭ�� ���� (���� �ð� �̻� ��� �־�߸� �巡�׷� �Ǻ�)
    private IEnumerator detect_drag()
    {
        float dragging_time = 0;
        bool isDraggingStarted = false;

        // ��ü���� ����
        deck_changer.set_index(-1);
        deck_changer.set_swap_code(code);

        while (true)
        {
            // ���콺�� �� �� ���·� ���� �ð��� ������ �巡�� ��� ����
            if (dragging_time >= Util.drag_time_standard && !isDraggingStarted)
            {
                isDraggingStarted = true;
                OnDragStarted();
            }

            // ���콺 ����
            if (Input.GetMouseButton(0) == false)
            {

                // �巡�� ���̾�����
                if (isDraggingStarted)
                {
                    OnDragFinished();
                }
                // �巡�� ���� �ƴϸ�
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
