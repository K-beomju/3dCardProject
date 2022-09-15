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

    private bool isActive = false;
    //public static bool isReflect = false;

    private CanvasGroup canvasGroup;

    public bool isCardOnHand;

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
    public UICard uiCard = null;

    private void Start()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        activeBTN.onClick.AddListener(ReflectBoxActive);
        confirmBTN.onClick.AddListener(ConfirmReflect);
        cancelBTN.onClick.AddListener(CancelReflect);
        CardManager.Instance.OnReflect += CallOnReflect;
        uiCard.gameObject.SetActive(false);
        ReflectBoxActive(false, ()=>uiCard.gameObject.SetActive(true));

    }

    public void SetUpCard(Card card)
    {
        isCardOnHand = true;
        uiCard.Setup(card);
    }

    public void CallOnReflect()
    {
        StartCoroutine(CallOnReflectCo());
    }

    public IEnumerator CallOnReflectCo()
    {
        if (TutorialManager.Instance != null && TutorialManager.Instance.isTutorial)
        {
            if (BattleTutorial.Instance != null)
                BattleTutorial.Instance.isNullity = true;
        }

        if (GameManager.Instance.State == GameState.RUNNING)
        {
            Vector3 pos = CardManager.Instance.hackField.transform.position;
            pos += new Vector3(0, 5f, 0);
            waitingCard.transform.DOScale(waitingCard.transform.localScale * 2f, .2f);
            waitingCard.transform.DOMove(pos, .4f);
            waitingCard.transform.DORotate(new Vector3(40, 0, 0), .3f);
            yield return new WaitForSeconds(1f);
            // ¿Ã¸®±â
            ReflectBoxActive(true);
        }
    }

    public void ConfirmReflect()
    {
        Timing.RunCoroutine(ConfirmReflectProcess());
    }
    private IEnumerator<float> ConfirmReflectProcess()
    {
        if (isCardOnHand)
        {

            CardManager.Instance.myCards.Remove(uiCard.linkedCard);
            //a?.GetComponent<Order>().SetOriginOrder(0);

            CardManager.Instance.SetOriginOrder();
            CardManager.Instance.CardAlignment();
            CardManager.Instance.CardDie(uiCard.linkedCard);
            if (WaitingCard != null)
            {
                CardManager.Instance.CardDie(WaitingCard);
            }

            ReflectBoxActive(false);
            yield return Timing.WaitForSeconds(0.7f);
            NewFieldManager.Instance.AvatarMove(NewFieldManager.Instance.enemyCard.curField, () =>
            {
                TurnManager.ChangeTurn();
                if (TutorialManager.Instance != null && TutorialManager.Instance.isTutorial)
                {
                    if (BattleTutorial.Instance != null)
                        BattleTutorial.Instance.isDoneNullity = true;
                }
            });
            isCardOnHand = false;
        }
    }
    public void CancelReflect()
    {
        if (TutorialManager.Instance != null && TutorialManager.Instance.isTutorial)
        {
            return;
        }
        CardManager.Instance.WaitingActionUntilFinishOnReflect?.Invoke();
        ReflectBoxActive(false);
    }
    public void ReflectBoxActive(bool inBool,System.Action act = null)
    {
        canvasGroup.DOFade(inBool ? 1 : 0,.2f);
        uiCard.transform.DOMoveY(inBool ? 540 : -270, 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            isActive = inBool;
            act?.Invoke();
        }
        );
    }
    public void ReflectBoxActive()
    {
        DOTween.Kill(this.gameObject);
        canvasGroup.DOFade(isActive ? 1 : 0,.2f);
        uiCard.gameObject.transform.DOMoveY(isActive ? 540 : -270, 0.2f).SetEase(Ease.OutQuad);
        isActive = !isActive;
    }
}