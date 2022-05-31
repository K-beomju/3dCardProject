using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckManager : DeckManager
{
    private void Start()
    {
        SetupItemBuffer(SaveManager.Instance.saveDeckData.CurDeck.ShallowCopy().DeckData);
    }
}
