using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionReflect : CardAction
{
    public override void TakeAction(Card card)
    {
        CardManager.Instance.OnReflect?.Invoke();
    }
}
