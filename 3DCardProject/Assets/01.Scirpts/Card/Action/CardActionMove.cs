using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionMove : CardAction
{
    public override void TakeAction(Card card)
    {
        if (card.isPlayerCard)
        {
            NewFieldManager.Instance.AvatarMove(NewFieldManager.Instance.playerCard.curField,()=>
            {
                TurnManager.ChangeTurn(TurnType.Enemy);
            });
        }
        else
        {
            NewFieldManager.Instance.AvatarMove(NewFieldManager.Instance.enemyCard.curField, () =>
            {
                TurnManager.ChangeTurn(TurnType.Player);
            });
        }
    }

}
