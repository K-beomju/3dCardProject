using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionCountDie : CardAction
{
    public override void TakeAction(Card card)
    {
        if(card.isPlayerCard)
        {

        }   
        else
        {
            --EnemyFieldManager.Instance.spawnCardCount;

            EnemyFieldManager efm = EnemyFieldManager.Instance;
            if (efm.spawnCardCount <= 0)
            {
                GameManager.Instance.OnWinGame?.Invoke();
            }
        }
    }
}
