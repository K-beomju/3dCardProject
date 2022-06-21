using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMoveAction : CardAction
{
    public override void TakeAction(Card card)
    {
        print("AAAAA");
        if (card.isPlayerCard)
        {
            print("BBBB");
            NewFieldManager.Instance.AvatarMove(NewFieldManager.Instance.playerCard.curField);
        }
        else
        {
            print("CCCC");
            NewFieldManager.Instance.AvatarMove(NewFieldManager.Instance.enemyCard.curField);
        }
    }

}
