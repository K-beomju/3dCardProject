using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionStandOnMove : CardAction
{
    private Card card;
    public override void TakeAction(Card card)
    {
        this.card = card.curField.avatarCard;
        var node = NewFieldManager.Instance.GetNodeByData(card.curField);

        TurnManager.Instance.CanChangeTurn = false;
        NewFieldManager.Instance.AvatarMove(card.curField,()=> {
            Debug.Log("CT");
            Debug.Log(TurnManager.Instance.CanChangeTurn);
            bool canChangeTurn = false;

            if (NewFieldManager.Instance.IsClockDir)
            {
                if (node.NextNode.Data.upperCard != null)
                {
                    canChangeTurn = node.NextNode.Data.upperCard.item.uid != 103;
                }
                else
                {
                    canChangeTurn = true;
                }
            }
            else
            {
                if (node.PrevNode.Data.upperCard != null)
                {
                    canChangeTurn = node.NextNode.Data.upperCard.item.uid != 103;
                }
                else
                {
                    canChangeTurn = true;
                }
            }

            if(canChangeTurn)
            {
                ChangeTurn();
            }
            /*TurnManager.Instance.CanChangeTurn = true;
            Invoke("ChangeTurn", .5f);*/
        });
    }

    private void ChangeTurn()
    {
        if (!TurnManager.Instance.CanChangeTurn) return;
        Debug.Log(TurnManager.Instance.CanChangeTurn);

        TurnManager.ChangeTurn(TurnManager.Instance.Type == TurnType.Player ? TurnType.Enemy : TurnType.Player);

    }
}
