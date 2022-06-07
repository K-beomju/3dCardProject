using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAction : CardAction
{
    public override void TakeAction(Card card)
    {
        CardManager.Instance.CardDie(card);
    }
}
