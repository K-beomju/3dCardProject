using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� ���� 
[System.Serializable]
public class Enemy
{
    public Vector2Int vector2Int;
    public ItemSO item;
}

[System.Serializable]
public struct StageData
{
    public Enemy[] enemys;
}


[CreateAssetMenu(fileName = "StageSO", menuName = "Scriptable Object/StageSO")]
public class StageSO : ScriptableObject
{
    public StageData[] StageData;
}
