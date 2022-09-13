using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionChangeTurnState : CardAction
{
    public override void TakeAction(Card card)
    {
        TurnManager.ChangeTurn(card.isPlayerCard ? TurnType.Enemy : TurnType.Player);

        if (BattleTutorial.isChangeDir)
            BattleTutorial.isStop = true;
    }
}
