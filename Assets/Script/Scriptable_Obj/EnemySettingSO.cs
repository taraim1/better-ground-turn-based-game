using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Enemy_settig // ���������� �� �����Ͱ� ��ũ���ͺ� ������Ʈ�� ����Ǵ� Ŭ����
{
    public List<CharacterManager.enemy_code> enemy_Codes;
}

[CreateAssetMenu(fileName = "EnemySettingSO", menuName = "Scriptable_Objects_enemySetting")]
public class EnemySettingSO : ScriptableObject 
{
    public Enemy_settig[] enemy_Settigs;
}
