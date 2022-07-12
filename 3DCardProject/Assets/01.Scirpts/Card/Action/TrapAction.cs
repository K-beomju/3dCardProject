using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAction : CardAction
{
    public override void TakeAction(Card card)
    {
        var node = NewFieldManager.Instance.fields.GetNodeByData(card.curField);
        Card avatar = card.curField.avatarCard;
        if (avatar != null && avatar.item.IsAvatar)
        {
            if (avatar != null)
            {
                if (avatar.isPlayerCard)
                {
                    CardManager.Instance.RandCardDelete();
                }
                else
                {

                    Card desCard = CardManager.Instance.CreateCard(EnemyManager.Instance.dm.PopItem(), false);
                    CardManager.Instance.CardDie(desCard);
                }
                    CardManager.Instance.CardDie(card);
            }

        }


    }
}
