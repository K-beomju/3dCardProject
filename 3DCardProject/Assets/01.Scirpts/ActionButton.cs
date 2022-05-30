using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionButton : MonoBehaviour
{
    public Action OnMouseDownAct;

    private void OnMouseDown()
    {
        OnMouseDownAct?.Invoke();
        TurnManager.ChangeTurn(TurnType.Enemy,ref TurnManager.isClick);
    }
}
