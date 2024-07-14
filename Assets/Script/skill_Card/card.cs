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

    public GameObject owner; // ī�� ������ �ִ� ĳ����
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

    // ī���� ���� ��ġ, ȸ��, �������� ����
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
        // ���� ���߿� ���� �����ؾ���
        minpower = Card.minPowerOfLevel[0];
        maxpower = Card.maxPowerOfLevel[0];
        value_rangeTMP.text = string.Format("{0} - {1}", minpower, maxpower); 
        this.index = index;

    }

    // �־��� PRS�� Dotween ����� �̵� or �׳� �̵�
    public void MoveTransform(PRS prs, bool use_Dotween, float DotweenTime) 
    {
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
                        state = current_mode.normal;
                        CardManager.instance.clear_highlighted_card();
                        CardManager.instance.Align_cards(CardManager.instance.active_index);
                    }
                    yield break; 
                }

                // ��� ī�带 ���� order�� 
                CardManager.instance.Set_origin_order(CardManager.instance.active_index);

                // �巡�� ���̾�����
                if (state == current_mode.dragging)
                {
                    state = current_mode.normal;
                    CardManager.instance.clear_highlighted_card();
                    CardManager.instance.Align_cards(CardManager.instance.active_index);

                }
                // �巡�� ���� �ƴϸ�
                else 
                {
                    // ī�� ���̶���Ʈ or ���̶���Ʈ ����
                    if (CardManager.instance.highlighted_card != this)
                    {
                        CardManager.instance.highlight_card(this);
                    }
                    else
                    {
                        CardManager.instance.clear_highlighted_card();
                    }

                    // ���̶���Ʈ�� ī�� order����
                    if (CardManager.instance.highlighted_card != null)
                    {

                        CardManager.instance.highlighted_card.gameObject.GetComponent<element_order>().Set_highlighted_order();
                    }

                    // ī�� ��ġ ��� �� ����
                    CardManager.instance.Align_cards(CardManager.instance.active_index);
                }
                yield break;
            }
            
            // ���콺�� �� �� ���·� ���� �ð��� ������ �巡�� ��� ���� (�д��� �ƴϾ�� ��)
            if (dragging_time >= Util.drag_time_standard && !isDraggingStarted && !owner.GetComponent<Character>().isPanic) 
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
        // �巡�� �����ͷ� ī�� ������ �Ѱ���
        dragPointer.GetComponent<drag_pointer>().cards = Card;
        // ī�� ������� �巡�� �ϴ� ���̶�� ����, ī�� ������ �Ѱ���
        BattleCalcManager.instance.set_using_card(this);

        CardManager.instance.Align_cards(CardManager.instance.active_index);
    }

}
