using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckManager : DeckManager
{
    void Start()
    {
        List<Item> items = StageManager.Instance.GetCurrentStageData();
        SetupItemBuffer(items);
    }
}
