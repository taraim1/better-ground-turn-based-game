using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;
using Unity.VisualScripting;

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

    public GameObject owner; // 카드 가지고 있는 캐릭터
    public int minpower;
    public int maxpower;

    [DoNotSerialize]
    public Coroutine running_drag = null;

    public bool isEnemyCard = false;
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

    public Cards Card;

    public void Setup(Cards card, int index) 
    {
        Card = card;

        illust.sprite = Card.sprite;
        nameTMP.text = Card.name;
        costTMP.text = Card.cost.ToString();
        typeTMP.text = Card.type;
        behavior_typeTMP.text = Card.behavior_type;
        // 여기 나중에 레벨 적용해야함
        minpower = Card.minPowerOfLevel[0];
        maxpower = Card.maxPowerOfLevel[0];
        value_rangeTMP.text = string.Format("{0} - {1}", minpower, maxpower); 
        this.index = index;

    }

    // 주어진 PRS로 Dotween 사용한 이동 or 그냥 이동
    public void MoveTransform(PRS prs, bool use_Dotween, float DotweenTime) 
    {
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
                        state = current_mode.normal;
                        CardManager.instance.clear_highlighted_card();
                        CardManager.instance.Align_cards(CardManager.instance.active_index);
                    }
                    yield break; 
                }

                // 모든 카드를 원래 order로 
                CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                // 드래그 중이었으면
                if (state == current_mode.dragging)
                {
                    state = current_mode.normal;
                    CardManager.instance.clear_highlighted_card();
                    CardManager.instance.Align_cards(CardManager.instance.active_index);

                }
                // 드래그 중이 아니면
                else 
                {
                    // 카드 하이라이트 or 하이라이트 해제
                    if (CardManager.instance.highlighted_card != this)
                    {
                        CardManager.instance.highlight_card(this);
                    }
                    else
                    {
                        CardManager.instance.clear_highlighted_card();
                    }

                    // 하이라이트된 카드 order설정
                    if (CardManager.instance.highlighted_card != null)
                    {

                        CardManager.instance.highlighted_card.gameObject.GetComponent<element_order>().Set_highlighted_order();
                    }

                    // 카드 위치 계산 및 정렬
                    CardManager.instance.Align_cards(CardManager.instance.active_index);
                }
                yield break;
            }
            
            // 마우스를 안 뗀 상태로 일정 시간이 지나면 드래그 기능 시작 (패닉이 아니어야 함)
            if (dragging_time >= Util.drag_time_standard && !isDraggingStarted && !owner.GetComponent<Character>().isPanic) 
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
        // 드래그 포인터로 카드 데이터 넘겨줌
        dragPointer.GetComponent<drag_pointer>().cards = Card;
        // 카드 판정기로 드래그 하는 중이라는 정보, 카드 데이터 넘겨줌
        BattleCalcManager.instance.set_using_card(this);

        CardManager.instance.Align_cards(CardManager.instance.active_index);
    }

}
