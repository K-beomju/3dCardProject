using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class PlayerDeckData : ISerializeble
{
    private List<Item> deckData = new List<Item>();

    public List<Item> DeckData
    {
        get
        {
            return deckData;
        }
        private set
        {
            deckData = value;
        }
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
