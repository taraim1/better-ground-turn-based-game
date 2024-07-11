using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]

// TMP link 기능 사용하려면 넣는 코드, ActionManager의 액션을 일으켜줌
public class TMP_LinkHandler : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text TMPtext;
    private Canvas canvas;
    public Camera cam;

    private void Awake()
    {
        TMPtext = GetComponent<TMP_Text>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            cam = null;
        }
        else
        {
            cam = canvas.worldCamera;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 mousePos = new Vector3(eventData.position.x, eventData.position.y, 0);

        int linkTaggedText = TMP_TextUtilities.FindIntersectingLink(TMPtext, mousePos, cam);

        if (linkTaggedText != -1)
        {
            TMP_LinkInfo linkInfo = TMPtext.textInfo.linkInfo[linkTaggedText];
            ActionManager.TMP_link_clicked?.Invoke(linkInfo.GetLinkID());
        }


    }

}
