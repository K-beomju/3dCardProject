using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PRS
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;
    private Transform transform;

    public PRS(Transform transform)
    {
        this.pos = transform.position;
        this.rot = transform.rotation;
        this.scale = transform.localScale;
    }

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }
}

public class Utils
{
    public static Quaternion QI => Quaternion.identity;



    public static Vector3 MousePos
    {
        get
        {
            var pos = Input.mousePosition;
            pos.z = Camera.main.farClipPlane;
            Camera.main.ScreenToWorldPoint(pos);
            //pos.Normalize();
            return pos;
        }
    }
    public static Color32 PlayerColor = new Color32(100, 100, 255, 255);
    public static Color32 EnemyColor = new Color32(255, 100, 100, 255);
    public static Color32 WhiteColor = new Color32(255, 255, 255, 255);

    public static IEnumerator WaitForInputKey(KeyCode keyCode)
    {
        while (!Input.GetKey(keyCode))
        {
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
}
