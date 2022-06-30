using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;


public class DebugCommand : EditorWindow
{
    [MenuItem("StageCamClear", menuItem = "Debug/Stage/StageCamClear")]
    public static void StageCamClear()
    {
        GameObject cam = GameObject.Find("CM vcam1");
        CinemachineVirtualCamera virtualCam= cam.GetComponent<CinemachineVirtualCamera>();

        cam.transform.position = new Vector3(13.94f, 2f, -20.5f);
        cam.transform.rotation = Quaternion.Euler(15.652f, -87.359f,0);
        virtualCam.LookAt = GameObject.Find("Totem").transform;

    }
    
    [MenuItem("StageCamClear", menuItem = "Debug/Stage/StageValueReset")]
    public static void StageValueReset()
    {
        PlayerPrefs.DeleteKey("StageValue");
    }
}
