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
    private TurnType type;
    public TurnType Type
    {
        get
        {
            return type;
        }
        set
        {
            if(type != value)
            {
                type = value;
                if(type == TurnType.Player || type == TurnType.Enemy)
                {
                    Card playerCard = NewFieldManager.Instance.playerCard;
                    Card enemyCard = NewFieldManager.Instance.enemyCard;
                    CanvasGroup gc = nameTagObj.GetComponent<CanvasGroup>();
                    Sequence seq = DOTween.Sequence();
                    Outline outline = null;

                    seq.Append(gc.DOFade(0, .1f).OnUpdate(()=> {
                        if (type == TurnType.Player)
                        {
                            enemyCard.LinkedModel.ModelObject.GetComponentInChildren<Outline>().OutlineWidth = gc.alpha;
                        }
                        else if (type == TurnType.Enemy)
                        {
                            playerCard.LinkedModel.ModelObject.GetComponentInChildren<Outline>().OutlineWidth = gc.alpha;
                        }
                    }));
                    seq.AppendCallback(() => {
                        if (type == TurnType.Player)
                        {
                            mainCard = playerCard;
                            UnitNameText.text = "Player";
                            UnitNameText.color = Utils.PlayerColor;
                            outline = playerCard.LinkedModel.ModelObject.GetComponentInChildren<Outline>();
                            outline.OutlineColor = Utils.PlayerColor;
                        }
                        else if (type == TurnType.Enemy)
                        {
                            mainCard = enemyCard;
                            UnitNameText.text = "Enemy";
                            UnitNameText.color = Utils.EnemyColor;
                            outline = enemyCard.LinkedModel.ModelObject.GetComponentInChildren<Outline>();
                            outline.OutlineColor = Utils.EnemyColor;
                        }
                    });
                    seq.Append(gc.DOFade(1, .1f).OnUpdate(()=> {
                        if(outline != null)
                        {
                            outline.OutlineWidth = gc.alpha * 5f;
                        }
                    }));
                }
             
            }
         
           
        }
    }

    private Camera mainCam;
    private CameraMove cameraMove;
    [SerializeField] private CanvasGroup changePanel;
    private GameObject nameTagObj;
    [SerializeField] private Text changeText;
    private TextMeshProUGUI UnitNameText;
    private Card mainCard;

    public bool CanChangeTurn { get; set; } = true;

    private void Start()
    {
        if(StageManager.Instance.SceneState == SceneState.BATTLE)
        {
            mainCam = Camera.main;
            cameraMove = mainCam.GetComponent<CameraMove>();
            nameTagObj = Instantiate(Resources.Load<GameObject>("NameTag"), GameObject.Find("WorldCardCanvas").transform);
            UnitNameText = nameTagObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            Type = TurnType.Standby;
        }
      
    }
    private void Update()
    {
        if(mainCard != null)
        {
            nameTagObj.transform.position = mainCard.LinkedModel.ModelObject.transform.position;
        }
    }
    public static void ChangeTurn()
    {
        if (!Instance.CanChangeTurn || GameManager.Instance.State == GameState.END) return;

        if (Instance.Type == TurnType.Player)
        {
            ChangeTurn(TurnType.Enemy);
        }
        else
        {
            ChangeTurn(TurnType.Player);
        }

    }

    // �� �ٲ� 
    public static void ChangeTurn(TurnType _type)
    {
        if (!Instance.CanChangeTurn) return;
        if (Instance.Type != _type)
        {
            TurnType temp = Instance.Type;
            Instance.Type = TurnType.Change;

            Instance.ChangeTurnPanel(_type,() =>
            {
                Instance.Type = _type;
                if(temp != TurnType.Standby)
                {
                    if (Instance.Type != TurnType.Player)
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
        return Instance.Type;
    }

    public static void CurChangeType(TurnType type)
    {
        Instance.Type = type;
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
            changePanel.gameObject.GetComponent<Image>().color = new Color32(Utils.PlayerColor.r, Utils.PlayerColor.g, Utils.PlayerColor.b, 100);
            changeText.text = "�� ��";
        }
        else
        {
            changePanel.gameObject.GetComponent<Image>().color = new Color32(Utils.EnemyColor.r, Utils.EnemyColor.g, Utils.EnemyColor.b, 100);
            changeText.text = "�� ��";
        }
        seq.Append(changePanel.DOFade(1, .3f));
        seq.AppendInterval(1.2f);
        seq.Append(changePanel.DOFade(0, .3f));
        seq.OnComplete(() => act?.Invoke());
    }

}
