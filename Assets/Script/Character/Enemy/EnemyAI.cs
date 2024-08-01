using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using BehaviorTree;

public enum EnemyAiType
{
    NoAI
}

namespace BehaviorTree 
{
    public abstract class MoveTree : BehaviorTree 
    {
        protected EnemyCharacter character; 
        public MoveTree(string name, EnemyCharacter character) : base(name) 
        {
            this.character = character;
        }

        public virtual void Move() { }
    }

    public abstract class SkillSelectTree : BehaviorTree
    {
        protected EnemyCharacter character;
        public SkillSelectTree(string name, EnemyCharacter character) : base(name)
        {
            this.character = character;
        }

        public virtual List<skillcard_code> Get_skills_for_current_turn() { return new List<skillcard_code>(); }
    }

    public class MoveTree_NoAI : MoveTree 
    {
        public MoveTree_NoAI(string name, EnemyCharacter character) : base(name, character) { }
    }

    public class SkillSelectTree_NoAI : SkillSelectTree 
    {
        public SkillSelectTree_NoAI(string name, EnemyCharacter character) : base(name, character) { }
    }
}

public class EnemyAI
{
    protected EnemyCharacter character;

    private MoveTree MoveTree;
    private SkillSelectTree SkillUseTree;

    public EnemyAI(EnemyCharacter character, MoveTree moveTree, SkillSelectTree skillSelectTree)
    {
        this.character = character;
        MoveTree = moveTree;
        SkillUseTree = skillSelectTree;
    }

    public void Move() => MoveTree.Move();
    public List<skillcard_code> Get_skills_for_current_turn() => SkillUseTree.Get_skills_for_current_turn();
}



public class temp : MonoBehaviour
{
    /*

    public GameObject layoutGroup;
    // �̹� ���� ��ų ������ ���� ����Ʈ
    public List<GameObject> skill_slots = new List<GameObject>();

    private bool isBattleEnded;

    // ���� �Ͽ� ����� ��ųī�� �ڵ尡 ���� ����Ʈ
    List<skillcard_code> current_turn_useDatas = new List<skillcard_code>();

    // �ൿƮ����

    // Process�� ī�� �ϳ��� ���ؼ� current_turn_useDatas�� �־��ִ� Ʈ��
    private BehaviorTree.BehaviorTree CardDataelectTree;
   

    // ���� �̹� �Ͽ� �� ��ų ī�带 ����
    private void select_skillCard(int skill_use_count)
    {
        
        for (int i = 0; i < skill_use_count; i++)
        {
            // ��ų �ϳ� �߰�
            CardDataelectTree.Reset();
            BehaviorTree.Node.Status status = CardDataelectTree.Process();

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

        current_turn_useDatas.Clear();

        // ���� �� ��ų �� ����
        clear_skills();

        // �д��� �ƴϸ� ��ųī�� ����� �� ��
        if (!enemy.IsPanic)
        {
            select_skillCard(1);
        }


        for (int i = 0; i < current_turn_useDatas.Count; i++)
        {
            // �̹� �Ͽ� �� ī��� ��ų ���� ����
            GameObject slot = Instantiate(skill_slot_prefab, layoutGroup.transform);
            GameObject card_obj = CardManager.instance.Summon_enemyData(current_turn_useDatas[i], gameObject);
            card card = card_obj.GetComponent<card>();
            slot.GetComponent<enemy_skillCard_slot>().card_obj = card_obj;
            slot.GetComponent<enemy_skillCard_slot>().enemy_Obj = gameObject;
            slot.GetComponent<enemy_skillCard_slot>().illust.sprite = card.illust.sprite;

            // ī�� Ÿ�� ���ϱ�
            switch (card.Data.BehaviorType)
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
    private void returnData()
    {
        foreach (GameObject card_obj in using_skill_Objects)
        {
            card card = card_obj.GetComponent<card>();
            card.MoveTransform(card.originPRS, false, 0f);
            card.state = card.current_mode.normal;
        }
    }

    // �Ʊ� ĳ���Ͱ� �׾��� �� �� ĳ���͸� Ÿ�����ϰ� �־��� ��ų�� ����
    private void OnCharacterDied(Character character) 
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
        for (int i = using_skill_Objects.Count - 1; i >= 0; i--)
        {
            card card = using_skill_Objects[i].GetComponent<card>();

            try
            {
                // ���� Ÿ���� Ʈ�������� �� ������ ����
                Transform tmp = card.target.transform;
            }
            catch (MissingReferenceException e)
            {
                CardManager.instance.DestroyData(card);
            }
        }
        yield break;
    }

    private void Awake()
    {
        ActionManager.enemy_skill_setting_phase += set_skill;
        ActionManager.enemy_skillData_deactivate += returnData;
        ActionManager.character_kill_complete += OnCharacterDied;
        ActionManager.battle_ended += OnBattleEnd;

        isBattleEnded = false;

        CardDataelectTree = new BehaviorTree.BehaviorTree("CardDataelectTree");
        CardDataelectTree.AddChild(new BehaviorTree.Leaf("RandomSelect", new BehaviorTree.RandomData_Pick_Strategy(enemy, current_turn_useDatas)));
    }

    private void OnDisable()
    {
        ActionManager.enemy_skill_setting_phase -= set_skill;
        ActionManager.enemy_skillData_deactivate -= returnData;
        ActionManager.character_kill_complete -= OnCharacterDied;
        ActionManager.battle_ended -= OnBattleEnd;
    }

    */
}

