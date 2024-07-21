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



    // 덱에서 랜덤한 카드 한 장을 고르는 전략
    // 지정한 스킬카드 코드 리스트에 코드 하나를 넣어준다.


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
            // 덱에서 쓸 스킬 랜덤하게 뽑음
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
