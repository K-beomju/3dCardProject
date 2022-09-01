using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerGameData : ISerializeble
{
    public bool isFirst;


    public Action OnStageChange;
    public Action OnHpChange;
    public Action OnMoneyChange;
    public Action OnDisposableItemChange;
    [SerializeField]
    private int stageValue = 0;
    public int StageValue
    {
        get
        {
            return stageValue;
        }
        set
        {
            stageValue = value;
            OnStageChange?.Invoke();
        }
    }

    [SerializeField]
    private int hp;
    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            OnHpChange?.Invoke();
        }
    }

    [SerializeField]
    private int money = 0;
    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
            OnMoneyChange?.Invoke();

        }
    }
    [SerializeField]
    private int disposableItem = 0b0000_0000;
    public int DisposableItem
    {
        get
        {
            return disposableItem;
        }
        set
        {
            disposableItem = value;
            OnDisposableItemChange?.Invoke();

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
