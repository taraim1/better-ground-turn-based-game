using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class enemy_skillCard_slot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SpriteRenderer illust;
    public GameObject card_obj;
    public GameObject enemy_Obj;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Image frame;

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
        // ���� Ŭ���� �ϰ� ���� ����� �÷��̾� ��ų ��� ������� �۵���
        if (eventData.button == PointerEventData.InputButton.Left && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase)
        {
            // �� ī�� ���� ����
            BattleEventManager.Trigger_event("enemy_skill_card_deactivate");
            // �� ������ ī�带 Ȱ��ȭ ��ġ��
            CardManager.instance.highlight_enemy_card(card_obj);
        }
    }

    // �� UI ���� ���콺�� ���
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (BattleCalcManager.instance.IsUsingCard) // ī�� ��� ���̸�
        {
            // �� ������ ī�带 ī�� ���� �������
            BattleCalcManager.instance.set_target(card_obj.GetComponent<card>());
            // �� ī�� ���� ����
            BattleEventManager.Trigger_event("enemy_skill_card_deactivate");
            // �� ������ ī�带 Ȱ��ȭ ��ġ��
            CardManager.instance.highlight_enemy_card(card_obj);
        }

    }

    // �� UI ���� ���콺�� ����ٰ� ������
    public void OnPointerExit(PointerEventData eventData)
    {
        if (BattleCalcManager.instance.IsUsingCard) // ī�� ��� ���̸�
        {
            // Ÿ�� ���� ����
            BattleCalcManager.instance.clear_target();
            // �� ī�� ���� ����
            BattleEventManager.Trigger_event("enemy_skill_card_deactivate");
        }

    }

    // ��ų�� ���Ǹ� ���۵Ǹ� �����
    private void skill_used() 
    {
        // ���߿� ä���� ����
    }


    private void Awake()
    {
        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        BattleEventManager.skill_used += skill_used;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
        BattleEventManager.skill_used -= skill_used;
    }
}
