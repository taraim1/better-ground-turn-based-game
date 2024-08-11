using BattleUI;
using BehaviorTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterBuilder : Singletone<CharacterBuilder>
{

    private bool isEnemy = false;
    private character_code code = character_code.kimchunsik;
    private coordinate coordinate = new coordinate();
    private int index = 0;

    private bool makeSpumObj = false;
    private bool makePanicSign = false;
    private bool makeHealthAndWillpowerBar = false;
    private bool makeSkillSlotController = false;
    private bool makeSkillPowerMeter = false;
    private bool makeCharacterEffectController = false;

    [SerializeField] private GameObject character_base_prefab;
    [SerializeField] private GameObject panic_sign_prefab;
    [SerializeField] private GameObject health_bar_prefab;
    [SerializeField] private GameObject willpower_bar_prefab;
    [SerializeField] private GameObject skill_slot_controller;
    [SerializeField] private GameObject skill_power_meter_prefab;
    [SerializeField] private GameObject character_effect_controller_prefab;

    [SerializeField] private CharacterDataSO DataSO;

    private void Reset_builder() // 싱글톤이라 하나 만들때마다 리셋해줘야함
    {
        IsEnemy(false).
        Code(character_code.kimchunsik).
        Coordinate(new coordinate()).
        Index(0).
        MakeSpumObj(false).
        MakePanicSign(false).
        MakeHealthAndWillpowerBar(false).
        MakeSkillSlotController(false).
        MakeSkillPowerMeter(false).
        MakeCharacterEffectController(false);
    }

    public CharacterBuilder IsEnemy(bool isEnemy){ this.isEnemy = isEnemy; return this; }
    public CharacterBuilder Code(character_code code) { this.code = code; return this; }
    public CharacterBuilder Coordinate(coordinate coordinate) { this.coordinate = coordinate; return this; }
    public CharacterBuilder Index(int i) { index = i; return this; }
    public CharacterBuilder MakeSpumObj(bool flag) { makeSpumObj = flag; return this; }
    public CharacterBuilder MakePanicSign(bool flag) { makePanicSign = flag; return this; }
    public CharacterBuilder MakeHealthAndWillpowerBar(bool flag) { makeHealthAndWillpowerBar = flag; return this; }
    public CharacterBuilder MakeSkillSlotController(bool flag) { makeSkillSlotController = flag; return this; }
    public CharacterBuilder MakeSkillPowerMeter(bool flag) { makeSkillPowerMeter = flag; return this; }
    public CharacterBuilder MakeCharacterEffectController(bool flag) { makeCharacterEffectController = flag; return this; }
    public Character build() 
    {
        GameObject rootObj = Instantiate(character_base_prefab);
        Character character;

        if (isEnemy) 
        {
            rootObj.tag = "EnemyCharacter";
            EnemyCharacter enemy_character = rootObj.AddComponent<EnemyCharacter>();
            character = enemy_character;

            if (!DataSO.EnemyData.ContainsKey(code)) 
            {
                print("적 스폰 과정에서 오류 발생 : 적 데이터 딕셔너리에 없는 캐릭터 코드입니다.");
            }

            MoveTree moveTree;
            SkillSelectTree skillSelectTree;

            switch (DataSO.EnemyData[code].moveAiType)
            {
                case EnemyAiType.NoAI:
                    moveTree = new MoveTree_NoAI("MoveTree_NoAi", enemy_character);
                    break;
                case EnemyAiType.RandomAI:
                    moveTree = new MoveTree_RandomMove("MoveTree_RandomMove", enemy_character);
                    break;
                default:
                    moveTree = new MoveTree_NoAI("MoveTree_NoAi", enemy_character);
                    break;

            }

            switch (DataSO.EnemyData[code].skillSelectAiType)
            {
                case EnemyAiType.NoAI:
                    skillSelectTree = new SkillSelectTree_NoAI("SkillSelectTree_NoAi", enemy_character);
                    break;
                case EnemyAiType.RandomAI:
                    skillSelectTree = new SkillSelectTree_RandomPickOne("SkillSelectTree_NoAi", enemy_character);
                    break;
                default:
                    skillSelectTree = new SkillSelectTree_NoAI("SkillSelectTree_NoAi", enemy_character);
                    break;
            }

            enemy_character.SetAI(new EnemyAI(enemy_character, moveTree, skillSelectTree));
        }
        else 
        {
            rootObj.tag = "PlayerCharacter";
            character = rootObj.AddComponent<PlayableCharacter>(); 
        }

        character.Data_SO = DataSO;
        character.Code = code;
        character.Load_data();
        character.Coordinate = coordinate;
        character.Character_index = index;

        character_base character_base = rootObj.GetComponent<character_base>();
        if (makeSpumObj) 
        {
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_Datapath);
            GameObject SPUM_unit_obj = character_base.Attach(character_base.location.Base, spPrefab);
            SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            SPUM_unit_obj.transform.localScale = new Vector3(-1.3f, 1.3f, 1);

            if (character.check_enemy()) 
            { 
                SPUM_unit_obj.transform.localScale = new Vector3(1.3f, 1.3f, 1);
            }
        }

        List<CharacterUI> characterUIs = new List<CharacterUI>();
        if (makePanicSign) 
        {
            characterUIs.Add(AttachUI(character_base, character_base.location.Middle_canvas, panic_sign_prefab));
        }

        if (makeHealthAndWillpowerBar) 
        {
            characterUIs.Add(AttachUI(character_base, character_base.location.Bottom_layoutGroup, health_bar_prefab)); 
            characterUIs.Add(AttachUI(character_base, character_base.location.Bottom_layoutGroup, willpower_bar_prefab));
        }

        if (makeSkillPowerMeter) 
        {
            characterUIs.Add(AttachUI(character_base, character_base.location.Middle_canvas, skill_power_meter_prefab));
        }

        foreach (CharacterUI characterUI in characterUIs) 
        {
            characterUI.Initialize(character);
        }

        if (makeSkillSlotController && isEnemy)
        {
            character_base.Attach(character_base.location.Base, skill_slot_controller);
        }

        if (makeCharacterEffectController) 
        {
            character_base.Attach(character_base.location.Base, character_effect_controller_prefab);
        }

        Reset_builder();
        return character;
    }

    private CharacterUI AttachUI(character_base character_base, character_base.location location, GameObject obj) 
    {
        GameObject Temp_obj = character_base.Attach(location, obj);
        return Temp_obj.GetComponent<CharacterUI>();
    }
}
