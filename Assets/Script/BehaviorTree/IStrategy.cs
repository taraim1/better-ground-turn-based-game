using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BehaviorTree
{
    public interface IStrategy
    {
        Node.Status Process();
        void Reset();
    }

    public interface IMoveStrategy : IStrategy
    { 
        void Move();
    }

    public interface ISkillUseStrategy : IStrategy 
    {
        void Use_skill();
    }


    // 덱에서 랜덤한 카드 한 장을 고르는 전략
    // 지정한 스킬카드 코드 리스트에 코드 하나를 넣어준다.
    public class Random_Card_Pick_Strategy : IStrategy
    {
        EnemyCharacter enemyCharacter;
        List<skillcard_code> resultList;
        public Random_Card_Pick_Strategy(EnemyCharacter enemyCharacter, List<skillcard_code> resultList) 
        {
            this.enemyCharacter = enemyCharacter;
            this.resultList = resultList;

        }

        public Node.Status Process() 
        {
            // 덱에서 쓸 스킬 랜덤하게 뽑음
            try
            {
                List<skillcard_code> deck_copy = enemyCharacter.Deck;
                int rand = UnityEngine.Random.Range(0, deck_copy.Count);
                resultList.Add(deck_copy[rand]);
                return Node.Status.Sucess;
            }
            catch (Exception e)
            {
                return Node.Status.Failure;
            }

                
        }

        public void Reset() 
        {
        }
    }

    // 카드 리스트 주면 랜덤하게 타겟 지정해줌
    public class Random_Card_target_Strategy : IStrategy
    {
        List<card> cards;
        public Random_Card_target_Strategy(List<card> cards)
        {
            this.cards = cards;

        }

        public Node.Status Process()
        {
            try
            {
                foreach (card card in cards) 
                {
                    switch (card.Data.BehaviorType)
                    {
                        case "공격":
                            int rand = UnityEngine.Random.Range(0, BattleManager.instance.playable_characters.Count);
                            card.target = BattleManager.instance.playable_characters[rand];
                            break;
                    }
                }


                return Node.Status.Sucess;
            }
            catch (Exception e)
            {
                return Node.Status.Failure;
            }


        }

        public void Reset()
        {
        }
    }
}
