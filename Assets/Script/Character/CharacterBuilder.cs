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
    private EnemyAiType aiType = EnemyAiType.NoAI;

    private bool makeSpumObj = false;
    private bool makePanicSign = false;
    private bool makeHealthAndWillpowerBar = false;

    [SerializeField] private GameObject character_base_prefab;
    [SerializeField] private GameObject panic_sign_prefab;
    [SerializeField] private GameObject health_bar_prefab;
    [SerializeField] private GameObject willpower_bar_prefab;

    [SerializeField] private CharacterDataSO DataSO;

    private void Reset_builder() // 싱글톤이라 하나 만들때마다 리셋해줘야함
    {
        IsEnemy(false).
        Code(character_code.kimchunsik).
        Coordinate(new coordinate()).
        Index(0).
        AiType(EnemyAiType.NoAI).
        MakeSpumObj(false).
        MakePanicSign(false);
        MakeHealthAndWillpowerBar(false);
    }

    public CharacterBuilder IsEnemy(bool isEnemy){ this.isEnemy = isEnemy; return this; }
    public CharacterBuilder Code(character_code code) { this.code = code; return this; }
    public CharacterBuilder Coordinate(coordinate coordinate) { this.coordinate = coordinate; return this; }
    public CharacterBuilder Index(int i) { index = i; return this; }
    public CharacterBuilder AiType(EnemyAiType type) { aiType = type; return this; }
    public CharacterBuilder MakeSpumObj(bool flag) { makeSpumObj = flag; return this; }
    public CharacterBuilder MakePanicSign(bool flag) { makePanicSign = flag; return this; }
    public CharacterBuilder MakeHealthAndWillpowerBar(bool flag) { makeHealthAndWillpowerBar = flag; return this; }
    public Character build() 
    {
        GameObject rootObj = Instantiate(character_base_prefab);
        Character character;

        if (isEnemy) 
        { 
            EnemyCharacter enemy_character = rootObj.AddComponent<EnemyCharacter>();
            character = enemy_character;

            switch (aiType)
            {
                case EnemyAiType.NoAI:
                    enemy_character.SetAI(new EnemyAI(
                        enemy_character, 
                        new BehaviorTree.MoveTree_NoAI("NoMove", enemy_character),
                        new BehaviorTree.SkillSelectTree_NoAI("NoSkillSelect", enemy_character)
                    ));
                    break;
            }
        }
        else { character = rootObj.AddComponent<PlayableCharacter>(); }

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
        }

        List<BattleUI.CharacterUI> characterUIs = new List<BattleUI.CharacterUI>();
        GameObject Temp_obj;
        if (makePanicSign) 
        {
            Temp_obj = character_base.Attach(character_base.location.Middle_canvas, panic_sign_prefab);
            characterUIs.Add(Temp_obj.GetComponent<BattleUI.CharacterUI>());
        }

        if (makeHealthAndWillpowerBar) 
        {
            Temp_obj = character_base.Attach(character_base.location.Bottom_layoutGroup, health_bar_prefab);
            characterUIs.Add(Temp_obj.GetComponent<BattleUI.CharacterUI>());
            Temp_obj = character_base.Attach(character_base.location.Bottom_layoutGroup, willpower_bar_prefab);
            characterUIs.Add(Temp_obj.GetComponent<BattleUI.CharacterUI>());
        }

        foreach (BattleUI.CharacterUI characterUI in characterUIs) 
        {
            characterUI.Initialize(character);
        }

        Reset_builder();
        return character;
    }
}
