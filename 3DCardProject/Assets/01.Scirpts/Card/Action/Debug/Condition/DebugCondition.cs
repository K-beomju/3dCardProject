using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCondition : CardCondition
{
    public override bool CheckCondition()
    {
        Debug.Log("����� �����");
        return true;
    }
}
