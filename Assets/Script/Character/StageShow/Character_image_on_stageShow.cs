using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Character_image_on_stageShow : MonoBehaviour, IPointerDownHandler
{
    public character_code code;

    int click_count = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;

    public void OnPointerDown(PointerEventData data)
    {
        click_count++;
        if (click_count == 1) clicktime = Time.time;

        // ����Ŭ���̸�
        if (click_count > 1 && Time.time - clicktime < clickdelay)
        {
            click_count = 0;
            clicktime = 0;
            // ĳ���͸� ��Ƽ�� �߰�
            PartyManager.instance.add_character_to_party(code);
            // ĳ���� ���ε�
            StageManager.instance.Reload_characters();

        }
        else if (click_count > 2 || Time.time - clicktime > clickdelay) click_count = 1; clicktime = Time.time;

    }

}
