using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionTakeBackActionTrigger : CardAction
{
    public override void TakeAction(Card card)
    {
        // 턴 상태에 따라 적 턴이면 플레이어가 플레이어 턴이면 적이 사용 가능
        if (TurnManager.CurReturnType() == TurnType.Enemy)
        {
            // 플레이어가 받아치기 카드 사용 가능
        }
        else if (TurnManager.CurReturnType() == TurnType.Player)
        {
            // 적이 카드 받아치기 사용 가능

        }
    }
}
