using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private bool isActive = true;
    public static bool isReflect = false;


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
            if(item.GetComponent<UICard>().linkedCard == card)
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
        
        Card card = inGO.GetComponent<UICard>().linkedCard;
        reflectCard = card;

        if (!card.item.IsStructCard)
        {
            card.OnAttack();
        }
        else
        {
            // 여기에 필드 선택해서 선택한 필드에 SetUP(card) 해줘야 함
            isReflect = true;
        }

        CardUIList.Remove(inGO);
        Destroy(inGO);
    }

    public void CallOnReflect()
    {
        if(CardUIList.Count > 0)
        {
            // 올리기
            ReflectBoxActive(true);
        }
    }

    public void ConfirmReflect()
    {
        if(selectedCard != null)
        {
            RemoveCardUI(selectedCard.gameObject);

            var a = selectedCard.linkedCard;
            CardManager.Instance.myCards.Remove(a);
            a?.GetComponent<Order>().SetOriginOrder(0);

            CardManager.Instance.SetOriginOrder();
            CardManager.Instance.CardAlignment();
            CardManager.Instance.CardDie(a);
            //CardManager.Instance.CardAlignment();
            // 박스 내리기
            ReflectBoxActive(false);
            TurnManager.Instance.ChangeTurn();
        }
        else
        {
            Debug.LogWarning("카드를 선택해야 합니다");
        }
    }
    public void CancelReflect()
    {
        CardManager.Instance.WaitingActionUntilFinishOnReflect?.Invoke();
        ReflectBoxActive(false);
    }
    public void ReflectBoxActive(bool inBool)
    {
        transform.DOMoveY(250 * (isActive ? -1 : 1), 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            isActive = inBool;
            activeBTN.transform.parent.gameObject.SetActive(inBool);
            confirmBTN.transform.parent.gameObject.SetActive(inBool);
            cancelBTN.transform.parent.gameObject.SetActive(inBool);
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
