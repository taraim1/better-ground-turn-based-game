using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Enemy_settig // 스테이지별 적 데이터가 스크립터블 오브젝트로 저장되는 클래스
{
    public List<CharacterManager.enemy_code> enemy_Codes;
}

[CreateAssetMenu(fileName = "EnemySettingSO", menuName = "Scriptable_Objects_enemySetting")]
public class EnemySettingSO : ScriptableObject 
{
    public Enemy_settig[] enemy_Settigs;
}
