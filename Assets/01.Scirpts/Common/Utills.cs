using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PRS
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

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
}
