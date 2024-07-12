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

            if (contentObject == null)
            {
                Debug.LogWarning("Canvas 내에서 Content GameObject를 찾을 수 없습니다. 해당 이름의 GameObject가 존재하지 않거나 비활성화 상태일 수 있습니다.");
            }
        }
        else
        {
            Debug.LogWarning("Canvas를 찾을 수 없습니다. BG 스크립트가 Canvas 하위에 있어야 합니다.");
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
        else
        {
            Debug.LogWarning("Content GameObject가 설정되지 않았습니다. 활성화할 GameObject를 찾을 수 없습니다.");
        }
    }
}
