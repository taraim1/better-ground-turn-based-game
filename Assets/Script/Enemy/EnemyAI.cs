using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // 적의 캐릭터 데이터
    public Character enemy;


    // 적이 이번 턴에 쓸 스킬 카드 코드 리스트를 반환하는 메소드
    private List<CardManager.skillcard_code> get_action() 
    {
        // 이번 턴에 쓸 스킬 개수
        int skill_use_count = 1;

        List<CardManager.skillcard_code> result = new List<CardManager.skillcard_code>();

        // 덱에서 쓸 스킬 랜덤하게 뽑음
        for (int i = 0; i < skill_use_count; i++) 
        {
            int rand = Random.Range(0, enemy.deck.Length);
            result.Add(enemy.deck[rand]);
        }

        return result;
    }

    // 이번 턴의 스킬을 설정하는 총괄 메소드
    private void set_skill() 
    {
        List<CardManager.skillcard_code> skill_list = get_action();
        print(string.Format("{0}의 이번 턴 스킬 : {1}", enemy.name, skill_list[0]));
    }

    private void Awake()
    {
        BattleEventManager.enemy_skill_setting_phase += set_skill;
    }

    private void OnDisable()
    {
        BattleEventManager.enemy_skill_setting_phase -= set_skill;
    }
}
