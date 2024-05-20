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
    public Sprite drag_spr;

    public GameObject drag_pointer;
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
        value_rangeTMP.text = string.Format("{0} - {1}", Card.minPowerOfLevel[0], Card.maxPowerOfLevel[0]);
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
    public IEnumerator detect_drag() 
    {
        float dragging_time = 0;
        bool isDraggingStarted = false;

        while (true) 
        {
            

            // ���콺 ���� ���� ����, ī�� ���̶���Ʈ ����, ī�� �巡�� ����
            if (Input.GetMouseButton(0) == false) 
            {
                if (state == current_mode.dragging) 
                {
                    state = current_mode.normal;
                    CardManager.instance.highlighted_card = null;
                    CardManager.instance.Aline_cards(CardManager.instance.active_index);
                    spriteRenderer.sprite = origin_sprite;

                }
                yield break;
            }
            
            // ���콺�� �� �� ���·� ���� �ð��� ������ �巡�� ��� ����
            if (dragging_time >= Util.drag_time_standard && !isDraggingStarted ) 
            {

                isDraggingStarted = true;
                // ���̶���Ʈ�� ī�� ����
                CardManager.instance.highlighted_card = null;
                drag_card();

            }

            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    void drag_card() 
    {
        state = current_mode.dragging;
        spriteRenderer.sprite = drag_spr;
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject dragPointer = Instantiate(drag_pointer, new Vector3(mousepos.x, mousepos.y, -2), Quaternion.identity);
        dragPointer.GetComponent<SpriteRenderer>().sortingOrder = 200;
        // �巡�� �����ͷ� ī�� ������ �Ѱ���
        dragPointer.GetComponent<drag_pointer>().cards = Card;

        CardManager.instance.Aline_cards(CardManager.instance.active_index);
    }

}
