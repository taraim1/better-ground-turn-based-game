using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // ���� ĳ���� ������
    public Character enemy;


    // ���� �̹� �Ͽ� �� ��ų ī�� �ڵ� ����Ʈ�� ��ȯ�ϴ� �޼ҵ�
    private List<CardManager.skillcard_code> get_action() 
    {
        // �̹� �Ͽ� �� ��ų ����
        int skill_use_count = 1;

        List<CardManager.skillcard_code> result = new List<CardManager.skillcard_code>();

        // ������ �� ��ų �����ϰ� ����
        for (int i = 0; i < skill_use_count; i++) 
        {
            int rand = Random.Range(0, enemy.deck.Length);
            result.Add(enemy.deck[rand]);
        }

        return result;
    }

    // �̹� ���� ��ų�� �����ϴ� �Ѱ� �޼ҵ�
    private void set_skill() 
    {
        List<CardManager.skillcard_code> skill_list = get_action();
        print(string.Format("{0}�� �̹� �� ��ų : {1}", enemy.name, skill_list[0]));
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
