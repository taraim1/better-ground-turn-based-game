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
    public GameObject background_prefab;
    public List<BattleGridManager.boardRow> board;

    [System.Serializable]
    public struct Cordinate
    {
        public int x;
        public int y;
    }
    public List<Cordinate> player_spawnpoints;
    public List<Cordinate> enemy_spawnpoints;
}

[CreateAssetMenu(fileName = "StageSettingSO", menuName = "Scriptable_Objects_StageSetting")]
public class StageSettingSO : ScriptableObject
{
    [SerializeField] public StageSettingSO_dictionary stage_Settings;
}

[System.Serializable]
public class StageSettingSO_dictionary : SerializableDictionary<int, Stage_settig> { }
