using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerDownHandler
{
    public GameObject bgPrefab;
    public Vector2 spawnPosition;
    private Canvas parentCanvas;
    void Start()
    {
        parentCanvas = FindObjectOfType<Canvas>();

        if (parentCanvas == null)
        {
            Debug.LogError("Canvas를 찾을 수 없습니다. 씬에 Canvas가 존재하는지 확인하세요.");
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (parentCanvas != null)
        {
            // BG 프리팹을 Canvas의 자식으로 생성
            GameObject bgInstance = Instantiate(bgPrefab, parentCanvas.transform);
            // RectTransform 컴포넌트를 통해 위치 설정
            RectTransform rectTransform = bgInstance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = spawnPosition;
        }
    }
}
