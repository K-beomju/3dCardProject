using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckManager : DeckManager
{
    private void Start()
    {
        SetupItemBuffer(SaveManager.Instance.saveDeckData.CurDeck.ShallowCopy().DeckData);
    }

    public void AddCardToDeck(Item InItem)
    {
        Item item = InItem.ShallowCopy();
        SaveManager.Instance.saveDeckData.CurDeck.DeckData.Add(item);
        AddCardToItemBuffer(item, item.count);
    }
}
