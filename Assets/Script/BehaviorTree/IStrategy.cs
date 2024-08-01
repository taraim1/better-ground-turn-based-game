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
    public class RandomData_Pick_Strategy : IStrategy
    {
        EnemyCharacter enemyCharacter;
        List<skillcard_code> resultList;
        public RandomData_Pick_Strategy(EnemyCharacter enemyCharacter, List<skillcard_code> resultList) 
        {
            this.enemyCharacter = enemyCharacter;
            this.resultList = resultList;

        }

        public Node.Status Process() 
        {
            // ������ �� ��ų �����ϰ� ����
            try
            {
                List<skillcard_code> deck_copy = enemyCharacter.Get_deck_copy();
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
}
