using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;


public class DebugCommand : EditorWindow
{
    
    
    [MenuItem("StageCamClear", menuItem = "Debug/Stage/StageValueReset")]
    public static void StageValueReset()
    {
        if(SaveManager.Instance != null)
        {
            SaveManager.Instance.gameData.StageValue = 0;
            SaveManager.Instance.gameData.RouteValue = 0;
            SaveManager.Instance.gameData.crossType = PlayerGameData.CrossType.None;
        }
    }
}
