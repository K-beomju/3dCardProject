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

    public int value; // 여기에 얻은 골드를 넣고 실행 시키면 됌 

    private void Awake()
    {
        Global.Pool.CreatePool<Coin>(coin.gameObject, player.transform);
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

    [ContextMenu("GetGold")]
    public void GetGoldDirect()
    {
        StartCoroutine(GetGoldIncreaseDirectCo(value));
    }

    private IEnumerator GetGoldIncreaseDirectCo(int value)
    {
        for (int i = 0; i < value; i++)
        {
            yield return new WaitForSeconds(.15f);
            coin = Global.Pool.GetItem<Coin>();
            coin.transform.position = player.transform.position + new Vector3(0, 2.5f, 0);
            coin.transform.rotation = Quaternion.Euler(90, 0, 0);
            coin.transform.DOLocalRotate(new Vector3(90, 90, 0), 1);
            coin.transform.DOMoveY(0.4f, .4f);

        }
        yield return new WaitForSeconds(1f);
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
            })); ;
      
        mySeq.Play();
    }
}
