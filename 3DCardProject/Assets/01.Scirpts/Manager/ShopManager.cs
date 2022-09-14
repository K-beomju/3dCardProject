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
    // Ʃ�丮��
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
            // ���������� ���ư���
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
       
        yield return TutorialManager.Instance.ExplainCol("\"����\"Ʃ�丮���Դϴ�.", 0);
        yield return StartProcess();
        yield return TutorialManager.Instance.ExplainCol("��ȸ�� �������� ������ �� �ֽ��ϴ�.", 250);
        yield return TutorialManager.Instance.ExplainCol("��� ī�带 �Ʒ��� ��Ͽ� ������� �����սô�.", 250);
        isPurchaseAble = true;
        TutorialManager.Instance.Fade(true);
        foreach (var card in shopCardList)
        {
            card.isPlayerCard = false;
        }
        shopCardList[1].SelectOutlineCard();
        shopCardList[1].isPlayerCard = true;
        SaveManager.Instance.gameData.Money += shopCardList[1].item.Price;

        yield return new WaitWhile(()=>!isPurchase);

        SaveManager.Instance.gameData.Money = 0;

        yield return TutorialManager.Instance.ExplainCol("���ϼ̽��ϴ�.", 0);
        yield return TutorialManager.Instance.ExplainCol("������ �������� \"����\"���� ����ϽǼ� �ֽ��ϴ�.", 0);
        yield return TutorialManager.Instance.ExplainCol("\"Exit\"��(��) ���� ���ư��ô�.", 0);

        isTutorialDone = true;
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
            Debug.Log($"���� ���� : {inItem.itemName} ");
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
            Debug.Log($"���� ����: {inItem.itemName} ");
            return false;
        }
    }
}
