using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionActiveCanChangeTurn : CardAction
{
    public override void TakeAction(Card card)
    {
        TurnManager.Instance.CanChangeTurn = !TurnManager.Instance.CanChangeTurn;
    }
}
