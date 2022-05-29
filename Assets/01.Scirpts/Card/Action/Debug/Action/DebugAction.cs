using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAction : CardAction
{
    public string debugStr = "";
    public override void TakeAction(Card card)
    {
        Debug.Log("µð¹ö±× : " + debugStr);
    }
}
