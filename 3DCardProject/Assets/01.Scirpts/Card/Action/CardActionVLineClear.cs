using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionVLineClear : CardAction
{
    public override void TakeAction(Card card)
    {
        Vector2Int gridPos = FieldManager.Instance.GetGridPos(card.curField);

        for (int y = 0; y < FieldManager.Instance.y; y++)
        {
            Vector2Int getPos = new Vector2Int(gridPos.x, y);
            Field field = FieldManager.Instance.GetField(getPos);
            //Debug.Log(getPos);
            if (field != null)
            {
                if (field.curCard != null)
                    field.curCard.OnDie();
            }
        }
    }
}
