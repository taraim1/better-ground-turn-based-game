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

    void Skill_Display() // ���� ��ų ī�带 ǥ���ϴ� �޼���
    {
        foreach (KeyValuePair<skillcard_code, CardData> entry in cardsSO.CardData_dict) // Dict���� ������ ��������
        {
            skillcard_code code = entry.Key;
            CardData cardData = entry.Value;

            // ������ �ν��Ͻ�ȭ
            GameObject Obj = Instantiate(Skill_Scr_Btn_prefab, Scroll_content.transform);

            // �ν��Ͻ�ȭ�� ������Ʈ���� Skill_UI ������Ʈ�� ��������
            Skill_UI skill_UI = Obj.GetComponent<Skill_UI>();

            // skill_UI�� null�� �ƴ��� Ȯ��
            if (skill_UI != null)
            {
                skill_UI.Skill_UI_Set(code);
            }
            else
            {
                Debug.LogError("�ν��Ͻ�ȭ�� �����տ��� Skill_UI ������Ʈ�� ã�� �� �����ϴ�!");
            }

            // CardData�� ���� ���
            Debug.Log($"ī�� �̸�: {cardData.Name}, ���: {cardData.Cost}, Ÿ��: {cardData.Type}");

            // ī���� ��Ÿ �Ӽ��鿡 ����
            Debug.Log($"�ൿ Ÿ��: {cardData.BehaviorType}, ����: {cardData.Level}");
        }
    }

}
