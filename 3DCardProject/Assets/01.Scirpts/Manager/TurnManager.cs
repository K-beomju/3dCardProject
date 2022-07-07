using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.UI;

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
    [SerializeField] private CanvasGroup changePanel;
    [SerializeField] private Text changeText;

    public bool CanChangeTurn { get; set; } = true;

    private void Start()
    {
        mainCam = Camera.main;
        cameraMove = mainCam.GetComponent<CameraMove>();
    }

    public void ChangeTurn()
    {
        if (!CanChangeTurn) return;

        if (type == TurnType.Player)
        {
            ChangeTurn(TurnType.Enemy);
        }
        else
        {
            ChangeTurn(TurnType.Player);
        }

        ChangeTurnPanel();
    }

    // 턴 바꿈 
    public static void ChangeTurn(TurnType _type)
    {
        if (!Instance.CanChangeTurn) return;

        if (Instance.type != _type)
        {
            Instance.type = _type;

            if (Instance.type != TurnType.Player)
            {
                EnemyAI.Instance.JudgementCard();
                //EnemyManager.Instance.EnemyAction();
            }
            else
            {
                CardManager.Instance.AddCard();
            }
            
            Instance.ChangeTurnPanel();

        }
        /*if (_type == TurnType.Player)
        {
           Instance.type = _type;
           PlayerManager.TurnReset();
        }
        else
        {
            Instance.cameraMove.isLock = false;
            FindObjectOfType<CameraMove>().isLock = true;
            Instance.mainCam.transform.DOMove(new Vector3(0, 5.9f, 0.3f), 0.3f).OnComplete(() => Instance.type = _type);

        }*/
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

    public void ChangeTurnPanel()
    {
        if (Instance.type == TurnType.Player)
        {
            changeText.text = "내 턴";
            changePanel.DOFade(1, .5f).OnComplete(() => changePanel.DOFade(0, .5f));
        }
        else
        {
            changeText.text = "적 턴";
            changePanel.DOFade(1, .5f).OnComplete(() => changePanel.DOFade(0, .5f));

        }
    }

}
