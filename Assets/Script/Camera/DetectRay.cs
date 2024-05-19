using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRay : MonoBehaviour
{
    // ������Ʈ Ŭ�� ����
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        { 
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D clicked_collider = Physics2D.OverlapPoint(pos);

            if (clicked_collider != null) 
            {
                GameObject gameObj = clicked_collider.gameObject;
                // �������� ĳ���� Ŭ�� �� �� ĳ������ �� ������
                if (gameObj.tag == "PlayerCharacter") 
                {
                    CardManager.instance.Change_active_hand(gameObj.GetComponent<Character_Obj>().Character_index);
                }
                // �������� ī�� Ŭ�� �� �� ī�� ���̶���Ʈ
                if (gameObj.tag == "SkillCard")
                {

                    CardManager.instance.highlighted_card = gameObj.GetComponent<card>();
                    // ��� ī�带 ���� order��
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++) 
                    {
                        for (int j = 0; j < BattleManager.instance.hand_data[i].Count; j++)
                        {
                            BattleManager.instance.hand_data[i][j].GetComponent<element_order>().Return_to_origin_order();
                        }
                    }
                    // ���̶���Ʈ�� ī�尡 ���� ������ ������ �ϱ�
                    gameObj.GetComponent<element_order>().Set_Most_front_order();

                    // ī�� ��ġ ��� �� ����
                    for (int i = 0; i < BattleManager.instance.hand_data.Count; i++)
                    {
                        CardManager.instance.Aline_cards(i);
                    }
                }
                else 
                {
                    CardManager.instance.highlighted_card = null;
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
    }
}
