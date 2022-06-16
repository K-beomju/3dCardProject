using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [field:SerializeField]
    public List<Item> itemBuffer { get; set; } = new List<Item>();
    protected virtual void Awake()
    {
        itemBuffer.Clear();

    }
    public Item GetTopItem()
    {
        if (itemBuffer.Count < 1) return null;

        return itemBuffer[0];
    }
    public Item PopItem()
    {
        if (itemBuffer.Count < 1) return null;

        Item item = itemBuffer[0];
        itemBuffer.RemoveAt(0);
        return item;
    }

    protected virtual void SetupItemBuffer(List<Item> items)
    {
        itemBuffer = new List<Item>();

        // ADD

        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];
            AddCardToItemBuffer(item, item.count);
        }

        // Shuffle
        SuffleItemBuffer();
    }

    protected virtual void AddCardToItemBuffer(Item item,float count)
    {
        for (int j = 0; j < count; j++)
        {
            itemBuffer.Add(item);
        }
    }

    protected virtual void SuffleItemBuffer()
    {
        for (int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = UnityEngine.Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }
}
