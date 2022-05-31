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
    Enemy
}

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField] private TurnType type;
    private Camera mainCam;
    public static bool isClick = true;

    private void Start()
    {
        mainCam = Camera.main;
    }

    // 턴 바꿈 
    public static void ChangeTurn(TurnType _type, ref bool isClick)
    {
        Instance.type = _type;
        if (_type == TurnType.Player)
        {
            Instance.mainCam.transform.DOMoveZ(-10.5f, 0.5f).OnComplete(() => PlayerManager.TurnReset());
            Instance.mainCam.transform.DORotate(new Vector3(45, 0, 0), 0.5f);
            isClick = true;
        }
        else
        {
            isClick = false;
            Instance.mainCam.transform.DOMoveZ(-5f, 0.5f);
            Instance.mainCam.transform.DORotate(new Vector3(55, 0, 0), 0.5f);
        }
    }

 

    // 현재 타입 반환 
    public static TurnType CurReturnType()
    {
        return Instance.type;
    }

 

}
