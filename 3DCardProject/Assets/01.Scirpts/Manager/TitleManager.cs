using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class TitleManager : Singleton<TitleManager>
{
    [SerializeField]
    private Transform[] cardPosTrm;
    // 시작, 나가기, 메뉴
    [SerializeField]
    private ItemSO[] cards;

    [SerializeField]
    private UnityEngine.UI.Button resetButton;

     private Camera mainCam;
    [SerializeField] private Ease camEase;
    [SerializeField] private Light dirLight;
    public Field titleField;
    private void Start()
    {
        mainCam = Camera.main;
        resetButton.onClick.AddListener(ResetIsFirstData);
        for (int i = 0; i < cardPosTrm.Length; i++)
        {
            if (cards.Length == i) break;
           /* if( 1 == i && SaveManager.Instance.gameData.IsFirst)
            {
                continue;
            }*/
            Card card = Global.Pool.GetItem<Card>();
            card.transform.localScale = new Vector3(.35f, .35f, .35f);
            card.Setup(cards[i].item, true, true);
            card.transform.position = cardPosTrm[i].position;
            card.transform.rotation = cardPosTrm[i].rotation;
            card.originPRS = new PRS(card.transform);
        }
        SoundManager.Instance.PlayBGMSound("Title", 0);
        StartCoroutine(TitleFieldBlink());
    }

    private IEnumerator TitleFieldBlink()
    {
        for (int i = 0; i < 50; i++)
        {

            yield return new WaitForSeconds(0.5f);
            titleField.GetComponent<Outline>().enabled = !titleField.GetComponent<Outline>().enabled;
        }

    }
    [ContextMenu("ResetIsFirstData")]
    public void ResetIsFirstData()
    {
        SaveManager.Instance.gameData.IsFirst = true;
        SaveManager.Instance.gameData.IsTutorialDone = false;
    }

    public void CameraMoveAction(Action action = null)
    {
        Sequence mySeq = DOTween.Sequence();
        mySeq.Append(mainCam.transform.DOMove(new Vector3(0, 6.34f, 13f), 3).SetEase(camEase).SetUpdate(true));
        dirLight.DOIntensity(0, 2).SetDelay(2);
        mySeq.AppendInterval(2f);
        mySeq.InsertCallback(2.5f, () => action());
        mySeq.InsertCallback(2, () => SoundManager.Instance.PlayFXSound("StartGame", 0.2f));
     
    }

}
