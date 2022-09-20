using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckManager : DeckManager
{
    private void Start()
    {
        SetUpPlayerDeckManager();
    }

    public void SetUpPlayerDeckManager()
    {
        SetupItemBuffer(CardManager.Instance.defaultDeck.items);
    }

}
