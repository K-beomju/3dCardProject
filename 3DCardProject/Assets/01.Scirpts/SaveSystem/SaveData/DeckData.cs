using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System;
[Serializable]
public class Deck
{
    [SerializeField]
    private List<Item> deck = new List<Item>();
    public List<Item> DeckData
    {
        get
        {
            return deck;
        }
        set
        {
            deck = value;
        }
    }
    public Deck ShallowCopy()
    {
        return (Deck)this.MemberwiseClone();
    }
}

[Serializable]
public class DeckData : ISerializeble
{
    
    [SerializeField]
    private List<Deck> deckListData = new List<Deck>();

    public List<Deck> DeckList
    {
        get
        {
            return deckListData;
        }
        set
        {
            deckListData = value;
        }
    }


    [SerializeField]
    private Deck deckData = new Deck();

    public Deck CurDeck
    {
        get
        {
            if(deckListData.Count < 1)
            {
                deckData.DeckData = ArraySOToItemList(defaultDeck);
                DeckList.Add(deckData.ShallowCopy());
            }
            return deckData;
        }
        set
        {
            deckData = value;
        }
    }
    [field:SerializeField]
    public ItemArraySO defaultDeck { get; set; }

    public List<Item> ArraySOToItemList(ItemArraySO arraySO)
    {
        Deck deck = new Deck();
        foreach (var item in arraySO.items)
        {
            /*Debug.Log(item);
            Debug.Log(deck.DeckData);*/
            deck.DeckData.Add(item.item.ShallowCopy());
        }
        return deck.ShallowCopy().DeckData;
    }

    public void Desirialize(string jsonString)
    {
        JsonUtility.FromJsonOverwrite(jsonString, this);
    }

    public string GetJsonKey()
    {
        return "GameData";
    }

    public JObject Serialize()
    {
        string jsonString = JsonUtility.ToJson(this);
        JObject returnVal = JObject.Parse(jsonString);

        return returnVal;
    }
}
