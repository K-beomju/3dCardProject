using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerDataInfo : MonoBehaviour
{
    [SerializeField] private Text hpText;
    [SerializeField] private Text goldText;
    [SerializeField] private Coin coin;
    [SerializeField] private GameObject player;


    [SerializeField] private CanvasGroup coinGroup;
    #region child
    [SerializeField] private Text coinText;
    [SerializeField] private Text plusText;
    [SerializeField] private Image coinImage;
    #endregion

    [SerializeField] private Ease ease;
    [SerializeField] private Camera mainCam;

    [SerializeField] private CanvasGroup topPanel;
    [SerializeField] private Text topText;
    private RectTransform topTextRtm;

    [SerializeField] private Sprite goldSprite;
    [SerializeField] private Sprite hpSprite;


    public Ease topEase;
    public int value; // 여기에 얻은 골드를 넣고 실행 시키면 됌 

    private void Awake()
    {
        Global.Pool.CreatePool<Coin>(coin.gameObject, player.transform);
        topTextRtm = topText.GetComponent<RectTransform>();
        topPanel.gameObject.SetActive(false);
        SaveManager.Instance.gameData.OnMoneyChange += DataInfoScreen;
        SaveManager.Instance.gameData.OnHpChange += DataInfoScreen;

    }


    private void Start()
    {
        DataInfoScreen();
    }

    public void DataInfoScreen()
    {
        hpText.text = SaveManager.Instance.gameData.Hp.ToString();
        goldText.text = SaveManager.Instance.gameData.Money.ToString();
    }

    [ContextMenu("GetGoldIncreaseDirect")]
    public void GetGoldIncreaseDirect()
    {
        StartCoroutine(GetGoldIncreaseDirectCo(value, true));
    }

    public void GetHpDecreaseDirect(int value)
    {
        StartCoroutine(GetGoldIncreaseDirectCo(value, false));
    }

    [ContextMenu("GetGoldDecreaseDirect")]
    public void GetGoldDecreaseDirect()
    {
        StartCoroutine(GetGoldDecreaseDirectCo(value));
    }

    private IEnumerator GetGoldIncreaseDirectCo(int value = 0, bool isGold = true)
    {
        coinImage.sprite = isGold ? goldSprite : hpSprite;

        if (isGold)
        {
            for (int i = 0; i < value; i++)
            {
                yield return new WaitForSeconds(.15f);
                Sequence coinSeq = DOTween.Sequence();
                Coin coin = Global.Pool.GetItem<Coin>();
                coin.transform.position = player.transform.position + new Vector3(0, 2.5f, 0);
                coin.transform.rotation = Quaternion.Euler(90, 0, 0);

                coinSeq.Append(coin.transform.DOMoveY(0.4f, .4f));
                coinSeq.Join(coin.transform.DOLocalRotate(new Vector3(90, 90, 0), .4f));
                coinSeq.AppendCallback(() =>
                {
                    Debug.Log("왜 안되냐고");
                    BoardManager.Instance.GetCoinEffect(coin.transform);
                    coin.gameObject.SetActive(false);

                });
                coinSeq.Play();
                SoundManager.Instance.PlayFXSound("GetGold", 0.2f);
            }

        }

        yield return new WaitForSeconds(1f);
        plusText.text = value > 0 ? "+" : "-";
        var camVec = mainCam.WorldToScreenPoint(player.transform.position + new Vector3(0.5f, 0.8f, 0));
        coinGroup.transform.position = camVec;
        coinText.text = Mathf.Abs(value).ToString();

        Sequence mySeq = DOTween.Sequence();
        mySeq.Append(coinImage.transform.DOMoveY(camVec.y + 150, .3f).SetEase(ease).SetLoops(2, LoopType.Yoyo))
            .Join(coinImage.DOFade(1, 0.3f))
            .Join(coinImage.transform.DORotate(new Vector3(0, 0, 15), .3f))
            .Insert(0.3f, coinImage.transform.DORotate(new Vector3(0, 0, -15), .3f))
            .Insert(0.6f, coinImage.transform.DORotate(new Vector3(0, 0, 0), .3f));
        mySeq.Insert(0.1f, plusText.transform.DOMoveY(camVec.y + 150, .3f).SetEase(ease).SetLoops(2, LoopType.Yoyo))
            .Join(plusText.DOFade(1, 0.3f))
            .Join(plusText.transform.DORotate(new Vector3(0, 0, -7), .3f))
            .Insert(0.3f, plusText.transform.DORotate(new Vector3(0, 0, 7), .3f))
            .Insert(0.6f, plusText.transform.DORotate(new Vector3(0, 0, 0), .3f));
        mySeq.Insert(0.2f, coinText.transform.DOMoveY(camVec.y + 150, .3f).SetEase(ease).SetLoops(2, LoopType.Yoyo))
            .Join(coinText.DOFade(1, 0.3f))
            .Join(coinText.transform.DORotate(new Vector3(0, 0, 10), .3f))
            .Insert(0.3f, coinText.transform.DORotate(new Vector3(0, 0, -10), .3f))
            .Insert(0.6f, coinText.transform.DORotate(new Vector3(0, 0, 0), .3f))
            .Insert(0.3f, coinGroup.DOFade(1, 1).OnComplete(() =>
            {
                coinGroup.transform.DOMoveY(camVec.y + 10, .3f);
                coinGroup.DOFade(0, 0.3f);
                DataInfoScreen();
            }));

        mySeq.Play();
    }

    private IEnumerator GetGoldDecreaseDirectCo(int value)
    {
        coinImage.sprite = goldSprite;
        for (int i = 0; i < value; i++)
        {
            yield return new WaitForSeconds(0.18f);
            coin = Global.Pool.GetItem<Coin>();
            coin.transform.position = player.transform.position + new Vector3(0, 1, 0);
            coin.transform.rotation = Quaternion.Euler(90, 0, mainCam.transform.eulerAngles.x + 50);
            coin.transform.DOMoveY(3f, 1.5f).SetEase(Ease.Linear);
        }

        yield return new WaitForSeconds(1f);
        plusText.text = "-";
        var camVec = mainCam.WorldToScreenPoint(player.transform.position + new Vector3(0.5f, 0.8f, 0));
        coinGroup.transform.position = camVec;
        coinText.text = value.ToString();

        Sequence mySeq = DOTween.Sequence();
        mySeq.Append(coinImage.transform.DOMoveY(camVec.y + 150, .3f).SetEase(ease).SetLoops(2, LoopType.Yoyo))
            .Join(coinImage.DOFade(1, 0.3f))
            .Join(coinImage.transform.DORotate(new Vector3(0, 0, 15), .3f))
            .Insert(0.3f, coinImage.transform.DORotate(new Vector3(0, 0, -15), .3f))
            .Insert(0.6f, coinImage.transform.DORotate(new Vector3(0, 0, 0), .3f));
        mySeq.Insert(0.1f, plusText.transform.DOMoveY(camVec.y + 150, .3f).SetEase(ease).SetLoops(2, LoopType.Yoyo))
            .Join(plusText.DOFade(1, 0.3f))
            .Join(plusText.transform.DORotate(new Vector3(0, 0, -7), .3f))
            .Insert(0.3f, plusText.transform.DORotate(new Vector3(0, 0, 7), .3f))
            .Insert(0.6f, plusText.transform.DORotate(new Vector3(0, 0, 0), .3f));
        mySeq.Insert(0.2f, coinText.transform.DOMoveY(camVec.y + 150, .3f).SetEase(ease).SetLoops(2, LoopType.Yoyo))
            .Join(coinText.DOFade(1, 0.3f))
            .Join(coinText.transform.DORotate(new Vector3(0, 0, 10), .3f))
            .Insert(0.3f, coinText.transform.DORotate(new Vector3(0, 0, -10), .3f))
            .Insert(0.6f, coinText.transform.DORotate(new Vector3(0, 0, 0), .3f))
            .Insert(0.3f, coinGroup.DOFade(1, 1).OnComplete(() =>
            {
                coinGroup.transform.DOMoveY(camVec.y + 10, .3f);
                coinGroup.DOFade(0, 0.3f);
                DataInfoScreen();

            }));

        mySeq.Play();
    }



    [ContextMenu("ShowTopPanel")]
    public void ShowTopPanel(string text)
    {
        topPanel.gameObject.SetActive(true);

        topPanel.alpha = 0;
        topPanel.transform.localScale = new Vector3(5, 2, 2);
        topTextRtm.anchoredPosition = new Vector2(-10, 0);
        topText.DOFade(0, 0);
        topText.text = text;

        Sequence mySeq = DOTween.Sequence();
        mySeq.Append(topPanel.DOFade(1, 0.7f))
            .Join(topPanel.transform.DOScale(new Vector3(20, 2, 2), 0.3f));
        mySeq.Insert(.5f, topTextRtm.DOAnchorPosX(0, 0.3f).SetEase(topEase))
            .Join(topText.DOFade(1, 0.3f));
        mySeq.Insert(2, topTextRtm.DOAnchorPosX(10, 0.3f))
            .Join(topText.DOFade(0, 0.3f))
            .Append(topPanel.transform.DOScale(new Vector3(20, 0, 0), 0.2f));


        mySeq.Play();
    }
}
