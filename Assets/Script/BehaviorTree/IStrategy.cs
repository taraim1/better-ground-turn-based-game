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


    // ������ ������ ī�� �� ���� ���� ����
    // ������ ��ųī�� �ڵ� ����Ʈ�� �ڵ� �ϳ��� �־��ش�.
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
            // ������ �� ��ų �����ϰ� ����
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

    // ī�� ����Ʈ �ָ� �����ϰ� Ÿ�� ��������
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
                        case "����":
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
