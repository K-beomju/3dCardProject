using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    private int stageValue;
    public int StageValue
    {
        get
        {
            return stageValue;
        }
        set
        {
            stageValue = value;
            SecurityPlayerPrefs.SetInt("game.stageValue", stageValue);
        }
    }

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
            SecurityPlayerPrefs.SetInt("game.hp", hp);
        }
    }

    private int money;
    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
            SecurityPlayerPrefs.SetInt("game.money", money);
        }
    }

    public Data()
    {
        InitData();
    }
    public void InitData()
    {
        stageValue = SecurityPlayerPrefs.GetInt("game.stageValue", 0);
        hp = SecurityPlayerPrefs.GetInt("game.hp", 10);
        money = SecurityPlayerPrefs.GetInt("game.money", 0);
    }
}
