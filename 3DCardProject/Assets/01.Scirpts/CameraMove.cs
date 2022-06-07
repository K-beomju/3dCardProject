using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    public bool isLock = false;

    void Update()
    {
        float yFieldPos = Camera.main.ScreenToViewportPoint(Input.mousePosition).y;
        float yGamePos = Camera.main.ScreenToViewportPoint(Input.mousePosition).y;

        if (!isLock)
        {
            if (yFieldPos < 0.1f)
            {
                transform.DOMove(new Vector3(0, 5.9f, -4f), 0.5f);
                isLock = true;
            }

        }
        if(isLock)
        {
            if(yGamePos > 0.4f)
            {
                transform.DOMove(new Vector3(0, 5.9f, 0.3f), 0.5f);
                isLock = false;
 
            }
        }
         
    }
}
