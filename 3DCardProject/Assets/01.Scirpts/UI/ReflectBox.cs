using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MEC;
using DG.Tweening;
public class ReflectBox : Singleton<ReflectBox>
{
    [SerializeField]
    private Button activeBTN;
    [SerializeField]
    private Button confirmBTN;
    [SerializeField]
    private Button cancelBTN;

    [SerializeField]
    private GameObject cardUIPrefab = null;

    [SerializeField]
    private Transform cardUIparent;

    public List<GameObject> CardUIList { get; private set; } = new List<GameObject>();

    private bool isActive = false;
    public static bool isReflect = false;

    private Card waitingCard = null;

    public Card WaitingCard
    {
        get
        {
            return waitingCard;
        }
        set
        {
            waitingCard = value;
        }
    }
    public UICard selectedCard = null;
    public Card reflectCard;

    private void Start()
    {
        ReflectBoxActive(false);
        activeBTN.onClick.AddListener(ReflectBoxActive);
        confirmBTN.onClick.AddListener(ConfirmReflect);
        cancelBTN.onClick.AddListener(CancelReflect);
        CardManager.Instance.OnReflect += CallOnReflect;
    }

    public void AddCardUI(Item item,Card card)
    {
        GameObject cardUIGO = Instantiate(cardUIPrefab, cardUIparent);
        cardUIGO.GetComponent<UICard>().Setup(item,card);
        CardUIList.Add(cardUIGO);
    }

    public void RemoveCardUI(Card card)
    {
        GameObject go = null;
        foreach (var item in CardUIList)
        {
            if(item.GetComponent<UICard>().linkedCard.item.itemName == card.item.itemName)
            {
                go = item;
                break;
            }    
        }
        if( go != null)
        {
            RemoveCardUI(go);
        }
    }
    public void RemoveCardUI(GameObject inGO)
    {
        
        CardUIList.Remove(inGO);
        Destroy(inGO);
    }

    public void CallOnReflect()
    {
        StartCoroutine(CallOnReflectCo());
    }

    public IEnumerator CallOnReflectCo()
    {
        if (CardUIList.Count > 0 && GameManager.Instance.State == GameState.RUNNING)
        {
            Vector3 pos = CardManager.Instance.hackField.transform.position;
            pos += new Vector3(0, 5f, 0);
            waitingCard.transform.DOScale(waitingCard.transform.localScale * 2f, .2f);
            waitingCard.transform.DOMove(pos, .4f);
            waitingCard.transform.DORotate(new Vector3(40, 0, 0), .3f);
            yield return new WaitForSeconds(1f);
            // �ø���
            ReflectBoxActive(true);
        }
    }

    public void ConfirmReflect()
    {
        Timing.RunCoroutine(ConfirmReflectProcess());
    }
    private IEnumerator<float> ConfirmReflectProcess()
    {
        if (selectedCard != null)
        {

            var a = selectedCard.linkedCard;
            if (a != null)
            {
                /* foreach (CardActionCondition item in a.item.OnCreate)
                 {
                     if (item.action is CardActionMirrorItemChange)
                     {
                         CardActionMirrorItemChange camic = item.action as CardActionMirrorItemChange;
                         CardManager.Instance.OnChangeLastUsedCard -= (item) => {
                             camic.ChangeCardItem(item);
                         };
                         break;
                     }
                 }*/
                reflectCard = a;

                if (!a.item.IsStructCard)
                {
                    int idx = 0;
                    foreach (CardActionCondition item in a.item.OnSpawn)
                    {
                        if (item.action is CardActionMove)
                        {
                            //CardMoveAction cma = item.action as CardMoveAction;
                            item.action = null;
                            break;
                        }
                        ++idx;
                    }
                    a.OnSpawn();

                    CardManager.Instance.myCards.Remove(a);
                    //a?.GetComponent<Order>().SetOriginOrder(0);

                    CardManager.Instance.SetOriginOrder();
                    CardManager.Instance.CardAlignment();
                    CardManager.Instance.CardDie(a);
                }
                else
                {
                    // ���⿡ �ʵ� �����ؼ� ������ �ʵ忡 SetUP(card) ����� ��
                    isReflect = true;
                }

                if (WaitingCard != null)
                {
                    CardManager.Instance.CardDie(WaitingCard);
                }
                RemoveCardUI(selectedCard.gameObject);


            }

            ReflectBoxActive(false);
            yield return Timing.WaitForSeconds(0.7f);
            NewFieldManager.Instance.AvatarMove(NewFieldManager.Instance.enemyCard.curField, () =>
            {
                TurnManager.ChangeTurn();
            });
            //CardManager.Instance.CardAlignment();
            // �ڽ� ������
        }
        else
        {
            Debug.LogWarning("ī�带 �����ؾ� �մϴ�");
            yield return 0;
        }
    }
    public void CancelReflect()
    {
        CardManager.Instance.WaitingActionUntilFinishOnReflect?.Invoke();
        ReflectBoxActive(false);
    }
    public void ReflectBoxActive(bool inBool)
    {
        transform.DOMoveY(250 * (inBool ? 1 : -1), 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            isActive = inBool;
            activeBTN.transform.parent.gameObject.SetActive(inBool);
            confirmBTN.transform.parent.gameObject.SetActive(inBool);
            cancelBTN.transform.parent.gameObject.SetActive(inBool);
            GetComponent<CanvasGroup>().DOFade(inBool ? 1 : 0,0.1f);
        }
        );
    }
    public void ReflectBoxActive()
    {
        DOTween.Kill(this.gameObject);
        transform.DOMoveY(250 * (isActive ? -1 : 1), 0.2f).SetEase(Ease.OutQuad);
        isActive = !isActive;
    }
}
