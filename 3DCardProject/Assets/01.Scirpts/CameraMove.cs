using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    public bool isLock = false;

    void Update()
    {
        float yPos = Camera.main.ScreenToViewportPoint(Input.mousePosition).y;
        if (!isLock)
        {
            if (yPos < 0.4f)
            {
                // Player
                if (!isLock)
                {
                    transform.DOMove(new Vector3(0, 5.9f, -4f), 0.5f);
                    //transform.DORotate(new Vector3(45, 0, 0), 0.5f);
                }
            }
            else
            {
                // Enemy

                transform.DOMove(new Vector3(0, 5.9f, 0.3f), 0.5f);
            }

        }

    }
}
