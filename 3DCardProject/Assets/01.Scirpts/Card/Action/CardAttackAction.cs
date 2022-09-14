using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAttackAction : CardAction
{
    public override void TakeAction(Card card)
    {
        if(TutorialManager.Instance != null && TutorialManager.Instance.isTutorial)
        {
            if (BattleTutorial.Instance != null)
                BattleTutorial.Instance.isAttak = true;
        }

        FindObjectOfType<Hack>().ChangeHack(card);
    }
}
