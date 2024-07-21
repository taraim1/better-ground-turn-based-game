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



    // ������ ������ ī�� �� ���� ���� ����
    // ������ ��ųī�� �ڵ� ����Ʈ�� �ڵ� �ϳ��� �־��ش�.


    public class Random_Card_Pick_Strategy : IStrategy
    {
        Character enemyCharacter;
        List<skillcard_code> resultList;
        public Random_Card_Pick_Strategy(Character enemyCharacter, List<skillcard_code> resultList) 
        {
            this.enemyCharacter = enemyCharacter;
            this.resultList = resultList;

        }

        public Node.Status Process() 
        {
            // ������ �� ��ų �����ϰ� ����
            try
            {
                int rand = UnityEngine.Random.Range(0, enemyCharacter.deck.Length);
                resultList.Add(enemyCharacter.deck[rand]);
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
