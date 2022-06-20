using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionUseLastUsedCard : CardAction
{
    public override void TakeAction(Card card)
    {
        Item lastUsedCard = CardManager.Instance.LastUsedCardItem;
        Debug.Log("마지막 카드 사용 시도");
        if(lastUsedCard != null)
        {
        Debug.Log("마지막 카드 사용 성공");
            Card card1 = new Card();
            card1.item = lastUsedCard;
            card1.OnSpawn();
        }
    }
}
