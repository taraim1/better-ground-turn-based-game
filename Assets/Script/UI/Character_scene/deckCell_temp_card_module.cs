using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ī�� ��ü�Ϸ� �� �� �����Ǵ°�
public class deckCell_temp_card_module : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
}
