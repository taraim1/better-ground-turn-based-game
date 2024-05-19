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

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if (hit.collider != null) 
            {
                GameObject gameObj = hit.collider.gameObject;
                // �������� ĳ���� Ŭ�� �� �� ĳ������ �� ������
                if (gameObj.tag == "PlayerCharacter") 
                {
                    CardManager.instance.Change_active_hand(gameObj.GetComponent<Character_Obj>().Character_index);
                }
            }
        }
    }
}
