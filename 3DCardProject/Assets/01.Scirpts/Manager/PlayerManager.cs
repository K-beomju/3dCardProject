using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public Vector2Int playerPos;
    public Item playerItem;
    public int PlayerHP;

    public List<Card> spawnCardList = new List<Card>();

    public static void TurnReset()
    {
        Instance.spawnCardList.ForEach(x => x.isMove = false);
    }

    public static void RemoveCard(Card card)
    {
        Instance.spawnCardList.Remove(card);
    }
}
