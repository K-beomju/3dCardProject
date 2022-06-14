using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAttackAction : CardAction
{
    public override void TakeAction(Card card)
    {
        FindObjectOfType<Hack>().ChangeHack(card);
        print("ASd");
    }
}
