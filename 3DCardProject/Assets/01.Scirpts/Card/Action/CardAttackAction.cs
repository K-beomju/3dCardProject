using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAttackAction : CardAction
{
    public override void TakeAction(Card card)
    {
        if(TutorialManager.Instance.isTutorial)
        {
            BattleTutorial.isAttak = true;
        }

        FindObjectOfType<Hack>().ChangeHack(card);
    }
}
