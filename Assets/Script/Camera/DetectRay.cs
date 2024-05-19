using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRay : MonoBehaviour
{
    // 오브젝트 클릭 감지
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        { 
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D clicked_collider = Physics2D.OverlapPoint(pos);

            if (clicked_collider != null) 
            {
                GameObject gameObj = clicked_collider.gameObject;
                // 전투에서 캐릭터 클릭 시 그 캐릭터의 패 보여줌
                if (gameObj.tag == "PlayerCharacter") 
                {
                    CardManager.instance.Change_active_hand(gameObj.GetComponent<Character_Obj>().Character_index);
                }
                // 전투에서 카드 클릭 시 그 카드 하이라이트
                if (gameObj.tag == "SkillCard")
                {

                    CardManager.instance.highlighted_card = gameObj.GetComponent<card>();
                    // 모든 카드를 원래 order로
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++) 
                    {
                        for (int j = 0; j < BattleManager.instance.hand_data[i].Count; j++)
                        {
                            BattleManager.instance.hand_data[i][j].GetComponent<element_order>().Return_to_origin_order();
                        }
                    }
                    // 하이라이트된 카드가 가장 앞으로 나오게 하기
                    gameObj.GetComponent<element_order>().Set_Most_front_order();

                    // 카드 위치 계산 및 정렬
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
                    {
                        CardManager.instance.Aline_cards(i);
                    }
                }
                else 
                {
                    CardManager.instance.highlighted_card = null;
                    // 모든 카드 정렬
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
                    {
                        CardManager.instance.Aline_cards(i);
                    }
                    // 모든 카드를 원래 order로
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
                    {
                        for (int j = 0; j < BattleManager.instance.hand_data[i].Count; j++)
                        {
                            BattleManager.instance.hand_data[i][j].GetComponent<element_order>().Return_to_origin_order();
                        }
                    }

                    // 카드 위치 계산 및 정렬
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
                    {
                        CardManager.instance.Aline_cards(i);
                    }
                }
            }
        }
    }
}
