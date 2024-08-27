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

        // 우선적으로 감지하는 레이어들 감지
        foreach (int mask in layerMasks) 
        {
            collider = Physics2D.OverlapPoint(MousePos, mask);
            if (collider != null)
            {
                return collider.gameObject;
            }
        }

        // 그 외 클릭 감지
        collider = Physics2D.OverlapPoint(MousePos);
        if (collider != null)
        {
            return collider.gameObject;
        }

        return null;
    }

    void Update()
    {
        // 클릭 or 터치 감지, 현재 페이즈가 플레이어 스킬 사용 페이즈여만 작동됨
        if (Input.GetMouseButtonDown(0) && BattleManager.instance.current_phase == BattleManager.phases.player_skill_phase)
        {
            GameObject gameObj = check_clicked_obj(); // 클릭된 오브젝트 감지

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

        // 모든 카드 정렬
        for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
        {
            CardManager.instance.Align_cards(i);
        }

        // 적 카드 강조 해제
        ActionManager.enemy_skillcard_deactivate?.Invoke();

        // 모든 카드를 원래 order로
        CardManager.instance.Set_origin_order(CardManager.instance.active_index);

        // 카드 위치 계산 및 정렬
        CardManager.instance.Align_cards(CardManager.instance.active_index);
    }
}
