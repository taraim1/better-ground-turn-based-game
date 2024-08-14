using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEditor;

public class enemy_skillCard_slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, Iclickable
{
    [SerializeField] private Image illustImage;
    private GameObject card_obj;
    private card card;
    private Character target_character;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Image frame;

    Color red = new Color(1f, 0f, 0f, 1f);
    Color grey = new Color(0.7f, 0.7f, 0.7f, 1f);

    private Coroutine running_drag = null;

    // PCȯ�� ������ ���� ����
    private bool isHighlightedByClick = false;
    bool isMouseOnThis = false;

    bool isDragging = false;

    private List<coordinate> current_range;

    public void Initialize(Sprite illust, GameObject card_obj)
    {
        illustImage.sprite = illust;
        this.card_obj = card_obj;
        card = card_obj.GetComponent<card>();
        if (card.target != null)
        {
            target_character = card.target;
            StartCoroutine(Set_line(target_character.gameObject.transform.position));
        }
        
    }

    // ���� ��������
    private void OnBattleEnd(bool victory) 
    {
        Destroy(gameObject);
    }

    public void OnClick() 
    {
        // �Ʊ� ī�� ���� ����
        CardManager.instance.clear_highlighted_card();

        // ��� ī�带 ���� order�� 
        CardManager.instance.Set_origin_order(CardManager.instance.active_index);

        // ī�� ��ġ ��� �� ����
        CardManager.instance.Align_cards(CardManager.instance.active_index);

        // �巡�� ���� ����
        running_drag = StartCoroutine(detect_drag());
    }

    // ��ġ�� �ָ� ���η������� �׷���
    public IEnumerator Set_line(Vector3 target) 
    {
        yield return new WaitForSeconds(0.001f); // ���̾ƿ��׷� ���� �ð� �̽������� �ణ ������Ŵ

        // ���� ����
        set_lineRenderer_color(red);

        if (!card.check_usable_coordinate(target_character.Coordinate)) 
        {
            set_lineRenderer_color(grey);
        }


        Vector3 objPos = transform.position;
        lineRenderer.SetPosition(0, new Vector3(objPos.x, objPos.y, -1f));
        lineRenderer.SetPosition(1, new Vector3(target.x, target.y, 1f));
        lineRenderer.enabled = true;
        yield break;
    }

    // ���η����� �� ����
    private void set_lineRenderer_color(Color color) 
    {
        lineRenderer.material.color = color;
    }

    // ���η����� ����
    public void Remove_line() 
    {
        lineRenderer.enabled = false;
    }

    // �� UI�� ������ �� �ߵ�
    private void OnPointerDown() 
    {
        // ���� ����� �÷��̾� ��ų ��� ������� �۵���
        if (BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase)
        {
            // �� ī�� ���� ����
            ActionManager.enemy_skillcard_deactivate?.Invoke();
            // �� ������ ī�带 Ȱ��ȭ ��ġ��
            CardManager.instance.highlight_enemy_card(card_obj);
            isHighlightedByClick = true;
        }
    }

    // �� UI ���� ���콺�� ��� (����ϻ󿡼� �巡�� �߿�)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (BattleCalcManager.instance.isUsingCard()) // ī�� ��� ���̸�
        {
            // �� ������ ī�带 ī�� ���� �������
            BattleCalcManager.instance.set_target(card_obj.GetComponent<card>());
            // �� ī�� ���� ����
            ActionManager.enemy_skillcard_deactivate?.Invoke();
            // �� ������ ī�带 Ȱ��ȭ ��ġ��
            CardManager.instance.highlight_enemy_card(card_obj);
            isHighlightedByClick = false;
        }

        isMouseOnThis = true;

    }

    // �巡�� �� �� UI ���� ���Դ� ������
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isHighlightedByClick && BattleCalcManager.instance.isUsingCard()) // Ŭ������ ���̶���Ʈ�� �� �ƴϸ�
        {
            // Ÿ�� ���� ����
            BattleCalcManager.instance.clear_target_card();
            // �� ī�� ���� ����
            ActionManager.enemy_skillcard_deactivate?.Invoke();
        }

        isMouseOnThis = false;
    }

    // �巡�� ����
    private IEnumerator detect_drag()
    {
        card card = card_obj.GetComponent<card>();

        float dragging_time = 0;
        while (true)
        {


            // ���콺 ���
            if (Input.GetMouseButton(0) == false)
            {
                // �巡�� �ƴ� ���
                if (!isDragging)
                {
                    OnPointerDown();
                    yield break;
                }

                // �巡�� ���� ���

                // ��ų �����Ÿ� ���̱� ����
                if (card.Data.RangeType == CardRangeType.limited)
                {
                    foreach (coordinate coordinate in current_range)
                    {
                        BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.original);
                    }
                }
                isDragging = false;
                yield break;
            }

            // ���콺�� �� �� ���·� ���� �ð��� ������ �巡�� ��� ����
            if (dragging_time >= Util.drag_time_standard && !isDragging)
            {
                // �� ī�� ���� ����
                ActionManager.enemy_skillcard_deactivate?.Invoke();

                // ��ų �����Ÿ� ���̱�
                if (card.Data.RangeType == CardRangeType.limited) 
                {
                    current_range = card.get_use_range(card.owner.Coordinate);
                    foreach (coordinate coordinate in current_range) 
                    {
                        BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.red);
                    }
                }
                isDragging = true;
            }

            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);

        }
    }

    // �� ī�� ���̶���Ʈ ������ ����
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
        if (target_character != null) 
        {
            StartCoroutine(Set_line(target_character.gameObject.transform.position));
        }
    }

    private void Awake()
    {
        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        ActionManager.enemy_skillcard_deactivate += Card_diactivated;
        ActionManager.battle_ended += OnBattleEnd;
        ActionManager.character_drag_started += On_character_drag_started;
        ActionManager.character_drag_ended += On_character_drag_ended;
        ActionManager.card_destroyed += OnCardDestoryed;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
        ActionManager.enemy_skillcard_deactivate -= Card_diactivated;
        ActionManager.battle_ended -= OnBattleEnd;
        ActionManager.character_drag_started -= On_character_drag_started;
        ActionManager.character_drag_ended -= On_character_drag_ended;
        ActionManager.card_destroyed -= OnCardDestoryed;
    }

    private void Update()
    {
        if (isMouseOnThis && BattleCalcManager.instance.isUsingCard()) 
        {
            // �� ������ ī�带 ī�� ���� ������� (�巡�� Ÿ�̹� ������ ���׳��°� ����)
            BattleCalcManager.instance.set_target(card);
        }

    }

    private void OnCardDestoryed(card card) 
    {
        if (card == this.card) 
        {
            Destroy(gameObject);
        }
    }
}
