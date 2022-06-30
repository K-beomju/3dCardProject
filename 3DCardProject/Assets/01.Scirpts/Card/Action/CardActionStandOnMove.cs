using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionStandOnMove : CardAction
{
    public override void TakeAction(Card card)
    {
        NewFieldManager.Instance.AvatarMove(card.curField,()=> {
            TurnManager.Instance.CanChangeTurn = true;
            TurnManager.ChangeTurn(card.isPlayerCard ? TurnType.Enemy : TurnType.Player);
        });
    }
}
