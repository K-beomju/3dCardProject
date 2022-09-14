using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionStop : CardAction
{
    public override void TakeAction(Card card)
    {
        if(TutorialManager.Instance != null && TutorialManager.Instance.isTutorial)
        {
            if (BattleTutorial.Instance != null)
                BattleTutorial.Instance.isStop = true;
        }
    }
}
