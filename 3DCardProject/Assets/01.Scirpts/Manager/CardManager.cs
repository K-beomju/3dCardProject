using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManager : Singleton<CardManager>
{
    [SerializeField] public GameObject cardPrefab;
    [SerializeField] public GameObject cardSpawnEffect;

    [SerializeField] public Transform cardSpawnPoint;
    [SerializeField] public Transform cardDeletePoint;
    [SerializeField] public Transform enemy_cardDeletePoint;

    [SerializeField] public List<Card> myCards;
    [SerializeField] protected Transform cardLeft;
    [SerializeField] protected Transform cardRight;

    private DeckManager deckManager;
    public List<Item> itemBuffer { get; set; }
    public Action OnReflect { get; set; }
    public Action WaitingActionUntilFinishOnReflect { get; set; }

    public Card selectCard;
    //public Card movingCard;
    private bool isCardDrag;
    private bool onCardArea;

    public static bool isFullMyCard = false;

    public Ray ray;
    public RaycastHit hitData;
    public Field hitField;

    public BezeirArrows arrowObject;

    private Camera mainCam;

    public Field hackField;

    public Action<Item> OnChangeLastUsedCard { get; set; }

    private Item lastUsedCardItem = new Item();
    public Item LastUsedCardItem
    {
        get
        {
            return lastUsedCardItem;
        }
        set
        {
            lastUsedCardItem = value;
            OnChangeLastUsedCard?.Invoke(lastUsedCardItem);
        }
    }

   
    [SerializeField] private ItemArraySO enemyDataArraySO;

    [field:SerializeField]
    public ItemArraySO defaultDeck { get; set; }

    public Item FindItem(uint uid)
    {
        if (defaultDeck.items.Count < 1) return null;

        foreach (var item in defaultDeck.items)
        {
            if (item.item.uid == uid)
            {
                return item.item.ShallowCopy();
            }
        }
        return null;
    }
    protected override void Awake()
    {
        base.Awake();
        Global.Pool.Clear();
        // ���⿡ �������� �߰�

        Global.Pool.CreatePool<Card>(cardPrefab, transform, 15);
        Global.Pool.CreatePool<Effect_Spawn>(cardSpawnEffect, transform, 15);
    }

    private void Start()
    {
        Debug.Log("AA");
        arrowObject.ActiveArrow(false);
        if (StageManager.Instance.SceneState != SceneState.STAGE)
        {
            mainCam = Camera.main;
            if (StageManager.Instance.SceneState == SceneState.BATTLE)
            {
                if (TutorialManager.Instance.isTutorial)
                {
                    StartCoroutine(TutorialSpawnCardCo(() => { TurnManager.ChangeTurn(TurnType.Player); }));
                }
                else
                    StartCoroutine(StartProcess(() => { TurnManager.ChangeTurn(TurnType.Player); }));

                deckManager = GetComponent<DeckManager>();

            }


        }
    }
    private IEnumerator StartProcess(Action act =null)
    {
        yield return NewFieldManager.Instance.FieldManagerStartCol();
        yield return BattleCameraController.ZoomInEnemy();
        yield return BattleCameraController.PanelIn();
        yield return new WaitForSeconds(5f);
        yield return BattleCameraController.PanelOut();
        yield return BattleCameraController.ZoomOut();
        StartCoroutine(SpawnCardCo(act));
    }
    private IEnumerator SpawnCardCo(Action act = null)
    {
        for (int i = 0; i < 5; i++)
        {
            AddCardForStart();
            yield return new WaitForSeconds(0.2f);
        }
        act?.Invoke();
    }

    private IEnumerator TutorialSpawnCardCo(Action act = null)
    {
        yield return NewFieldManager.Instance.FieldManagerStartCol();
        yield return new WaitForSeconds(10.5f);



        TutorialAddCardForStart(103);
        yield return new WaitForSeconds(0.2f);
        TutorialAddCardForStart(107);
        yield return new WaitForSeconds(0.2f);
        TutorialAddCardForStart(100);
        yield return new WaitForSeconds(0.2f);
        TutorialAddCardForStart(102);
        yield return new WaitForSeconds(0.2f);
        TutorialAddCardForStart(105);


        Item temp = deckManager.itemBuffer[1];
        if(deckManager.itemBuffer[1] != deckManager.FindItem(101))
        {
            deckManager.itemBuffer[1] = deckManager.FindItem(101);
            deckManager.itemBuffer[0] = temp;

        }

        act?.Invoke();
    }

    private IEnumerator DeleteCardCo()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].MoveTransform(new PRS(cardDeletePoint.position, myCards[i].transform.rotation, cardPrefab.transform.localScale), true, 0.3f);
            yield return new WaitForSeconds(0.1f);
        }

        for (int i = myCards.Count - 1; i >= 0; i--)
        {
            ReflectBox.Instance.isCardOnHand = false;
            myCards[i].SetDeleteObject();
            myCards.Remove(myCards[i]);
        }
    }

    public void DeleteMyCard(Card card)
    {
        int temp = myCards.IndexOf(card);
        myCards.RemoveAt(temp);
    }
    public void CardDie(Card card)
    {
        //SelectMovingCardAroundField(false, card);
        Debug.Log("Card Die : " + card.item.itemName);
        if (card.LinkedModel != null)
        {
            // �� ����
            Debug.Log("Model Die : " + card.item.itemName);
            card.LinkedModel.ModelObject.gameObject.SetActive(false);
        }

        PRS prs;
        if (card.isPlayerCard)
        {
            prs = new PRS(CardManager.Instance.cardDeletePoint.position, card.transform.rotation, card.transform.localScale);
        }
        else
        {
            //EnemyFieldManager.Instance.enemyCards.Remove(card);
            prs = new PRS(CardManager.Instance.enemy_cardDeletePoint.position, card.transform.rotation, card.transform.localScale);
        }

        if (card.curField != null)
        {
            if (card.item.IsAvatar)
            {
                card.curField.RemoveAvatarCard();
            }
            else if (card.item.IsUpperCard)
            {
                card.curField.RemoveUpperCard();
            }
            else
            {
                card.curField.RemoveCurCard();
            }

            card.curField = null;
        }

        card.MoveTransform(prs, true, 0.3f);

        //Destroy(card.gameObject, 1);
        card.gameObject.SetActive(false);
    }
    private void RemoveCard(bool killTween = false)
    {
        var a = selectCard;
        myCards.Remove(a);
        if (killTween)
            a.transform.DOKill();
        a?.GetComponent<Order>().SetOriginOrder(0);

        SetOriginOrder();
        CardAlignment();
    }
    public void RandCardDelete()
    {
        StartCoroutine(RandCardDeleteCo());
    }

    public IEnumerator RandCardDeleteCo()
    {

        int rand = UnityEngine.Random.Range(0, 1);
        if (rand == 0)
        {
            int index = UnityEngine.Random.Range(0, myCards.Count);
            Card delCard = myCards[index];
            delCard.canInteract = false;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(delCard.transform.DORotate(new Vector3(delCard.transform.localEulerAngles.x, delCard.transform.localEulerAngles.y, 0), 0.2f));
            sequence.Append(delCard.transform.DOMove(hackField.transform.position + new Vector3(0, 4f, 0), .15f).OnComplete(() => delCard.DeleteTrapCard())).AppendInterval(.3f);
            sequence.Append(delCard.transform.DOScale(delCard.transform.localScale * 2f, .3f)).AppendInterval(.3f);
            yield return new WaitForSeconds(2f);
            myCards.Remove(delCard);

            SetOriginOrder();
            CardAlignment();

            CardDie(delCard);

        }
        else
            print("�� �ߵ� ����");

    }





    private void Update()
    {
        if (StageManager.Instance.SceneState == SceneState.STAGE)
        {
            return;
        }
        if (GameManager.Instance?.State == GameState.END)
        {
            return;
        }

        if (isCardDrag)
        {
            CardDrag();
        }

        DetectCardArea();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddCard();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(DeleteCardCo());
        }

        ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitData, Mathf.Infinity))
        {
            Field field = hitData.transform.GetComponent<Field>();
            Card card = hitData.transform.GetComponent<Card>();
            PurchasePlank pp = hitData.transform.GetComponent<PurchasePlank>();
            if (InputManager.Instance.MouseUp)
            {

                if (selectCard != null)
                {
                    Debug.Log("AAAAAAAA");

                    if (field != null && ((field.curCard == null && !selectCard.item.IsUpperCard) || (field.upperCard == null && selectCard.item.IsUpperCard)) && field.isEnterRange)
                    {
                        if (selectCard.item.IsStructCard)
                        {
                            if (!field.isCommon)
                            {
                                NewFieldManager.Instance.Spawn(field, selectCard);
                                NewFieldManager.Instance.DisableSpawnRange();
                                //PlayerManager.Instance.playerCards.Add(selectCard);
                                RemoveCard(false);
                            }
                        }
                        else
                        {
                            if (field.isCommon)
                            {
                                NewFieldManager.Instance.Spawn(field, selectCard);
                                NewFieldManager.Instance.DisableSpawnRange();
                                //PlayerManager.Instance.playerCards.Add(selectCard);
                                RemoveCard(false);
                            }
                        }
                    }
                    else if (card != null && card.curField != null && ((card.curField.curCard == null && !selectCard.item.IsUpperCard) || (card.curField.upperCard == null && selectCard.item.IsUpperCard)) && card.curField.isEnterRange)
                    {
                        if (selectCard.item.IsStructCard)
                        {
                            if (!card.curField.isCommon)
                            {
                                NewFieldManager.Instance.Spawn(card.curField, selectCard);
                                NewFieldManager.Instance.DisableSpawnRange();
                                //PlayerManager.Instance.playerCards.Add(selectCard);
                                RemoveCard(false);
                            }
                        }
                        else
                        {
                            if (card.curField.isCommon)
                            {
                                NewFieldManager.Instance.Spawn(card.curField, selectCard);
                                NewFieldManager.Instance.DisableSpawnRange();
                                //PlayerManager.Instance.playerCards.Add(selectCard);
                                RemoveCard(false);
                            }
                        }
                    }
                    else if (pp != null)
                    {
                        Debug.Log($"���� �õ� : {selectCard.item.itemName} ");
                        if (ShopManager.Instance.Purchase(selectCard.item))
                        {
                            SaveManager.Instance.gameData.DisposableItem = selectCard.item.ShallowCopy();
                            selectCard.SetDeleteObject();
                        }
                    }
                    else
                    {
                        EnlargeCard(false, selectCard);
                    }

                    selectCard = null;
                    arrowObject.ActiveArrow(false);
                    isCardDrag = false;
                }
                hitField = null;
            }
            else if (InputManager.Instance.MouseBtn && selectCard != null && selectCard.curField == null)
            {
                if (card != null)
                {
                    if (card.curField != null)
                    {
                        hitField = card.curField;
                        card.curField.HitColor(true, true);
                        card.curField.HitColor(true, selectCard.item.IsUpperCard && !card.item.IsUpperCard);
                    }
                    else
                    {
                        hitField = null;
                    CardInfoUI.Instance.ActiveUI(false);
                    }

                }
                else if (field != null)
                {
                    hitField = field;
                    field.HitColor(true, field.isEnterRange && ((selectCard.item.IsStructCard && !field.isCommon) || (!selectCard.item.IsStructCard && field.isCommon)));
                }
                else
                {
                    hitField = null;
                    CardInfoUI.Instance.ActiveUI(false);
                }
                Debug.DrawRay(ray.origin, ray.direction * 30, Color.yellow);
            }

        }
        else
        {
            hitField = null;
            CardInfoUI.Instance.ActiveUI(false);

        }

        if (InputManager.Instance.MouseUp)
        {
            if (selectCard != null)
            {
                arrowObject.ActiveArrow(false);
                isCardDrag = false;
                EnlargeCard(false, selectCard);
                selectCard = null;
            }
            hitField = null;
        }

     
    }


    #region CardSystem

    public void AddCardForStart()
    {
        Item popItem = deckManager.PopItem();
        if (popItem != null)
        {
            Card card = CreateCard(popItem, true);
            myCards.Add(card);
            card.OnCreateCard();

            if (popItem.IsReflectCard)
                ReflectBox.Instance.SetUpCard(card);
            SetOriginOrder();

            CardAlignment();
        }
    }

    public void TutorialAddCard()
    {

        Item popItem = deckManager.PopItem();
        if (popItem != null)
        {
            Card card = CreateCard(popItem, true);
            myCards.Add(card);
            card.OnCreateCard();
            card.isPlayerCard = false;
            if (popItem.IsReflectCard)
                ReflectBox.Instance.SetUpCard(card);
            SetOriginOrder();

            CardAlignment();
        }
    }
    public void TutorialAddCardForStart(uint uid)
    {
        Item popItem = deckManager.PopItem(uid);
        if (popItem != null)
        {
            Card card = CreateCard(popItem, true);
            myCards.Add(card);
            card.OnCreateCard();
            card.isPlayerCard = false;
            if (popItem.IsReflectCard)
                ReflectBox.Instance.SetUpCard(card);
            SetOriginOrder();

            CardAlignment();
        }
    }

    public void AddCard()
    {
        Debug.Log("ASdASD");
        SoundManager.Instance.PlayFXSound("DrawCard", .1f);
        Item popItem = deckManager.PopItem();
        if (popItem != null)
        {
            Card card = CreateCard(popItem, true);
            myCards.Add(card);
            card.OnCreateCard();

            if (popItem.IsReflectCard)
                ReflectBox.Instance.SetUpCard(card);
            SetOriginOrder();

            CardAlignment();
        }

        if (myCards.Count < 1)
        {
            StartCoroutine(JudgeWinner());
            //PlayerDeckManager pdm = deckManager as PlayerDeckManager;
            //pdm.SetUpPlayerDeckManager();

            //StartCoroutine(SpawnCardCo());
        }
    }

    public Card CreateCard(Item item, bool isPlayerCard)
    {
        SoundManager.Instance.PlayFXSound("DrawCard", .1f);

        var card = Global.Pool.GetItem<Card>();
        card.transform.position = cardSpawnPoint.position;

        card.Setup(item, true, isPlayerCard);               
        card.GetComponent<Order>().SetOriginOrder(1);
        return card;
    }

    public void SetOriginOrder()
    {
        int count = myCards.Count;

        for (int i = 0; i < count; i++)
        {
            var targetCard = myCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    public void CardAlignment()
    {
        List<PRS> originCardPRss = new List<PRS>();
        originCardPRss = RoundAlignment(cardRight, cardLeft, myCards.Count, 0.5f, cardPrefab.transform.localScale * 0.8f);

        var targetCards = myCards;
        for (int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];

            targetCard.originPRS = originCardPRss[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.3f);
        }
    }
    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>();

        switch (objCount)
        {

            case 0: objLerps = new float[] { 0.5f }; break;
            case 1:
                objLerps = new float[] { 0.5f };
                {
                    cardLeft.transform.position = new Vector3(-.5f, cardLeft.position.y);
                    cardRight.transform.position = new Vector3(.5f, cardRight.position.y);
                }
                break;
            case 2:
                cardLeft.transform.position = new Vector3(-1.5f, cardLeft.position.y);
                cardRight.transform.position = new Vector3(1.5f, cardRight.position.y);
                objLerps = new float[] { 0.1f, 0.9f };
                break;
            case 3:
                cardLeft.transform.position = new Vector3(-2.5f, cardLeft.position.y);
                cardRight.transform.position = new Vector3(2.5f, cardRight.position.y);
                objLerps = new float[] { 0.1f, 0.5f, 0.9f };
                break;
            case 4:
                cardLeft.transform.position = new Vector3(-3.5f, cardLeft.position.y);
                cardRight.transform.position = new Vector3(3.5f, cardRight.position.y);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = 1f / (objCount - 1) * i;
                break;
            case 5:
                cardLeft.transform.position = new Vector3(-4.5f, cardLeft.position.y);
                cardRight.transform.position = new Vector3(4.5f, cardRight.position.y);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = 1f / (objCount - 1) * i;
                break;

            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Quaternion.Euler(45, 0, 0);

            float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
            targetPos.y += curve;
            targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);

            results.Add(new PRS(targetPos + new Vector3(0, 0, -9f), targetRot, scale));

        }
        return results;
    }


    #endregion

    #region MyCard

    public virtual void CardMouseOver(Card card)
    {
        if (!card.isPlayerCard) return;
        if (isCardDrag || card.isOnField) return;

        // if (!isCardDrag)
        EnlargeCard(true, card);

    }

    public virtual void CardMouseExit(Card card)
    {
        if (!card.isPlayerCard) return;
        if (card.isOnField || isCardDrag) return;

        EnlargeCard(false, card);
    }

    public virtual void CardMouseDown(Card card)
    {
        if (!card.isPlayerCard) return;

        if (card.curField == null)
        {
            ArrowMove(card);
            selectCard.transform.DOScale(new Vector3(0.3f, 0.3f, 0.5f), 0.05f);

            Vector3 enlarPos = new Vector3(selectCard.transform.position.x, selectCard.transform.position.y, selectCard.transform.position.z - 1);
            selectCard.MoveTransform(new PRS(enlarPos, Quaternion.Euler(75, 0, 0), cardPrefab.transform.localScale), false);
        }

    }
    public virtual void ArrowMove(Card card, bool isYfixed = true)
    {
        isCardDrag = true;
        selectCard = card;

        arrowObject.ActiveArrow(true);

        float x = mainCam.WorldToScreenPoint(selectCard.transform.position).x;
        float y = mainCam.WorldToScreenPoint(selectCard.transform.position).y;
        arrowObject.transform.position = new Vector3(x, isYfixed ? 470 : y + 300, 0);
    }

    public virtual void CardMouseUp()
    {
        isCardDrag = false;
    }

    void CardDrag()
    {
        if (!onCardArea)
        {
            //selectCard.MoveTransform(new PRS(Utils.MousePos - new Vector3(0, 10, -2), Quaternion.Euler(45, 0, 0), selectCard.originPRS.scale), false);
        }
    }

    protected virtual void DetectCardArea()
    {
        /*   RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
           int cardArealayer = LayerMask.NameToLayer("CardArea");
           int monsterFieldLayer = LayerMask.NameToLayer("MonsterField");
           int magicFieldLayer = LayerMask.NameToLayer("MagicField");
           onCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == cardArealayer);
           onMonsterField = Array.Exists(hits, x => x.collider.gameObject.layer == monsterFieldLayer);
           if (onMonsterField)
               curMonsterField = Array.Find(hits, x => x.collider.gameObject.layer == monsterFieldLayer).transform.GetComponent<MonsterField>();
           onMagicField = Array.Exists(hits, x => x.collider.gameObject.layer == magicFieldLayer);
           if (onMagicField)
               curMagicField = Array.Find(hits, x => x.collider.gameObject.layer == magicFieldLayer).transform.GetComponent<MagicField>();
   */
    }
    private void EnlargeCard(bool isEnlarge, Card card)
    {
        switch (StageManager.Instance.SceneState)
        {
            case SceneState.Title:
                if (isEnlarge)
                {
                    Vector3 enlarPos = new Vector3(card.originPRS.pos.x, card.originPRS.pos.y + .5f, card.originPRS.pos.z);
                    card.MoveTransform(new PRS(enlarPos, card.originPRS.rot, card.originPRS.scale), false);
                }
                else
                {
                    card.MoveTransform(card.originPRS, false);
                }
                break;
            case SceneState.STAGE:
                break;
            case SceneState.BATTLE:
                if (isEnlarge)
                {
                    Vector3 enlarPos = new Vector3(card.originPRS.pos.x, card.originPRS.pos.y + 1.5f, card.originPRS.pos.z + 1.5f);
                    card.MoveTransform(new PRS(enlarPos, Quaternion.Euler(75, 0, 0), cardPrefab.transform.localScale), false);
                }
                else
                {
                    card.MoveTransform(card.originPRS, false);
                }

                card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
                break;
            case SceneState.SHOP:
                break;
            default:
                break;
        }

    }

    #endregion

    public void MountCardSupport(uint num, MountState state = MountState.NULL, bool canCatch = false)
    {
        Item cardItem = null;
        if (num == 0)
        {
            cardItem = EnemyManager.Instance.GetRandItem(canCatch);
        }
        else
        {
            cardItem = EnemyManager.Instance.PopItem(num);
        }

        if (cardItem == null)
        {
            EnemyDeckManager edm = EnemyManager.Instance.dm as EnemyDeckManager;
            edm.SetUpEnemyDeckManager();
            cardItem = edm.PopItem();
        }
        Card card = CreateCard(cardItem, false);
        if (state == MountState.NULL)
        {
            switch (num)
            {
                case 100:// ����
                case 101:// ��ȿ
                case 102:// ü����
                case 104:// �ſ�
                case 105:// ��ž
                    state = MountState.Hack;
                    break;
                case 103:// ��Ʋ
                case 106:// ��
                case 107:// ��
                    break;
                default:
                    break;

            }
        }

        EnemyAI.Instance.MountingCard(card, state);
    }

    public void TutorialCardOutLine(uint uid)
    {
        int index = myCards.FindIndex(x => x.item.uid == uid);
        myCards[index].SelectOutlineCard();
        foreach (var item in myCards)
        {
            if (item.item.uid == uid)
            {
                item.isPlayerCard = true;
                continue;
            }

            item.isPlayerCard = false;
        }
    }

    public Item FindEnemyItem(uint uid)
    {
        if (enemyDataArraySO.items.Count < 1) return null;

        foreach (var itemSO in enemyDataArraySO.items)
        {
            if (itemSO.item.uid == uid)
            {
                return itemSO.item.ShallowCopy();
            }
        }
        return null;
    }

    public IEnumerator JudgeWinner()
    {
        Hack hack = hackField.GetComponent<Hack>();
        yield return BattleCameraController.ZoomIn(hack.HackColorObject.transform);
        yield return new WaitForSeconds(3f);
        if (hack.IsPlayerState())
        {
            //Win
            GameManager.Instance.OnWinGame?.Invoke();
        }
        else if(hack.IsEnemyState())
        {
            // Lose
            GameManager.Instance.OnLoseGame?.Invoke();
        }
        else
        {
            // tie
            GameManager.Instance.OnTieGame?.Invoke();
        }
    }
}
