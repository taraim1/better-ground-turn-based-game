using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Scene : MonoBehaviour
{
    [SerializeField]
    public CardDataSO cardsSO;
    private skillcard_code code;
    [SerializeField] private GameObject Scroll_content;
    [SerializeField] private GameObject Skill_Scr_Btn_prefab;

    // Start is called before the first frame update
    void Start()
    {
        Skill_Display();
    }

    void Skill_Display() // 씬에 스킬 카드를 표시하는 메서드
    {
        foreach (KeyValuePair<skillcard_code, CardData> entry in cardsSO.CardData_dict) // Dict에서 데이터 가져오기
        {
            skillcard_code code = entry.Key;
            CardData cardData = entry.Value;

            // 프리팹 인스턴스화
            GameObject Obj = Instantiate(Skill_Scr_Btn_prefab, Scroll_content.transform);

            // 인스턴스화된 오브젝트에서 Skill_UI 컴포넌트를 가져오기
            Skill_UI skill_UI = Obj.GetComponent<Skill_UI>();

            // skill_UI가 null이 아닌지 확인
            if (skill_UI != null)
            {
                skill_UI.Skill_UI_Set(code);
            }
            else
            {
                Debug.LogError("인스턴스화된 프리팹에서 Skill_UI 컴포넌트를 찾을 수 없습니다!");
            }

            // CardData의 정보 출력
            Debug.Log($"카드 이름: {cardData.Name}, 비용: {cardData.Cost}, 타입: {cardData.Type}");

            // 카드의 기타 속성들에 접근
            Debug.Log($"행동 타입: {cardData.BehaviorType}, 레벨: {cardData.Level}");
        }
    }

}
