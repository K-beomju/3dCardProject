using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionTakeBackActionTrigger : CardAction
{
    public override void TakeAction(Card card)
    {
        // �� ���¿� ���� �� ���̸� �÷��̾ �÷��̾� ���̸� ���� ��� ����
        if (TurnManager.CurReturnType() == TurnType.Enemy)
        {
            // �÷��̾ �޾�ġ�� ī�� ��� ����
        }
        else if (TurnManager.CurReturnType() == TurnType.Player)
        {
            // ���� ī�� �޾�ġ�� ��� ����

        }
    }
}
