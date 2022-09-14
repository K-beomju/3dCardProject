using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionChangeDir : CardAction
{
    public override void TakeAction(Card card)
    {
        if (BattleTutorial.Instance != null && BattleTutorial.Instance.isChangeCard)
            BattleTutorial.Instance.isChangeDir = true;

        NewFieldManager.Instance.ChangeDir();
    }
}
