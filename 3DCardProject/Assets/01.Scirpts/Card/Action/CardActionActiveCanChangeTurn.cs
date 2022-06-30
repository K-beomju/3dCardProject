using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionActiveCanChangeTurn : CardAction
{
    public override void TakeAction(Card card)
    {
        var cardNode = NewFieldManager.Instance.fields.GetNodeByData(card.curField);
        var playerNode = NewFieldManager.Instance.GetPlayerNodeByData();
        var enemyNode = NewFieldManager.Instance.GetEnemyNodeByData();
        var curNode = card.isPlayerCard ? playerNode: enemyNode;

        bool result = false;

        if(NewFieldManager.Instance.IsClockDir)
        {
            result = cardNode.NextNode == curNode;
        }
        else
        {
            result = cardNode.PrevNode == curNode;
        }

        TurnManager.Instance.CanChangeTurn = result;
    }
}
