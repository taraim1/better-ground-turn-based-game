using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class enemy_skillCard_slot : MonoBehaviour, IPointerClickHandler, IPointerUpHandler
{
    public Image illust;
    public GameObject card_obj;
    [SerializeField] private LineRenderer lineRenderer;

    // ��ġ �� ���� �ָ� ���η������� �׷���
    public IEnumerator Set_line(Vector3 target) 
    {
        yield return new WaitForSeconds(0.001f); // ���̾ƿ��׷� ���� �ð� �̽������� �ణ ������Ŵ
        Vector3 objPos = transform.position;
        lineRenderer.SetPosition(0, new Vector3(objPos.x, objPos.y, 1f));
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
    public void OnPointerClick(PointerEventData eventData) 
    {
        // ���� Ŭ���� �ϸ�
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // �� ī�� ���� ����
            BattleEventManager.Trigger_event("enemy_skill_card_deactivate");
            // �� ������ ī�带 Ȱ��ȭ ��ġ��
            CardManager.instance.highlight_enemy_card(card_obj);
        }
    }

    // �� UI ������ ���콺 Ŭ���� �������� ��
    public void OnPointerUp(PointerEventData eventData) 
    { 
    
    }

    private void Awake()
    {
        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;

    }
}
