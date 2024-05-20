using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRay : MonoBehaviour
{
    // ���̶���Ʈ�� ī�带 Ŭ������ �� �� Ŭ���� ���� ���̶���Ʈ�� ī�� ����
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

            // ������Ʈ Ŭ�� ����
            if (clicked_collider != null) 
            {
                GameObject gameObj = clicked_collider.gameObject;

                // �������� ĳ���� Ŭ�� �� �� ĳ������ �� ������
                if (gameObj.tag == "PlayerCharacter") 
                {
                    CardManager.instance.highlighted_card = null;
                    CardManager.instance.Change_active_hand(gameObj.GetComponent<Character_Obj>().Character_index);
                }
                // �������� ī�� Ŭ�� ��
                else if (gameObj.tag == "SkillCard")
                {

                    // ī�� �巡�� ���� ����
                    StartCoroutine(gameObj.GetComponent<card>().detect_drag());

                    // ��� ī�带 ���� order�� 
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++) 
                    {
                        for (int j = 0; j < BattleManager.instance.hand_data[i].Count; j++)
                        {
                            BattleManager.instance.hand_data[i][j].GetComponent<element_order>().Return_to_origin_order();
                        }
                    }
                    // ���̶���Ʈ�� ī�尡 ���� ������ ������ �ϱ�
                    if (CardManager.instance.highlighted_card != null) 
                    {
                        CardManager.instance.highlighted_card.gameObject.GetComponent<element_order>().Set_highlighted_order();
                    }


                    // ī�� ��ġ ��� �� ����

                    CardManager.instance.Aline_cards(CardManager.instance.active_index);

                }
                else // �� �̿��� �� Ŭ����
                {
                    CardManager.instance.highlighted_card = null;
                    CardManager.instance.Change_active_hand(-1);

                    // ��� ī�� ����
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
                    {
                        CardManager.instance.Aline_cards(i);
                    }
                    // ��� ī�带 ���� order��
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
                    {
                        for (int j = 0; j < BattleManager.instance.hand_data[i].Count; j++)
                        {
                            BattleManager.instance.hand_data[i][j].GetComponent<element_order>().Return_to_origin_order();
                        }
                    }

                    // ī�� ��ġ ��� �� ����
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

            // ������Ʈ Ŭ�� ����
            if (clicked_collider != null)
            {
                GameObject gameObj = clicked_collider.gameObject;
      
                // �������� ī�� Ŭ�� �� �� ī�� ���̶���Ʈ or ���̶���Ʈ ����
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

                    // ��� ī�带 ���� order�� 
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
                    {
                        for (int j = 0; j < BattleManager.instance.hand_data[i].Count; j++)
                        {
                            BattleManager.instance.hand_data[i][j].GetComponent<element_order>().Return_to_origin_order();
                        }
                    }
                    // ���̶���Ʈ�� ī�尡 ���� ������ ������ �ϱ�
                    if (CardManager.instance.highlighted_card != null)
                    {
                        CardManager.instance.highlighted_card.gameObject.GetComponent<element_order>().Set_highlighted_order();
                    }
                    // ī�� ��ġ ��� �� ����

                    CardManager.instance.Aline_cards(CardManager.instance.active_index);

                }
                
            }
        }
    }
}
