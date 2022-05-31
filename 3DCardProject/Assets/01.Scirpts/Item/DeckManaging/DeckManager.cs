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
            for (int j = 0; j < item.count; j++)
                itemBuffer.Add(item);
        }

        // Shuffle
        for (int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = UnityEngine.Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;

        }
    }
   
}
