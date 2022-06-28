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
    private CameraMove cameraMove;

    private void Start()
    {
        mainCam = Camera.main;
        cameraMove = mainCam.GetComponent<CameraMove>();
    }

    public void ChangeTurn()
    {
        if (type == TurnType.Player)
        {
            type = TurnType.Enemy;
            EnemyManager.Instance.EnemyAction();
        }
        else
        {
            type = TurnType.Player;
            CardManager.Instance.AddCard();
        }
    }

    // ≈œ πŸ≤ﬁ 
    public static void ChangeTurn(TurnType _type)
    {
        if (_type == TurnType.Player)
        {
           Instance.type = _type;
           PlayerManager.TurnReset();
           //CardManager.Instance.MyCardMove(false);
        }
        else
        {
            Instance.cameraMove.isLock = false;
            FindObjectOfType<CameraMove>().isLock = true;
            Instance.mainCam.transform.DOMove(new Vector3(0, 5.9f, 0.3f), 0.3f).OnComplete(() => Instance.type = _type);
            //CardManager.Instance.MyCardMove(true);

        }
    }

    public static void PlayerCardMove()
    {
        Instance.mainCam.transform.DOMove(new Vector3(0, 5.9f, 0.3f), 0.3f);
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
