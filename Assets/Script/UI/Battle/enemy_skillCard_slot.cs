using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEditor;

public class enemy_skillCard_slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image illust;
    public GameObject card_obj;
    public GameObject enemy_Obj;
    public GameObject target_obj;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Image frame;

    Color red = new Color(1f, 0f, 0f, 1f);
    Color grey = new Color(0.7f, 0.7f, 0.7f, 1f);

    public Coroutine running_drag = null;

    // PCȯ�� ������ ���� ����
    private bool isHighlightedByClick = false;
    bool isMouseOnThis = false;

    bool isBattleEnded;
    bool isDragging = false;

    private List<Tuple<int, int>> current_range;

    // ���� ��������
    private void OnBattleEnd(bool victory) 
    {
        isBattleEnded = true;
    }

    // ��ġ�� �ָ� ���η������� �׷���
    public IEnumerator Set_line(Vector3 target) 
    {
        yield return new WaitForSeconds(0.001f); // ���̾ƿ��׷� ���� �ð� �̽������� �ణ ������Ŵ

        card using_card = card_obj.GetComponent<card>();
        Character target_character = target_obj.GetComponent<Character>();
        Character using_character = using_card.owner.GetComponent<Character>();

        // ���� ����
        set_lineRenderer_color(red);
        if (using_card.Data.RangeType == CardRangeType.limited) 
        {

            if (!BattleCalcManager.instance.check_limited_range_usable(target_character.get_coordinate(), using_card.get_use_range(using_character.get_coordinate()))) 
            {
                set_lineRenderer_color(grey);
            }
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
        if (isBattleEnded) { return; }

        // ���� ����� �÷��̾� ��ų ��� ������� �۵���
        if (BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase)
        {
            // �� ī�� ���� ����
            ActionManager.enemy_skillData_deactivate?.Invoke();
            // �� ������ ī�带 Ȱ��ȭ ��ġ��
            CardManager.instance.highlight_enemyData(card_obj);
            isHighlightedByClick = true;
        }
    }

    // �� UI ���� ���콺�� ��� (����ϻ󿡼� �巡�� �߿�)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isBattleEnded) { return; }

        if (BattleCalcManager.instance.IsUsingCard) // ī�� ��� ���̸�
        {
            // �� ������ ī�带 ī�� ���� �������
            BattleCalcManager.instance.set_target(card_obj.GetComponent<card>());
            // �� ī�� ���� ����
            ActionManager.enemy_skillData_deactivate?.Invoke();
            // �� ������ ī�带 Ȱ��ȭ ��ġ��
            CardManager.instance.highlight_enemyData(card_obj);
            isHighlightedByClick = false;
        }

        isMouseOnThis = true;

    }

    // �巡�� �� �� UI ���� ���Դ� ������
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isBattleEnded) { return; }

        if (!isHighlightedByClick) // Ŭ������ ���̶���Ʈ�� �� �ƴϸ�
        {
            // Ÿ�� ���� ����
            BattleCalcManager.instance.clear_target_card();
            // �� ī�� ���� ����
            ActionManager.enemy_skillData_deactivate?.Invoke();
        }

        isMouseOnThis = false;
    }

    // �巡�� ����
    public IEnumerator detect_drag()
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
                    foreach (Tuple<int, int> coordinate in current_range)
                    {
                        BattleGridManager.instance.set_tile_color(coordinate.Item1, coordinate.Item2, Tile.TileColor.original);
                    }
                }
                isDragging = false;
                yield break;
            }

            // ���콺�� �� �� ���·� ���� �ð��� ������ �巡�� ��� ����
            if (dragging_time >= Util.drag_time_standard && !isDragging)
            {
                // �� ī�� ���� ����
                ActionManager.enemy_skillData_deactivate?.Invoke();

                // ��ų �����Ÿ� ���̱�
                if (card.Data.RangeType == CardRangeType.limited) 
                {
                    current_range = card.get_use_range(card.owner.GetComponent<Character>().get_coordinate());
                    foreach (Tuple<int, int> coordinate in current_range) 
                    {
                        BattleGridManager.instance.set_tile_color(coordinate.Item1, coordinate.Item2, Tile.TileColor.red);
                    }
                }
                isDragging = true;
            }

            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);

        }
    }

    // ��ų�� ���Ǹ� �����
    private void skill_used() 
    {
        // ���߿� ä���� ����
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
        if (target_obj != null) 
        {
            StartCoroutine(Set_line(target_obj.transform.position));
        }
    }

    private void Awake()
    {
        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        ActionManager.skill_used += skill_used;
        ActionManager.enemy_skillData_deactivate += Card_diactivated;
        ActionManager.battle_ended += OnBattleEnd;
        ActionManager.character_drag_started += On_character_drag_started;
        ActionManager.character_drag_ended += On_character_drag_ended;

        isBattleEnded = false;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
        ActionManager.skill_used -= skill_used;
        ActionManager.enemy_skillData_deactivate -= Card_diactivated;
        ActionManager.battle_ended -= OnBattleEnd;
        ActionManager.character_drag_started -= On_character_drag_started;
        ActionManager.character_drag_ended -= On_character_drag_ended;
    }

    private void Update()
    {
        if (isMouseOnThis && BattleCalcManager.instance.IsUsingCard) 
        {
            // �� ������ ī�带 ī�� ���� ������� (�巡�� Ÿ�̹� ������ ���׳��°� ����)
            BattleCalcManager.instance.set_target(card_obj.GetComponent<card>());
        }

        // ��ų�� ��µ��� ������ �� �������� ���� ����
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
