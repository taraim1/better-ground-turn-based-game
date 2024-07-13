using UnityEngine;
using UnityEngine.EventSystems;

public class Character_Pop : MonoBehaviour, IPointerDownHandler
{
    public GameObject bgPrefab; 
    public Vector2 spawnPosition; 
    private Canvas parentCanvas; 
    private GameObject contentObject; // Content GameObject에 대한 참조 변수

    void Start()
    {
        // Canvas 찾기
        parentCanvas = FindObjectOfType<Canvas>();
        if (parentCanvas == null)
        {
            Debug.LogError("Canvas를 찾을 수 없습니다. 해당 Canvas가 존재하는지 확인해주세요.");
        }

        // Content GameObject 찾기
        contentObject = GameObject.Find("Content");
        if (contentObject == null)
        {
            Debug.LogError("Content GameObject를 찾을 수 없습니다. 해당 이름의 GameObject가 존재하는지 확인해주세요.");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (parentCanvas != null)
        {
            // bgPrefab 인스턴스화
            GameObject bgInstance = Instantiate(bgPrefab, parentCanvas.transform);
            RectTransform rectTransform = bgInstance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = spawnPosition;
            bgInstance.GetComponent<Character_popup>().setup(gameObject.GetComponent<Character>());
        }

        // Content GameObject 비활성화
        if (contentObject != null)
        {
            contentObject.SetActive(false);
        }
    }
}

