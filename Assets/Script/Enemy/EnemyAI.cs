using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    // 카드 데이터가 담긴 스크립터블 오브젝트
    [SerializeField] CardsSO cardsSO;

    // 적의 캐릭터 데이터
    public Character enemy;
    // 적의 스킬 슬롯 프리팹
    public GameObject skill_slot_prefab;
    // 적이 이턴 턴에 쓸 카드 오브젝트가 들어가는 리스트
    public List<GameObject> using_skill_Objects;

    public GameObject layoutGroup;
    // 이번 턴의 스킬 슬롯이 들어가는 리스트
    public List<GameObject> skill_slots = new List<GameObject>();

    private bool isBattleEnded;

   
    // 적이 이번 턴에 쓸 스킬 카드 코드 리스트를 반환하는 메소드
    private List<skillcard_code> get_action()
    {
        // 이번 턴에 쓸 스킬 개수
        int skill_use_count = 1;

        List<skillcard_code> result = new List<skillcard_code>();

        // 덱에서 쓸 스킬 랜덤하게 뽑음
        for (int i = 0; i < skill_use_count; i++)
        {
            int rand = UnityEngine.Random.Range(0, enemy.deck.Length);
            result.Add(enemy.deck[rand]);
        }

        return result;
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

    // 이번 턴의 스킬을 설정하는 총괄 메소드
    private void set_skill()
    {
        // 전투 끝났으면 작동 X
        if (isBattleEnded) { return; }

        List<skillcard_code> skill_list = new List<skillcard_code>();

        // 이전 턴 스킬 다 삭제
        clear_skills();

        // 패닉이 아니면 행동함
        if (!gameObject.GetComponent<Character>().isPanic)
        {
            skill_list = get_action();
        }


        for (int i = 0; i < skill_list.Count; i++)
        {
            // 이번 턴에 쓸 카드와 스킬 슬롯 만듦
            GameObject slot = Instantiate(skill_slot_prefab, layoutGroup.transform);
            GameObject card_obj = CardManager.instance.Summon_enemy_card(skill_list[i], gameObject);
            card card = card_obj.GetComponent<card>();
            slot.GetComponent<enemy_skillCard_slot>().card_obj = card_obj;
            slot.GetComponent<enemy_skillCard_slot>().enemy_Obj = gameObject;
            slot.GetComponent<enemy_skillCard_slot>().illust.sprite = card.illust.sprite;

            // 카드 타겟 정하기
            switch (card.Card.behavior_type)
            {
                case "공격":
                    int rand = UnityEngine.Random.Range(0, BattleManager.instance.playable_characters.Count);
                    card.target = BattleManager.instance.playable_characters[rand];
                    // 라인렌더러 설정
                    Vector3 targetpos = card.target.transform.position;
                    StartCoroutine(slot.GetComponent<enemy_skillCard_slot>().Set_line(new Vector3(targetpos.x, targetpos.y, -2f)));
                    break;
                case "방어":
                    card.target = gameObject;
                    break;
                case "회피":
                    card.target = gameObject;
                    break;
            }


            using_skill_Objects.Add(card_obj);
            skill_slots.Add(slot);
            // 적이 이번 턴에 쓰는 전체 카드가 들어가게 되는 리스트
            BattleManager.instance.enemy_cards.Add(card);
        }

        // 스킬 설정 완료 카운트 증가
        BattleManager.instance.enemy_skill_set_count += 1;
    }

    // 적의 모든 스킬 카드 강조 해제
    private void return_card()
    {
        foreach (GameObject card_obj in using_skill_Objects)
        {
            card card = card_obj.GetComponent<card>();
            card.MoveTransform(card.originPRS, false, 0f);
            card.state = card.current_mode.normal;
        }
    }

    // 아군 캐릭터가 죽었을 때 그 캐릭터를 타게팅하고 있었던 스킬을 제거
    private void OnPlayerCharacterDied() 
    {
        StartCoroutine(check_dead_target());
    }

    // 전투 끝나면
    private void OnBattleEnd(bool victory)
    {
        isBattleEnded = true;
    }

    private IEnumerator check_dead_target() 
    {
        yield return new WaitForSeconds(0.01f);// 캐릭터 오브젝트 비활성화를 잠시 기다림

        for (int i = using_skill_Objects.Count - 1; i >= 0; i--)
        {
            card card = using_skill_Objects[i].GetComponent<card>();

            try
            {
                Transform tmp = card.target.transform;
            }
            catch (MissingReferenceException e)
            {
                CardManager.instance.Destroy_card(card);
            }
        }
        yield break;
    }

    private void Awake()
    {
        BattleEventManager.enemy_skill_setting_phase += set_skill;
        BattleEventManager.enemy_skill_card_deactivate += return_card;
        BattleEventManager.player_character_died += OnPlayerCharacterDied;
        BattleEventManager.battle_ended += OnBattleEnd;

        isBattleEnded = false;
    }

    private void OnDisable()
    {
        BattleEventManager.enemy_skill_setting_phase -= set_skill;
        BattleEventManager.enemy_skill_card_deactivate -= return_card;
        BattleEventManager.player_character_died -= OnPlayerCharacterDied;
        BattleEventManager.battle_ended -= OnBattleEnd;
    }
}

