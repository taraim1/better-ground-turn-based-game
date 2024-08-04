using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using BehaviorTree;
using static UnityEngine.EventSystems.EventTrigger;
using Unity.IO.LowLevel.Unsafe;

public enum EnemyAiType
{
    NoAI,
    RandomAI
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
        protected BehaviorTree targetSelectTree;

        public SkillSelectTree(string name, EnemyCharacter character) : base(name)
        {
            this.character = character;
            targetSelectTree = new BehaviorTree("targetSelcetTree");
        }

        public virtual List<card> Get_skills_for_current_turn() { return new List<card>(); }

        protected card Create_card(skillcard_code code, Character character) 
        {
            GameObject card_obj = CardManager.instance.Summon_enemy_card(code, character);
            return card_obj.GetComponent<card>();
        }
    }

    public class MoveTree_NoAI : MoveTree 
    {
        public MoveTree_NoAI(string name, EnemyCharacter character) : base(name, character) { }
    }

    public class SkillSelectTree_NoAI : SkillSelectTree 
    {
        public SkillSelectTree_NoAI(string name, EnemyCharacter character) : base(name, character) { }
    }

    public class SkillSelectTree_RandomPickOne : SkillSelectTree
    {
        List<skillcard_code> codes = new List<skillcard_code>();
        List<card> cards = new List<card>();
        public SkillSelectTree_RandomPickOne(string name, EnemyCharacter character) : base(name, character) 
        {
            AddChild(new Leaf("RandomSelect", new Random_Card_Pick_Strategy(character, codes)));
            targetSelectTree.AddChild(new Leaf("targetRandomSelect", new Random_Card_target_Strategy(cards)));
        }

        public override List<card> Get_skills_for_current_turn() 
        {
            codes.Clear();
            cards.Clear();
            Process();
            foreach (skillcard_code code in codes) 
            {
                cards.Add(Create_card(code, character));
            }
            targetSelectTree.Process();
            return cards;

        }

    }
}

public class EnemyAI
{
    protected EnemyCharacter character;

    private MoveTree MoveTree;
    private SkillSelectTree SkillSelectTree;

    public EnemyAI(EnemyCharacter character, MoveTree moveTree, SkillSelectTree skillSelectTree)
    {
        this.character = character;
        MoveTree = moveTree;
        SkillSelectTree = skillSelectTree;
    }

    public void Move() => MoveTree.Move();
    public List<card> Get_skills_for_current_turn() => SkillSelectTree.Get_skills_for_current_turn();
}



