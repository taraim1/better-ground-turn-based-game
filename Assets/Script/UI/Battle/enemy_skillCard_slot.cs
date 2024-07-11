using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class enemy_skillCard_slot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image illust;
    public GameObject card_obj;
    public GameObject enemy_Obj;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Image frame;

    // PCȯ�� ������ ���� ����
    private bool isHighlightedByClick = false;
    bool isMouseOnThis = false;

    bool isBattleEnded;

    // ���� ��������
    private void OnBattleEnd(bool victory) 
    {
        isBattleEnded = true;
    }

    // ��ġ �� ���� �ָ� ���η������� �׷���
    public IEnumerator Set_line(Vector3 target) 
    {
        yield return new WaitForSeconds(0.001f); // ���̾ƿ��׷� ���� �ð� �̽������� �ణ ������Ŵ
        Vector3 objPos = transform.position;
        lineRenderer.SetPosition(0, new Vector3(objPos.x, objPos.y, -1f));
        lineRenderer.SetPosition(1, new Vector3(target.x, target.y, 1f));
        lineRenderer.enabled = true;
        yield break;
    }

    // ���η����� ����
    public void Remove_line() 
    {
        lineRenderer.enabled = false;
    }

    // �� UI�� ������ ��
    public void OnPointerDown(PointerEventData eventData) 
    {
        if (isBattleEnded) { return; }

        // ���� Ŭ���� �ϰ� ���� ����� �÷��̾� ��ų ��� ������� �۵���
        if (eventData.button == PointerEventData.InputButton.Left && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase)
        {
            // �� ī�� ���� ����
            ActionManager.enemy_skill_card_deactivate?.Invoke();
            // �� ������ ī�带 Ȱ��ȭ ��ġ��
            CardManager.instance.highlight_enemy_card(card_obj);
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
            ActionManager.enemy_skill_card_deactivate?.Invoke();
            // �� ������ ī�带 Ȱ��ȭ ��ġ��
            CardManager.instance.highlight_enemy_card(card_obj);
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
            ActionManager.enemy_skill_card_deactivate?.Invoke();
        }

        isMouseOnThis = false;
    }

    // ��ų�� ���Ǹ� ���۵Ǹ� �����
    private void skill_used() 
    {
        // ���߿� ä���� ����
    }

    // �� ī�� ���̶���Ʈ ������ ����
    private void Card_diactivated() 
    {
        isHighlightedByClick = false;
    }

    private void Awake()
    {
        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        ActionManager.skill_used += skill_used;
        ActionManager.enemy_skill_card_deactivate += Card_diactivated;
        ActionManager.battle_ended += OnBattleEnd;

        isBattleEnded = false;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
        ActionManager.skill_used -= skill_used;
        ActionManager.enemy_skill_card_deactivate -= Card_diactivated;
        ActionManager.battle_ended -= OnBattleEnd;
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
