using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCard : MonoBehaviour
{
    private int bombCount;
    public Action OnUnderZero;
    public TextMesh CountText;
    public void InitBomb(int inCount)
    {
        bombCount = inCount;
        TextRefresh();
    }
    public void Counting()
    {
        if(--bombCount<= 0)
        {
            OnUnderZero?.Invoke();
        }
        TextRefresh();
    }

    public void TextRefresh()
    {
        CountText.text = bombCount.ToString();
    }
}
