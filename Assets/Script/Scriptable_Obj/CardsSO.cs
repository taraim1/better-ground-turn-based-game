using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Cards // ī�� �����Ͱ� ��ũ���ͺ� ������Ʈ�� ����Ǵ� Ŭ����
{
    public string name;
    public int cost;
    public string type;
    public string behavior_type;
    public Sprite sprite;
    public int[] minPowerOfLevel;
    public int[] maxPowerOfLevel;

    public bool isDirectUsable; // ĳ���Ϳ� ���� ����� �������� ����

    public List<SkillEffect> effects;
}

[CreateAssetMenu(fileName = "CarsSO", menuName = "Scriptable_Objects")]
public class CardsSO : ScriptableObject 
{
    public Cards[] cards;
}
