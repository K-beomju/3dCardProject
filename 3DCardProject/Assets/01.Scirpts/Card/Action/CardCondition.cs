using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class CardCondition : MonoBehaviour
{
    public abstract bool CheckCondition();
}
