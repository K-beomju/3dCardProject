using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionTurn2Phase2 : CardAction
{
    public override void TakeAction(Card card)
    {
        if (EnemyAI.Instance != null)
        {
            EnemyAI.Instance.isPhaseOne = false;
            EnemyAI.Instance.Turn2Phase2();
        }
    }
}
