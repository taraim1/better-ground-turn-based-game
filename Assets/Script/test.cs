using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Runtime.CompilerServices;

public class test : MonoBehaviour
{
    void asdf()
    {
        /*
        캐릭터 불러오는 법

        ========== 1. 캐릭터 인스턴스를 담을 오브젝트 만들기 (캐릭터 클래스가 모노비헤이비어 상속받고 있어서 그냥은 못 만듦) ==========

        미리 만들어놓기 or 스크립트로 만들기 택일

        Character 스크립트 달아주면 됨

        ========== 2. 캐릭터 데이터 불러오기 ==========

        Character 캐릭터 = 만든_오브젝트.GetComponent<Character>();
        ㄴ 아까 만든 오브젝트의 캐릭터 인스턴스를 참조하기

        JsonUtility.FromJsonOverwrite(CharacterManager.instance.load_character_from_json(캐릭터 코드), 캐릭터);
        ㄴ 캐릭터 인스턴스의 데이터를 Json에서 불러와서 덮어씌움
        ㄴ 캐릭터 코드 예시) CharacterManager.character_code.kimchunsik (이거 방식 조금 바꿀거긴 한데 일단 지금은 이럼)
        ㄴ 정확한 작동 방식 필요하면 알려드림 (아니면 CharacterManager 스크립트에 있는 거 봐도 됨)

        ========== 3. 캐릭터 데이터에 접근하기 ==========
        자세한 게 필요하면 Character 스크립트로 ㄱㄱ
        아니면 나한테 질문 ㄱ


        >> 캐릭터 인스턴스에서 퍼블릭으로 접근할 수 있는 것들 <<
        캐릭터.name 이런 식으로 접근가능


        이름
        캐릭터 설명
        캐릭터 코드
        적 코드 (적도 캐릭터 취급이라 같이 있음)
        레벨
        캐릭터 언락 유무
        덱
        스펌 데이터 저장 경로
        전투에서 쓰이는 현재 체력, 캐릭터 번호 등의 정보들 <--- 이건 딱히 쓸모 없을거고

        -----------------------------------------------

        >> 프라이빗이라 메소드로 접근해야 하는 거 <<

        최대 체력, 정신력
        ㄴ 캐릭터.get_max_health_of_level(캐릭터.level) 이런 식으로 하면 줌


        */


    }
}


