using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카드 교체하려 할 때 생성되는거
public class deckCell_temp_card_module : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
}
