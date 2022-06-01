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
            Instance.mainCam.transform.DOMoveZ(-2f, 0.5f).OnComplete(() => 
            {
                Instance.type = _type;
                PlayerManager.TurnReset();
            });
            Instance.mainCam.transform.DORotate(new Vector3(75, 0, 0), 0.5f);
        }
        else
        {
            Instance.mainCam.transform.DOMoveZ(-5f, 0.5f).OnComplete(() => Instance.type = _type);
            Instance.mainCam.transform.DORotate(new Vector3(55, 0, 0), 0.5f);
        }
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
