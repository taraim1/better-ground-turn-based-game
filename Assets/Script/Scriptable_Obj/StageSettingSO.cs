using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Stage_settig // 스테이지별 적 데이터가 스크립터블 오브젝트로 저장되는 클래스
{
    public List<CharacterManager.enemy_code> enemy_Codes;
    public int holy_water_requirement;
    public ResourceManager.resource_code[] loots;
    public int[] minLootNumber;
    public int[] maxLootNumber;
}

[CreateAssetMenu(fileName = "StageSettingSO", menuName = "Scriptable_Objects_StageSetting")]
public class StageSettingSO : ScriptableObject 
{
    public Stage_settig[] stage_Settings;
}
