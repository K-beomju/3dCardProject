using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilClass
{
    public static bool IsIncludeFlag<T>(T from, T to)
    {
        int _from = (int)(object)from;
        int _to = (int)(object)to;

        if((_from & _to) != 0)
        {
            return true;
        }

        return false;
    }
}