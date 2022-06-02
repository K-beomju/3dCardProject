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
    
    protected override void SuffleItemBuffer()
    {
        base.SuffleItemBuffer();

        List<Item> tempBuffer = new List<Item>();

        foreach (var item in itemBuffer)
        {
            if (item.isMagic)
            {
                tempBuffer.Add(item);
            }
        }
        foreach (var temp in tempBuffer)
        {
            itemBuffer.Remove(temp);
        }
    }
}
