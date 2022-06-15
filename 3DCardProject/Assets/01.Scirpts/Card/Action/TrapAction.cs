using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAction : CardAction
{
    public override void TakeAction(Card card)
    {
        CardManager.Instance.RandCardDelete();
    }
}
