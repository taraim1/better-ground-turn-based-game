using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card_description : MonoBehaviour
{
    private card target_card; // ī�� ������ ������ ī��
    private GameObject target_card_obj; // �� ī���� ������Ʈ
    private float target_card_show_elapsed_time = 0; // ī�� ������ �����ְ� ���� �ð�
    [SerializeField] private Vector3 offset; // ī��� �󸶳� �������־�� �ϴ��� ����
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

            // ī�� ���̶���Ʈ �Ǵ� ��
            if (target_card_show_elapsed_time <= 0.2f)
            {
                current_offset = offset * target_card_show_elapsed_time / 0.2f;
            }
            // ī�� ���̶���Ʈ �� ��
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
