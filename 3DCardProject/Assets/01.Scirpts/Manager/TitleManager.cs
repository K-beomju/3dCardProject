using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class TitleManager : Singleton<TitleManager>
{
    [SerializeField]
    private Transform[] cardPosTrm;
    // ����, ������, �޴�
    [SerializeField]
    private ItemSO[] cards;

    [SerializeField]
    private UnityEngine.UI.Button resetButton;

    [SerializeField] private Camera mainCam;
    [SerializeField] private Ease camEase;

    private void Start()
    {
        mainCam = Camera.main;
        resetButton.onClick.AddListener(ResetIsFirstData);
        for (int i = 0; i < cardPosTrm.Length; i++)
        {
            if (cards.Length == i) break;
            if( 1 == i && SaveManager.Instance.gameData.isFirst)
            {
                continue;
            }
            Card card = Global.Pool.GetItem<Card>();
            card.transform.localScale = new Vector3(.35f, .35f, .35f);
            card.Setup(cards[i].item, true, true);
            card.transform.position = cardPosTrm[i].position;
            card.transform.rotation = cardPosTrm[i].rotation;
            card.originPRS = new PRS(card.transform);
        }
    }

    [ContextMenu("ResetIsFirstData")]
    public void ResetIsFirstData()
    {
        SaveManager.Instance.gameData.isFirst = true;
        SaveManager.Instance.gameData.IsTutorialDone = false;
    }

    public void CameraMoveAction(Action action = null)
    {
        Sequence mySeq = DOTween.Sequence();
        mySeq.Append(mainCam.transform.DOMove(new Vector3(0, 6.34f, 15f), 3).SetEase(camEase).SetUpdate(true));
        mySeq.InsertCallback(2f, () => action());
    }

}
