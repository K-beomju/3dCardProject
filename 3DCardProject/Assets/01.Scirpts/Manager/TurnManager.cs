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

    // ≈œ πŸ≤ﬁ 
    public static void ChangeTurn(TurnType _type)
    {
        if (_type == TurnType.Player)
        {
           Instance.type = _type;
           PlayerManager.TurnReset();
           CardManager.Instance.MyCardMove(false);

        }
        else
        {
            FindObjectOfType<CameraMove>().isLock = true;
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
