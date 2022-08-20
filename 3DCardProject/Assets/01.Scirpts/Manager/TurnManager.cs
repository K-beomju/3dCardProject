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
    Standby,
    Player,
    Change,
    Enemy
}

public class TurnManager : Singleton<TurnManager>
{
    public TurnType type;
    private Camera mainCam;
    private CameraMove cameraMove;
    [SerializeField] private CanvasGroup changePanel;
    [SerializeField] private Text changeText;

    public bool CanChangeTurn { get; set; } = true;

    private void Start()
    {
        mainCam = Camera.main;
        cameraMove = mainCam.GetComponent<CameraMove>();
        type = TurnType.Standby;
    }

    public static void ChangeTurn()
    {
        if (!Instance.CanChangeTurn || GameManager.Instance.State == GameState.END) return;

        if (Instance.type == TurnType.Player)
        {
            ChangeTurn(TurnType.Enemy);
        }
        else
        {
            ChangeTurn(TurnType.Player);
        }

    }

    // 턴 바꿈 
    public static void ChangeTurn(TurnType _type)
    {
        if (!Instance.CanChangeTurn) return;
        if (Instance.type != _type)
        {
            TurnType temp = Instance.type;
            Instance.type = TurnType.Change;

            Instance.ChangeTurnPanel(_type,() =>
            {
                Instance.type = _type;
                if(temp != TurnType.Standby)
                {
                    if (Instance.type != TurnType.Player)
                    {
                        EnemyAI.Instance.JudgementCard();
                    }
                    else
                    {
                        CardManager.Instance.AddCard();
                    }
                }
                    

            });

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

    public void ChangeTurnPanel(TurnType type,Action act)
    {
        StartCoroutine(ChangeTurnPanelCo(type, act));
    }

    public IEnumerator ChangeTurnPanelCo(TurnType type,Action act)
    {
        yield return new WaitForSeconds(1f);
        if (GameManager.Instance.State == GameState.END) yield break;

        Sequence seq = DOTween.Sequence();

        if (type == TurnType.Player)
        {
            changePanel.gameObject.GetComponent<Image>().color  = new Color32(98, 119, 255,174);
            changeText.text = "내 턴";
        }
        else
        {
            changePanel.gameObject.GetComponent<Image>().color = new Color32(255, 97, 97, 174);
            changeText.text = "적 턴";
        }
        seq.Append(changePanel.DOFade(1, .3f));
        seq.AppendInterval(1.2f);
        seq.Append(changePanel.DOFade(0, .3f));
        seq.OnComplete(() => act?.Invoke());
    }

}
