using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // ī�� �����Ͱ� ��� ��ũ���ͺ� ������Ʈ
    [SerializeField] CardsSO cardsSO;

    // ���� ĳ���� ������
    public Character enemy;
    // ���� ��ų ���� ���̾ƿ��׷� ������
    public GameObject layoutGroup_prefab;
    // ���� ��ų ���� ������
    public GameObject skill_slot_prefab;
    // ���� ���� �Ͽ� �� ī�� ������Ʈ�� ���� ����Ʈ
    public List<GameObject> using_skill_Objects;

    GameObject layoutGroup;
    // �̹� ���� ��ų ������ ���� ����Ʈ
    List<GameObject> skill_slots = new List<GameObject>();
    public GameObject canvas;

    // ���� �̹� �Ͽ� �� ��ų ī�� �ڵ� ����Ʈ�� ��ȯ�ϴ� �޼ҵ�
    private List<CardManager.skillcard_code> get_action() 
    {
        // �̹� �Ͽ� �� ��ų ����
        int skill_use_count = 1;

        List<CardManager.skillcard_code> result = new List<CardManager.skillcard_code>();

        // ������ �� ��ų �����ϰ� ����
        for (int i = 0; i < skill_use_count; i++) 
        {
            int rand = Random.Range(0, enemy.deck.Length);
            result.Add(enemy.deck[rand]);
        }

        return result;
    }

    // �̹� ���� ��ų�� �����ϴ� �Ѱ� �޼ҵ�
    private void set_skill()
    {
        skill_slots.Clear();
        foreach (GameObject card_obj in using_skill_Objects)
        {
            Destroy(card_obj);
        }
        using_skill_Objects.Clear();
        List<CardManager.skillcard_code> skill_list = get_action();

        for (int i = 0; i < skill_list.Count; i++) 
        {
            // �̹� �Ͽ� �� ī��� ��ų ���� ����
            GameObject slot = Instantiate(skill_slot_prefab, layoutGroup.transform);
            GameObject card_obj = CardManager.instance.Summon_enemy_card(skill_list[i]);
            card card = card_obj.GetComponent<card>();
            slot.GetComponent<enemy_skillCard_slot>().card_obj = card_obj;

            slot.GetComponent<enemy_skillCard_slot>().illust.sprite = card.illust.sprite;

            // ī�� Ÿ�� ���ϱ�
            switch (card.Card.behavior_type) 
            {
                case "����":
                    int rand = Random.Range(0, BattleManager.instance.playable_characters.Count);
                    card.target = BattleManager.instance.playable_characters[rand];
                    // ���η����� ����
                    Vector3 targetpos = card.target.transform.position;
                    StartCoroutine(slot.GetComponent<enemy_skillCard_slot>().Set_line(new Vector3(targetpos.x, targetpos.y, -2f)));
                    break;
                case "���":
                    card.target = gameObject;
                    break;
                case "ȸ��":
                    card.target = gameObject;
                    break;
            }


            using_skill_Objects.Add(card_obj);
            skill_slots.Add(slot);
        }
    }

    // ���� ��� ��ų ī�� ���� ����
    private void return_card() 
    {
        foreach (GameObject card_obj in using_skill_Objects) 
        {
            card card = card_obj.GetComponent<card>();
            card.MoveTransform(card.originPRS, false, 0f);
            card.state = card.current_mode.normal;
        }
    }

    private void Awake()
    {
        BattleEventManager.enemy_skill_setting_phase += set_skill;
        BattleEventManager.enemy_skill_card_deactivate += return_card;
        canvas = GameObject.Find("Canvas");
        layoutGroup = Instantiate(layoutGroup_prefab, Vector3.zero, Quaternion.identity, canvas.transform);
        layoutGroup.GetComponent<UI_hook_up_object>().target_object = gameObject;
    }

    private void OnDisable()
    {
        BattleEventManager.enemy_skill_setting_phase -= set_skill;
        BattleEventManager.enemy_skill_card_deactivate -= return_card;
    }
}
