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

    // �� �ٲ� 
    public static void ChangeTurn(TurnType _type)
    {
        Instance.type = _type;
    }

    // ���� Ÿ�� ��ȯ 
    public static TurnType CurReturnType()
    {
        return Instance.type;
    }

    // ������ ������ �ƴ��� ��ȯ
    public static bool CurEntityTurn()
    {
        if (Instance.type == TurnType.Player)
        {
            return true;
        }
        return false;
    }


}
