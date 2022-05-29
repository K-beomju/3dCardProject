using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TurnType
{
    Player,
    Enemy
}

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField] private TurnType type;

    // 턴 바꿈 
    public static void ChangeTurn(TurnType _type)
    {
        Instance.type = _type;
    }

    // 현재 타입 반환 
    public static TurnType CurReturnType()
    {
        return Instance.type;
    }

    // 누구의 턴인지 아닌지 반환
    public static bool CurEntityTurn()
    {
        if (Instance.type == TurnType.Player)
        {
            return true;
        }
        return false;
    }


}
