using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerGameData : ISerializeble
{
    public bool isFirst = true;
    public bool isTutorialDone = false;


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
            if(hp <= 0)
            {

            }
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
    public Item DisposableItem
    {
        get
        {
            uint uid = 0;
            switch (disposableItem)
            {
                case 0b1000_0000:
                    uid = 100;
                    break;
                case 0b0100_0000:
                    uid = 101;
                    break;
                case 0b0010_0000:
                    uid = 102;
                    break;
                case 0b0001_0000:
                    uid = 103;
                    break;
                case 0b0000_1000:
                    uid = 104;
                    break;
                case 0b0000_0100:
                    uid = 105;
                    break;
                case 0b0000_0010:
                    uid = 106;
                    break;
                case 0b0000_0001:
                    uid = 107;
                    break;
            }

            return SaveManager.Instance.saveDeckData.FindItem(uid); ;
        }
        set
        {
            int item = 0b0000_0000;
            if(value != null)
            {
                switch (value.uid)
                {

                    case 100:
                        item = 0b1000_0000;
                        break;
                    case 101:
                        item = 0b0100_0000;
                        break;
                    case 102:
                        item = 0b0010_0000;
                        break;
                    case 103:
                        item = 0b0001_0000;
                        break;
                    case 104:
                        item = 0b0000_1000;
                        break;
                    case 105:
                        item = 0b0000_0100;
                        break;
                    case 106:
                        item = 0b0000_0010;
                        break;
                    case 107:
                        item = 0b0000_0001;
                        break;
                }
            }
          
            disposableItem = item;
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
