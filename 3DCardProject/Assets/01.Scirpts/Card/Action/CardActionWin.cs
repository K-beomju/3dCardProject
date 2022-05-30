using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionWin : CardAction
{
    public override void TakeAction(Card card)
    {
        if(card.isPlayerCard)
        {
            GameManager.Instance.OnWinGame?.Invoke();
        }
        else
        {
            GameManager.Instance.OnLoseGame?.Invoke();
        }
    }
}
