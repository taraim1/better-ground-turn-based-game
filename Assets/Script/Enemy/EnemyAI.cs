using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    // ī�� �����Ͱ� ��� ��ũ���ͺ� ������Ʈ
    [SerializeField] CardsSO cardsSO;

    // ���� ĳ���� ������
    public Character enemy;
    // ���� ��ų ���� ������
    public GameObject skill_slot_prefab;
    // ���� ���� �Ͽ� �� ī�� ������Ʈ�� ���� ����Ʈ
    public List<GameObject> using_skill_Objects;

    public GameObject layoutGroup;
    // �̹� ���� ��ų ������ ���� ����Ʈ
    public List<GameObject> skill_slots = new List<GameObject>();

    private bool isBattleEnded;

    // ���� �Ͽ� ����� ��ųī�� �ڵ尡 ���� ����Ʈ
    List<skillcard_code> current_turn_use_cards = new List<skillcard_code>();

    // �ൿƮ����

    // Process�� ī�� �ϳ��� ���ؼ� current_turn_use_cards�� �־��ִ� Ʈ��
    private BehaviorTree.BehaviorTree CardSelectTree;
   

    // ���� �̹� �Ͽ� �� ��ų ī�带 ����
    private void select_skillCard(int skill_use_count)
    {
        
        for (int i = 0; i < skill_use_count; i++)
        {
            // ��ų �ϳ� �߰�
            CardSelectTree.Reset();
            BehaviorTree.Node.Status status = CardSelectTree.Process();

            if (status == BehaviorTree.Node.Status.Failure) 
            {
                print("��ųī�� ���� �������� ���� �߻�");
            }
        }

    }

    public void clear_skills()
    {
        skill_slots.Clear();
        foreach (GameObject card_obj in using_skill_Objects)
        {
            Destroy(card_obj);
        }
        using_skill_Objects.Clear();
    }

    // �̹� ���� ��ų�� �����ϴ� �Ѱ� �޼ҵ�
    private void set_skill()
    {
        // ���� �������� �۵� X
        if (isBattleEnded) { return; }

        current_turn_use_cards.Clear();

        // ���� �� ��ų �� ����
        clear_skills();

        // �д��� �ƴϸ� ��ųī�� ����� �� ��
        if (!enemy.data.isPanic)
        {
            select_skillCard(1);
        }


        for (int i = 0; i < current_turn_use_cards.Count; i++)
        {
            // �̹� �Ͽ� �� ī��� ��ų ���� ����
            GameObject slot = Instantiate(skill_slot_prefab, layoutGroup.transform);
            GameObject card_obj = CardManager.instance.Summon_enemy_card(current_turn_use_cards[i], gameObject);
            card card = card_obj.GetComponent<card>();
            slot.GetComponent<enemy_skillCard_slot>().card_obj = card_obj;
            slot.GetComponent<enemy_skillCard_slot>().enemy_Obj = gameObject;
            slot.GetComponent<enemy_skillCard_slot>().illust.sprite = card.illust.sprite;

            // ī�� Ÿ�� ���ϱ�
            switch (card._Card.behavior_type)
            {
                case "����":
                    int rand = UnityEngine.Random.Range(0, BattleManager.instance.playable_characters.Count);
                    card.target = BattleManager.instance.playable_characters[rand];
                    // ���η����� ����
                    Vector3 targetpos = card.target.transform.position;
                    StartCoroutine(slot.GetComponent<enemy_skillCard_slot>().Set_line(new Vector3(targetpos.x, targetpos.y, -2f)));
                    // Ÿ�� ������Ʈ ����
                    slot.GetComponent<enemy_skillCard_slot>().target_obj = card.target;
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
            // ���� �̹� �Ͽ� ���� ��ü ī�尡 ���� �Ǵ� ����Ʈ
            BattleManager.instance.enemy_cards.Add(card);
        }

        // ��ų ���� �Ϸ� ī��Ʈ ����
        BattleManager.instance.enemy_skill_set_count += 1;
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

    // �Ʊ� ĳ���Ͱ� �׾��� �� �� ĳ���͸� Ÿ�����ϰ� �־��� ��ų�� ����
    private void OnPlayerCharacterDied() 
    {
        StartCoroutine(check_dead_target());
    }

    // ���� ������
    private void OnBattleEnd(bool victory)
    {
        isBattleEnded = true;
    }

    private IEnumerator check_dead_target() 
    {
        yield return new WaitForSeconds(0.01f);// ĳ���� ������Ʈ ��Ȱ��ȭ�� ��� ��ٸ�

        for (int i = using_skill_Objects.Count - 1; i >= 0; i--)
        {
            card card = using_skill_Objects[i].GetComponent<card>();

            try
            {
                Transform tmp = card.target.transform;
            }
            catch (MissingReferenceException e)
            {
                CardManager.instance.Destroy_card(card);
            }
        }
        yield break;
    }

    private void Awake()
    {
        ActionManager.enemy_skill_setting_phase += set_skill;
        ActionManager.enemy_skill_card_deactivate += return_card;
        ActionManager.player_character_died += OnPlayerCharacterDied;
        ActionManager.battle_ended += OnBattleEnd;

        isBattleEnded = false;

        CardSelectTree = new BehaviorTree.BehaviorTree("CardSelectTree");
        CardSelectTree.AddChild(new BehaviorTree.Leaf("RandomSelect", new BehaviorTree.Random_Card_Pick_Strategy(enemy, current_turn_use_cards)));
    }

    private void OnDisable()
    {
        ActionManager.enemy_skill_setting_phase -= set_skill;
        ActionManager.enemy_skill_card_deactivate -= return_card;
        ActionManager.player_character_died -= OnPlayerCharacterDied;
        ActionManager.battle_ended -= OnBattleEnd;
    }
}

