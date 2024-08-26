using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    GameObject cardPrefab;

    // ī�� ����� ��

    void Start()
    {
        // 1. ī�� �ڵ� enum ��ȸ
        foreach (skillcard_code code in Enum.GetValues(typeof(skillcard_code))) 
        { 
            // 2. ī�� ������ ��������
            CardData data = CardManager.instance.getData_by_code(code);

            // 3. ī�� ��� ���� Ȯ��
            if (!data.IsUnlocked) continue;

            // 4. ī�� ������Ʈ ����� (ī�� �����տ� ī�� ��ũ��Ʈ �־�� ��)
            var cardObj = Instantiate(cardPrefab);

            // 5. ī�� ������Ʈ�� card������Ʈ�� ����
            card card = cardObj.GetComponent<card>();

            // 6. ī�� �ʱ�ȭ (Setup �� ��° ���ڴ� �׳� 0 �����ʼ�)
            card.Setup(data, 0);
        }


    }

}
