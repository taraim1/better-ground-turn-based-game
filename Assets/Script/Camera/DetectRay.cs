using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRay : MonoBehaviour
{
    // 하이라이트된 카드를 클릭했을 때 그 클릭을 떼면 하이라이트된 카드 해제
    IEnumerator clear_highlighted_card() 
    {
        while (true) 
        {
            if (!Input.GetMouseButton(0)) 
            {
                CardManager.instance.highlighted_card = null;
                yield break;
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        { 
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D clicked_collider = Physics2D.OverlapPoint(pos);

            // 오브젝트 클릭 감지
            if (clicked_collider != null) 
            {
                GameObject gameObj = clicked_collider.gameObject;

                // 전투에서 캐릭터 클릭 시 그 캐릭터의 패 보여줌
                if (gameObj.tag == "PlayerCharacter") 
                {
                    CardManager.instance.highlighted_card = null;
                    CardManager.instance.Change_active_hand(gameObj.GetComponent<Character_Obj>().Character_index);
                }
                // 전투에서 카드 클릭 시
                else if (gameObj.tag == "SkillCard")
                {

                    // 카드 드래그 감지 시작
                    StartCoroutine(gameObj.GetComponent<card>().detect_drag());

                    // 모든 카드를 원래 order로 
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++) 
                    {
                        for (int j = 0; j < BattleManager.instance.hand_data[i].Count; j++)
                        {
                            BattleManager.instance.hand_data[i][j].GetComponent<element_order>().Return_to_origin_order();
                        }
                    }
                    // 하이라이트된 카드가 가장 앞으로 나오게 하기
                    if (CardManager.instance.highlighted_card != null) 
                    {
                        CardManager.instance.highlighted_card.gameObject.GetComponent<element_order>().Set_highlighted_order();
                    }


                    // 카드 위치 계산 및 정렬

                    CardManager.instance.Aline_cards(CardManager.instance.active_index);

                }
                else // 그 이외의 것 클릭시
                {
                    CardManager.instance.highlighted_card = null;
                    CardManager.instance.Change_active_hand(-1);

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

        if (Input.GetMouseButtonUp(0)) 
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D clicked_collider = Physics2D.OverlapPoint(pos);

            // 오브젝트 클릭 감지
            if (clicked_collider != null)
            {
                GameObject gameObj = clicked_collider.gameObject;
      
                // 전투에서 카드 클릭 시 그 카드 하이라이트 or 하이라이트 해제
                if (gameObj.tag == "SkillCard")
                {
                    if (CardManager.instance.highlighted_card != gameObj.GetComponent<card>())
                    {
                        CardManager.instance.highlighted_card = gameObj.GetComponent<card>();
                    }
                    else
                    {
                        StartCoroutine(clear_highlighted_card());
                    }

                    // 모든 카드를 원래 order로 
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
                    {
                        for (int j = 0; j < BattleManager.instance.hand_data[i].Count; j++)
                        {
                            BattleManager.instance.hand_data[i][j].GetComponent<element_order>().Return_to_origin_order();
                        }
                    }
                    // 하이라이트된 카드가 가장 앞으로 나오게 하기
                    if (CardManager.instance.highlighted_card != null)
                    {
                        CardManager.instance.highlighted_card.gameObject.GetComponent<element_order>().Set_highlighted_order();
                    }
                    // 카드 위치 계산 및 정렬

                    CardManager.instance.Aline_cards(CardManager.instance.active_index);

                }
                
            }
        }
    }
}
