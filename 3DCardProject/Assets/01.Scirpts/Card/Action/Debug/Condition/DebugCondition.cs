using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCondition : CardCondition
{
    public override bool CheckCondition(Card card)
    {
        Debug.Log("����� �����");
        return true;
    }
}
