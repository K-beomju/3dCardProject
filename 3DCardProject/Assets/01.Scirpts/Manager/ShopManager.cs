using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopManager : Singleton<ShopManager>
{
    private DeckManager deckManager;
    [SerializeField]
    private List<Transform> cardPosTrm;
    [SerializeField]
    private List<Card> cardList;
    [SerializeField]
    private List<TextMeshProUGUI> priceTextrList;
    [SerializeField]
    private List<Text>  dummyTextList;

    [SerializeField]
    private Transform cardSpawnTrm;
    [SerializeField]
    private TextMeshProUGUI curMoneyText;

    public int cardCount = 3;

    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private bool isPurchaseAble = true;
    [SerializeField]
    private List<Card> shopCardList = new List<Card>();
    // 튜토리얼
    [SerializeField]
    private bool isTutorial;
    [SerializeField]
    private bool isTutorialDone =false;
    [SerializeField]
    private bool isPurchase = false;
    private void Start()
    {
        deckManager = GetComponent<DeckManager>();
        if (!isTutorial)
        {
            StartCoroutine(StartProcess());
        }
        else
        {
            StartCoroutine(ShopTutorialProcess());
        }

        SaveManager.Instance.gameData.OnMoneyChange -= RefreshMoneyInfo;
        SaveManager.Instance.gameData.OnMoneyChange += RefreshMoneyInfo;
        RefreshMoneyInfo();
        exitButton.onClick.AddListener(()=> {
            // 스테이지로 돌아가기
            if (!isTutorial)
            {
                Global.LoadScene.LoadScene("Stage", () => { StageManager.Instance.OnLoadStageScene?.Invoke(); StageManager.Instance.SceneState = SceneState.STAGE; });
            }
            else if(isTutorial && isTutorialDone)
            {
                Global.LoadScene.LoadScene("Tutorials");
            }


        });
    }
   
    private IEnumerator ShopTutorialProcess()
    {
        TutorialManager.Instance.isTutorial = true;
        isTutorialDone = false;
        isPurchaseAble = false;
       
        yield return TutorialManager.Instance.ExplainCol("이곳은 상점입니다.", 0);
        yield return StartProcess();
        yield return TutorialManager.Instance.ExplainCol("일회성 아이템을 구입할 수 있습니다.", 250);
        yield return TutorialManager.Instance.ExplainCol("가운데 카드를 아래의 블록에 끌어놓아 구입합시다.", 250);
        isPurchaseAble = true;
        TutorialManager.Instance.Fade(true);
        foreach (var card in shopCardList)
        {
            card.isPlayerCard = false;
        }
        shopCardList[1].SelectOutlineCard();
        shopCardList[1].isPlayerCard = true;
        SaveManager.Instance.gameData.Money += shopCardList[1].item.Price;

        yield return WaitBeforePurchase();

        SaveManager.Instance.gameData.Money = 0;

        yield return TutorialManager.Instance.ExplainCol("잘하셨습니다.", 0);
        yield return TutorialManager.Instance.ExplainCol("구매한 아이템은 \"전투\"에서 사용하실수 있습니다.", 0);
        yield return TutorialManager.Instance.ExplainCol("\"Exit\"를(을) 눌러 돌아갑시다.", 0);

        isTutorialDone = true;
    }
    private IEnumerator WaitBeforePurchase()
    {
        while (!isPurchase)
        {
            yield return new WaitForEndOfFrame();
        }
        
        yield break;
    }
    public IEnumerator StartProcess()
    {
       
        yield return new WaitForSeconds(.7f);

        for (int i = 0; i < cardCount; i++)
        {
            yield return new WaitForSeconds(.5f);
            AddCard();
        }
        yield return new WaitForSeconds(.9f);
        yield break;
    }
    public void RefreshMoneyInfo()
    {
        curMoneyText.text = "$" + SaveManager.Instance.gameData.Money.ToString();
    }
    public void AddCard()
    {
        Item popItem = deckManager.PopItem();
        if (popItem != null)
        {
            Card card = CreateCard(popItem, true);

            for (int i = 0; i < cardPosTrm.Count; i++)
            {
                if (cardList[i] == null)
                {
                    cardList[i] = card;
                    card.transform.position = cardSpawnTrm.position;
                    dummyTextList[i].text = "";
                    shopCardList.Add(card);

                    Sequence seq = DOTween.Sequence();
                    seq.Append(card.transform.DOScale(.8f, 0.15f));
                    seq.Join(card.transform.DORotate(cardPosTrm[i].rotation.eulerAngles, .3f));
                    seq.Join(card.transform.DOMove(cardPosTrm[i].position, .3f));
                    seq.OnComplete(()=>card.Emphasize(() => {
                        dummyTextList[i].DOText($"${card.item.Price}", .3f).OnUpdate(() =>
                        {
                            priceTextrList[i].text = dummyTextList[i].text;
                        });
                    }));
                    
                    break;
                }
            }

        }

    }

    public Card CreateCard(Item item, bool isPlayerCard)
    {
        var card = Global.Pool.GetItem<Card>();
        
        card.isDisposable = true;
        card.Setup(item, true, isPlayerCard);
        card.GetComponent<Order>().SetOriginOrder(1);
        return card;
    }

    public bool Purchase(Item inItem)
    {
        if(SaveManager.Instance.gameData.Money >= inItem.Price && isPurchaseAble)
        {
            Debug.Log($"구입 성공 : {inItem.itemName} ");
            isPurchase = true;
            for (int i = 0; i < cardList.Count; i++)
            {
                if(cardList[i].item == inItem)
                {
                    priceTextrList[i].text = "SoldOut";
                    cardList[i] = null;
                    break;
                }
            }
            SaveManager.Instance.gameData.Money -= inItem.Price;
            return true;
        }
        else
        {
            Debug.Log($"구입 실패: {inItem.itemName} ");
            return false;
        }
    }
}
