using UnityEngine;
using UnityEngine.UI;

public class BG : MonoBehaviour
{
    private GameObject contentObject; // Content GameObject에 대한 참조 변수

    void Start()
    {
        // Canvas 내에서 Content GameObject 찾기
        Canvas parentCanvas = GetComponentInParent<Canvas>(); // 현재 GameObject가 속한 Canvas 가져오기
        if (parentCanvas != null)
        {
            // Canvas 하위에서 비활성화된 GameObject 찾기
            Transform[] children = parentCanvas.GetComponentsInChildren<Transform>(true); // true를 넘겨서 비활성화된 GameObject도 포함
            foreach (Transform child in children)
            {
                if (child.name == "Content")
                {
                    contentObject = child.gameObject;
                    break;
                }
            }
        }

        // 버튼 찾기 및 클릭 이벤트 연결
        Button button = GetComponentInChildren<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        // Content GameObject 활성화
        if (contentObject != null)
        {
            contentObject.SetActive(true);
        }
    }
}
