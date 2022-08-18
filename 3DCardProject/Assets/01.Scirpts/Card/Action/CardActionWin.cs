using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionWin : CardAction
{
    public override void TakeAction(Card card)
    {
        GameManager.Instance.State = GameState.END;
        GameManager.Instance.OnWinGame?.Invoke();
    }
}
