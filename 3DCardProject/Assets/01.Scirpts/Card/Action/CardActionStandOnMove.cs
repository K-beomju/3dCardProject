using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionStandOnMove : CardAction
{
    private Card card;
    public override void TakeAction(Card card)
    {
        this.card = card.curField.avatarCard;
        TurnManager.Instance.CanChangeTurn = false;
        NewFieldManager.Instance.AvatarMove(card.curField,()=> {
            Debug.Log("CT");
            Debug.Log(TurnManager.Instance.CanChangeTurn);
            ChangeTurn();
            /*TurnManager.Instance.CanChangeTurn = true;
            Invoke("ChangeTurn", .5f);*/
        });
    }

    private void ChangeTurn()
    {
        if (!TurnManager.Instance.CanChangeTurn) return;
        Debug.Log(TurnManager.Instance.CanChangeTurn);

        TurnManager.ChangeTurn(card.isPlayerCard ? TurnType.Enemy : TurnType.Player);

    }
}
