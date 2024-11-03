using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree 
{
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
            targetSelectTree.Reset();
            targetSelectTree.Process();
            return cards;

        }

    }
}
