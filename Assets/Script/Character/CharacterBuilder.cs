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
    private bool makeSpumObjFlag = false;
    private int index = 0;
    private EnemyAiType aiType = EnemyAiType.NoAI;


    private void Reset_builder() // 싱글톤이라 하나 만들때마다 리셋해줘야함
    {
        IsEnemy(false).
        Code(character_code.kimchunsik).
        MakeSpumObjFlag(false).
        Coordinate(new coordinate()).
        Index(0).
        AiType(EnemyAiType.NoAI);
    }

    public CharacterBuilder IsEnemy(bool isEnemy){ this.isEnemy = isEnemy; return this; }
    public CharacterBuilder Code(character_code code) { this.code = code; return this; }
    public CharacterBuilder Coordinate(coordinate coordinate) { this.coordinate = coordinate; return this; }
    public CharacterBuilder Index(int i) { index = i; return this; }
    public CharacterBuilder MakeSpumObjFlag(bool flag) { makeSpumObjFlag = flag; return this; }
    public CharacterBuilder AiType(EnemyAiType type) { aiType = type; return this; }
    public GameObject build() 
    {
        GameObject rootObj = new GameObject("Character_base");
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

        character.Code = code;
        character.Load_data();

        character.Coordinate = coordinate;

        character.Character_index = index;


        if (makeSpumObjFlag) 
        {
            GameObject spPrefab = Resources.Load<GameObject>(character.SPUM_Datapath);
            GameObject SPUM_unit_obj = Instantiate(spPrefab, rootObj.transform);
            SPUM_unit_obj.transform.localPosition = new Vector3(0, -0.4f, 0);
            SPUM_unit_obj.transform.localScale = new Vector3(-1.3f, 1.3f, 1);
        }

        Reset_builder();
        return rootObj;
    }
}
