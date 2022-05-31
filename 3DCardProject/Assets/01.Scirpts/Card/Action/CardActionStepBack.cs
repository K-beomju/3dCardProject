using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionStepBack : CardAction
{
    public override void TakeAction(Card card)
    {
        Vector2Int gridPos = FieldManager.Instance.GetGridPos(card.curField);

        Vector2Int getPos = new Vector2Int(gridPos.x, gridPos.y -1);

        FieldManager.Instance.MoveToGrid(getPos,card);
    }
}
