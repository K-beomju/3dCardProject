using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionLose : CardAction
{
    public override void TakeAction(Card card)
    {
        GameManager.Instance.State = GameState.END;
        GameManager.Instance.OnLoseGame?.Invoke();
    }
}
