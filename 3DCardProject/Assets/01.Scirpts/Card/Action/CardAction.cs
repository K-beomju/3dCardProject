using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class CardAction : MonoBehaviour
{
    public abstract void TakeAction(Card card);
}