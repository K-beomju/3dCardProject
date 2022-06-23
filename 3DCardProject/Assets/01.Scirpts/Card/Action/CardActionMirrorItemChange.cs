using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionMirrorItemChange : CardAction
{
    Card thisCard = null;
    public override void TakeAction(Card card)
    {
        CardManager.Instance.OnChangeLastUsedCard += (item) => {
            thisCard = card;
            ChangeCardItem(item);
        };
    }

    public void ChangeCardItem( Item item)
    {
        if (thisCard == null)
        {
            CardManager.Instance.OnChangeLastUsedCard -= (item) => {
                ChangeCardItem(item);
            };
            return;
        }

        if (thisCard.curField == null && thisCard.gameObject != null)
        {
            thisCard.item = item;
            thisCard.RefreshInform();
        }
    }
}
