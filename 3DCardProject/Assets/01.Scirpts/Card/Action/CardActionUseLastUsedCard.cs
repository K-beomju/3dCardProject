using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionUseLastUsedCard : CardAction
{
    public override void TakeAction(Card card)
    {
        Item lastUsedCard = CardManager.Instance.LastUsedCardItem;
        Debug.Log("������ ī�� ��� �õ�");
        if(lastUsedCard != null)
        {
        Debug.Log("������ ī�� ��� ����");
            Card card1 = new Card();
            card1.item = lastUsedCard;
            card1.OnSpawn();
        }
    }
}
