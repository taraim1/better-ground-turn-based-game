using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Stage_settig // ���������� �� �����Ͱ� ��ũ���ͺ� ������Ʈ�� ����Ǵ� Ŭ����
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
