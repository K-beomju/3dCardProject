using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

[System.Serializable]
public enum TurnType
{
    Player,
    Change,
    Enemy
}

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField] private TurnType type;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    // ÅÏ ¹Ù²Þ 
    public static void ChangeTurn(TurnType _type)
    {
        if (_type == TurnType.Player)
        {
            Instance.mainCam.transform.DOMove(new Vector3(0,1, -10.5f), 0.5f).OnComplete(() => 
            {
                Instance.type = _type;
                PlayerManager.TurnReset();
            });
            Instance.mainCam.transform.DORotate(new Vector3(45, 0, 0), 0.5f);
            CardManager.Instance.MyCardMove(false);

        }
        else
        {
            Instance.mainCam.transform.DOMove(new Vector3(0, 5.9f, 0.3f), 0.5f).OnComplete(() => Instance.type = _type);
            Instance.mainCam.transform.DORotate(new Vector3(75, 0, 0), 0.5f);
            CardManager.Instance.MyCardMove(true);

        }
    }

    public static void PlayerCardMove()
    {
        Instance.mainCam.transform.DOMove(new Vector3(0, 5.9f, 0.3f), 0.5f);
        Instance.mainCam.transform.DORotate(new Vector3(75, 0, 0), 0.5f);
    }

    public static TurnType CurReturnType()
    {
        return Instance.type;
    }

    public static void CurChangeType(TurnType type)
    {
        Instance.type = type;
    }




}
