using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckManager : DeckManager
{
    void Start()
    {
        //List<Item> items = StageManager.Instance.GetCurrentStageData();
        SetUpEnemyDeckManager();
    }

    public void SetUpEnemyDeckManager()
    {
        SetupItemBuffer(SaveManager.Instance.saveDeckData.CurDeck.ShallowCopy().DeckData);
    }
}
