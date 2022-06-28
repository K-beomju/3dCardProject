using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAction : CardAction
{
    public override void TakeAction(Card card)
    {
        var node = NewFieldManager.Instance.fields.GetNodeByData(card.curField);
        card = node.Data.avatarCard;
        if (card != null && card.item.IsAvatar)
        {
            Card avatar = card.curField.avatarCard;
            if (avatar != null)
            {
                if (avatar.isPlayerCard)
                {
                    CardManager.Instance.RandCardDelete();
                }
                else
                {

                    card = CardManager.Instance.CreateCard(EnemyManager.Instance.dm.PopItem(), false);
                    CardManager.Instance.CardDie(card);
                }
            }

        }


    }
}
