using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionLose : CardAction
{
    public override void TakeAction(Card card)
    {
        if (card.isPlayerCard)
        {
            GameManager.Instance.OnLoseGame?.Invoke();
        }
        else
        {
            GameManager.Instance.OnWinGame?.Invoke();
        }
    }
}
