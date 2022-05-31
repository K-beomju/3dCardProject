using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionHLineClear : CardAction
{
    public override void TakeAction(Card card)
    {
        Vector2Int gridPos = FieldManager.Instance.GetGridPos(card.curField);


        for (int x = 0; x < FieldManager.Instance.x; x++)
        {
            Vector2Int getPos = new Vector2Int(x, gridPos.y);
            Field field = FieldManager.Instance.GetField(getPos);
            Debug.Log(getPos);
            if (field != null)
            {
                if (field.curCard != null)
                    field.curCard.OnDie();
            }
        }
    }
}
