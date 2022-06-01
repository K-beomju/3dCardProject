using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "StageSO", menuName = "Scriptable Object/StageSO")]
public class StageSO : ScriptableObject
{
    public StageData[] StageData;
}
