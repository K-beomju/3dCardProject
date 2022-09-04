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

    private void Start()
    {
        deckManager = GetComponent<DeckManager>();
        StartCoroutine(StartProcess());

        SaveManager.Instance.gameData.OnMoneyChange += RefreshMoneyInfo;
        RefreshMoneyInfo();
        exitButton.onClick.AddListener(()=> {
            // 스테이지로 돌아가기
            Global.LoadScene.LoadScene("Stage");
        });
    }
    public IEnumerator StartProcess()
    {
       
        yield return new WaitForSeconds(.7f);

        for (int i = 0; i < cardCount; i++)
        {
            yield return new WaitForSeconds(.5f);
            AddCard();
        }
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
        if(SaveManager.Instance.gameData.Money >= inItem.Price)
        {
            Debug.Log($"구입 성공 : {inItem.itemName} ");
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
