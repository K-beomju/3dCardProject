using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionChangeDir : CardAction
{
    public override void TakeAction(Card card)
    {
        NewFieldManager.Instance.ChangeDir();
    }
}
