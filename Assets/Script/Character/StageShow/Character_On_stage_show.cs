using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character_On_stage_show : MonoBehaviour
{
    Vector2 offset;
    Vector2 originPos;


    private void OnMouseDrag()
    {      
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        transform.position = mousePos + offset;
    }

    private void OnMouseDown()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        originPos = transform.position;
        offset = new Vector2(transform.position.x - mousePos.x, transform.position.y - mousePos.y);
        ActionManager.character_drag_started_on_stageShow?.Invoke();
    }

    private void OnMouseUp() 
    {
        ActionManager.character_drag_finished_on_stageShow?.Invoke();
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);

        transform.position = originPos;

        foreach (Collider2D collider in colliders) 
        {
            // 제거 박스 아니면 실행 종료
            if (collider.gameObject.GetComponent<Character_delete_box>() == null) { continue; }

            //  제거 박스면 캐릭터 제거

            PartyManager.instance.remove_character_from_party(gameObject.GetComponent<Character>().Code);
            StageManager.instance.Reload_characters(); 
        }

    }

}
