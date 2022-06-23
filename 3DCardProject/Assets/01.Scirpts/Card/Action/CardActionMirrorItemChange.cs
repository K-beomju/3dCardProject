using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionMirrorItemChange : CardAction
{
    public override void TakeAction(Card card)
    {
        CardManager.Instance.OnChangeLastUsedCard += (Item)=> { 
            if(card.curField == null && card.gameObject != null)
            {
                card.item = Item;
                card.RefreshInform();
            }
        };
    }
}
