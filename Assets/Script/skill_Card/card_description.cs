using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card_description : MonoBehaviour
{
    private card target_card; // 카드 설명을 보여줄 카드
    private GameObject target_card_obj; // 그 카드의 오브젝트
    private float target_card_show_elapsed_time = 0; // 카드 설명을 보여주고 지난 시간
    [SerializeField] private Vector3 offset; // 카드와 얼마나 떨어져있어야 하는지 저장
    Vector3 current_offset;

    public void Set_target(card target_card) 
    { 
        this.target_card = target_card;
        target_card_obj = target_card.gameObject;
        target_card_show_elapsed_time = 0;
    }

    public void Clear_target() 
    {
        target_card = null;
        target_card_obj = null;
        transform.position = new Vector3(-13, 0, 0);
    }

    private void Update()
    {
        if (target_card != null)
        {
            target_card_show_elapsed_time += Time.deltaTime;

            // 카드 하이라이트 되는 중
            if (target_card_show_elapsed_time <= 0.2f)
            {
                current_offset = offset * target_card_show_elapsed_time / 0.2f;
            }
            // 카드 하이라이트 다 됨
            else 
            {
                current_offset = offset;
            }

            transform.position = target_card_obj.transform.position + current_offset;
            transform.rotation = target_card_obj.transform.rotation;
            transform.localScale = target_card_obj.transform.localScale;

        }

    }


}
