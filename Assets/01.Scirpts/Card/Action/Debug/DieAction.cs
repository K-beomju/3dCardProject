using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAction : CardAction
{
    public override void TakeAction(Card card)
    {

        PRS prs;
        if (card.curField.isPlayerField)
        {
            prs = new PRS(CardManager.Instance.cardDeletePoint.position, card.transform.rotation, card.transform.localScale);
        }
        else
        {
            prs = new PRS(CardManager.Instance.enemy_cardDeletePoint.position, card.transform.rotation, card.transform.localScale);
        }
        card.curField.RemoveCard();
        card.curField = null;
        card.MoveTransform(prs, true, 0.3f);

        Destroy(card.gameObject, 1);
    }
}
