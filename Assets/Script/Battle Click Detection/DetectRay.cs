using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRay : MonoBehaviour
{

    List<int> layerMasks = new List<int>();


    private void Awake()
    {
        ActionManager.battle_ended += OnBattleEnd;
        layerMasks.Add(LayerMask.GetMask("SkillCard"));
        layerMasks.Add(LayerMask.GetMask("skillslot"));
        layerMasks.Add(LayerMask.GetMask("character"));
    }

    private void OnDestroy()
    {
        ActionManager.battle_ended -= OnBattleEnd;
    }

    private void OnBattleEnd(bool victory) 
    {
        Destroy(this);
    }

    private GameObject check_clicked_obj() 
    {
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider;

        // �켱������ �����ϴ� ���̾�� ����
        foreach (int mask in layerMasks) 
        {
            collider = Physics2D.OverlapPoint(MousePos, mask);
            if (collider != null)
            {
                return collider.gameObject;
            }
        }

        // �� �� Ŭ�� ����
        collider = Physics2D.OverlapPoint(MousePos);
        if (collider != null)
        {
            return collider.gameObject;
        }

        return null;
    }

    void Update()
    {
        // Ŭ�� or ��ġ ����, ���� ����� �÷��̾� ��ų ��� ������� �۵���
        if (Input.GetMouseButtonDown(0) && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase)
        {
            GameObject gameObj = check_clicked_obj(); // Ŭ���� ������Ʈ ����

            if (gameObj == null)
            {
                OnOtherClicked();
                return; 
            }

            Iclickable iclickable = gameObj.GetComponent<Iclickable>();

            if (iclickable == null) 
            {
                OnOtherClicked();
                return; 
            }

            iclickable.OnClick();
            
        }
    }

    private void OnOtherClicked() 
    {
        CardManager.instance.clear_highlighted_card();
        CardManager.instance.Change_active_hand(-1);

        // ��� ī�� ����
        for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
        {
            CardManager.instance.Align_cards(i);
        }

        // �� ī�� ���� ����
        ActionManager.enemy_skillcard_deactivate?.Invoke();

        // ��� ī�带 ���� order��
        CardManager.instance.Set_origin_order(CardManager.instance.active_index);

        // ī�� ��ġ ��� �� ����
        CardManager.instance.Align_cards(CardManager.instance.active_index);
    }
}
