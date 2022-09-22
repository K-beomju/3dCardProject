using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCard : MonoBehaviour
{
    private int bombCount;
    public Action OnUnderZero;
    public void InitBomb(int inCount)
    {
        bombCount = inCount;
    }
    public void Counting()
    {
        if(--bombCount<= 0)
        {
            OnUnderZero?.Invoke();
        }

    }

}
