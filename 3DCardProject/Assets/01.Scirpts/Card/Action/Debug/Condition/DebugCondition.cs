using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCondition : CardCondition
{
    public override bool CheckCondition(Card card)
    {
        Debug.Log("디버그 컨디션");
        return true;
    }
}
