using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
    public Action OnWinGame;
    public Action OnLoseGame;

    protected override void Awake()
    {
        base.Awake();
        OnWinGame += CallOnWinGame;
        OnLoseGame += CallOnLoseGame;
    }

    public void CallOnWinGame()
    {
        print("GAMEWIN");
    }
    public void CallOnLoseGame()
    {
        print("GAMELOSE");
    }
}
