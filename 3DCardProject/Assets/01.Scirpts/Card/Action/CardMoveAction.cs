using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMoveAction : CardAction
{
    public override void TakeAction(Card card)
    {
        if(card.isPlayerCard)
            NewFieldManager.Instance.Move(NewFieldManager.Instance.playerCard.curField);
        else
            NewFieldManager.Instance.Move(NewFieldManager.Instance.enemyCard.curField);

    }

}
