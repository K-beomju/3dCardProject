using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDeckManager : DeckManager
{
    public ItemArraySO shopList;
    protected override void Awake()
    {
        base.Awake();
        SetupItemBuffer(shopList.items);
    }
}
