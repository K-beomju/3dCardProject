using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    public bool isLock = false;
    public float yCardValue = 0.05f;
    public float yGameValue = 0.5f;

    void Update()
    {
        float yCardPos = Camera.main.ScreenToViewportPoint(Input.mousePosition).y;
        float yGamePos = Camera.main.ScreenToViewportPoint(Input.mousePosition).y;

        if (!isLock)
        {
            if (yCardPos < yCardValue)
            {
                transform.DOMove(new Vector3(0, 5.9f, -4f), 0.3f);
                isLock = true;
            }

        }
        if(isLock)
        {
            if(yGamePos > yGameValue)
            {
                transform.DOMove(new Vector3(0, 5.9f, 0.3f), 0.3f);
                isLock = false;
 
            }
        }
         
    }
}
