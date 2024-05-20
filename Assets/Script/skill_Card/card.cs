using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;

public class card : MonoBehaviour
{
    [SerializeField] SpriteRenderer illust;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text costTMP;
    [SerializeField] TMP_Text typeTMP;
    [SerializeField] TMP_Text behavior_typeTMP;
    [SerializeField] TMP_Text value_rangeTMP;
    [SerializeField] int index;

    Sprite origin_sprite;


    SpriteRenderer spriteRenderer;
    public Sprite drag_pointer_spr;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        origin_sprite = spriteRenderer.sprite;
    }
    public enum current_mode 
    { 
        normal,
        dragging
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
        value_rangeTMP.text = string.Format("{0} - {1}", Card.minPowerOfLevel[0], Card.maxPowerOfLevel[0]);
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
    public IEnumerator detect_drag() 
    {
        float dragging_time = 0;
        bool isDraggingStarted = false;

        while (true) 
        {
            

            // 마우스 떼면 감지 멈춤, 카드 하이라이트 해제, 카드 드래그 해제
            if (Input.GetMouseButton(0) == false) 
            {
                if (state == current_mode.dragging) 
                {
                    CardManager.instance.highlighted_card = null;
                    illust.sprite = Card.sprite;
                    nameTMP.text = Card.name;
                    costTMP.text = Card.cost.ToString();
                    typeTMP.text = Card.type;
                    behavior_typeTMP.text = Card.behavior_type;
                    value_rangeTMP.text = string.Format("{0} - {1}", Card.minPowerOfLevel[0], Card.maxPowerOfLevel[0]);

                    spriteRenderer.sprite = origin_sprite;

                }
                state = current_mode.normal;
                CardManager.instance.Aline_cards(CardManager.instance.active_index);
                yield break;
            }
            
            // 마우스를 안 뗀 상태로 일정 시간이 지나면 드래그 기능 시작
            if (dragging_time >= Util.drag_time_standard && !isDraggingStarted ) 
            {

                isDraggingStarted = true;
                // 하이라이트된 카드 해제
                CardManager.instance.highlighted_card = null;
                drag_card();

            }

            CardManager.instance.Aline_cards(CardManager.instance.active_index);
            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    void drag_card() 
    {
        state = current_mode.dragging;
        spriteRenderer.sprite = drag_pointer_spr;
        illust.sprite = null;
        nameTMP.text = null;
        costTMP.text = null;
        typeTMP.text = null;
        behavior_typeTMP.text = null;
        value_rangeTMP.text = null;
    }

}
