using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckManager : DeckManager
{
    void Start()
    {
        List<Item> items =  GetItemDataFromStageSO(StageManager.Instance.stageSO.stages[StageManager.Instance.targetStage]);
        SetupItemBuffer(items);
    }

    public List<Item> GetItemDataFromStageSO(StageSO stage)
    {
        List<Item> deck = new List<Item>();
        foreach (var item in stage.StageData)
        {
            deck.Add(item.itemSo.item.ShallowCopy());
        }
        return deck;
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
