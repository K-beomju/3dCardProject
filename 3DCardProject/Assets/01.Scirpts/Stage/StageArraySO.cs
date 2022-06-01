using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StageData
{
    public ItemSO itemSo;
    public Vector2Int vector2Int;
}


[CreateAssetMenu(fileName = "StageArraySO", menuName = "Scriptable Object/StageArraySO")]
public class StageArraySO : ScriptableObject
{
    public List<StageSO> stages;
}
