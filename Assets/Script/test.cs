using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    GameObject cardPrefab;

    // 카드 만드는 법

    void Start()
    {
        // 1. 카드 코드 enum 순회
        foreach (skillcard_code code in Enum.GetValues(typeof(skillcard_code))) 
        { 
            // 2. 카드 데이터 가져오기
            CardData data = CardManager.instance.getData_by_code(code);

            // 3. 카드 언락 여부 확인
            if (!data.IsUnlocked) continue;

            // 4. 카드 오브젝트 만들기 (카드 프리팹에 카드 스크립트 있어야 함)
            var cardObj = Instantiate(cardPrefab);

            // 5. 카드 오브젝트의 card컴포넌트에 접근
            card card = cardObj.GetComponent<card>();

            // 6. 카드 초기화 (Setup 두 번째 인자는 그냥 0 넣으십쇼)
            card.Setup(data, 0);
        }


    }

}
