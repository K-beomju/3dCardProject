using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMoveAction : CardAction
{
    public override void TakeAction(Card card)
    {
        if(card.isPlayerCard)
            NewFieldManager.Instance.AvatarMove(NewFieldManager.Instance.playerCard.curField);
        else
            NewFieldManager.Instance.AvatarMove(NewFieldManager.Instance.enemyCard.curField);

    }

}
