using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardManager : Singleton<CardManager>
{
    [SerializeField] public GameObject cardPrefab;

    [SerializeField] public Transform cardSpawnPoint;
    [SerializeField] public Transform cardDeletePoint;
    [SerializeField] public Transform enemy_cardDeletePoint;

    [SerializeField] public List<Card> myCards;
    [SerializeField] protected Transform cardLeft;
    [SerializeField] protected Transform cardRight;

    private DeckManager deckManager;
    public List<Item> itemBuffer { get; set; }
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

    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        StartCoroutine(SpawnCardCo());
        arrowObject.ActiveArrow(false);
        deckManager = GetComponent<DeckManager>();
        mainCam = Camera.main;
    }


    private IEnumerator SpawnCardCo()
    {
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 5; i++)
        {
            AddCard();
            yield return new WaitForSeconds(0.2f);
        }
        TurnManager.ChangeTurn(TurnType.Player);

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
            myCards[i].SetDeleteObject();
            myCards.Remove(myCards[i]);
        }
    }

    public void DeleteMyCard(Card card)
    {
        int temp = myCards.IndexOf(card);
        myCards.RemoveAt(temp);
    }

    public void RandCardDelete()
    {
        int rand = UnityEngine.Random.Range(0, 2);
        if (rand == 0)
        {
            int index = UnityEngine.Random.Range(0, myCards.Count);
            myCards[index].SetDeleteObject();
            myCards.Remove(myCards[index]);

            SetOriginOrder();
            CardAlignment();

            print("�� �ߵ�");

        }
        else
            print("�� �ߵ� ����");


    }



    private void Update()
    {
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

        if (InputManager.Instance.MouseUp)
        {
            if (Physics.Raycast(ray, out hitData, Mathf.Infinity))
            {
                Field field = hitData.transform.GetComponent<Field>();
                Card card = hitData.transform.GetComponent<Card>();

                if (selectCard != null)
                {
                    /*if (card != null && card.item != PlayerManager.Instance.playerItem && card.isPlayerCard && card.curField != null )
                    {
                        field = card.curField;

                        CardDie(card);

                        FieldManager.Instance.CheckingSpawn(field, selectCard);
                        PlayerManager.Instance.playerCards.Add(selectCard);
                        RemoveCard(false);
                        TurnManager.PlayerCardMove();
                        //MyCardMove(true);

                    }
                    else */
                    if (field != null && field.curCard == null)
                    {
                        NewFieldManager.Instance.Spawn(field, selectCard);
                        //FieldManager.Instance.CheckingSpawn(field, selectCard);
                        PlayerManager.Instance.playerCards.Add(selectCard);
                        RemoveCard(false);
                        //TurnManager.PlayerCardMove();
                        //MyCardMove(true);
                    }
                    else
                    {
                        EnlargeCard(false, selectCard);
                    }

                    selectCard = null;
                    arrowObject.ActiveArrow(false);
                    isCardDrag = false;
                }
                /*else if (movingCard != null)
                {
                    if (field != null && field.curCard == null && field.isSelected)
                    {
                        SelectMovingCardAroundField(false);
                        FieldManager.Instance.MoveToField(field, movingCard);
                    }
                    if (card != null && card.curField != null && card.curField.isSelected && !card.isPlayerCard)
                    {
                        SelectMovingCardAroundField(false);
                        FieldManager.Instance.MoveToField(card.curField, movingCard);
                    }

                }*/
            }

        }

        if (InputManager.Instance.MouseBtn && selectCard != null && selectCard.curField == null)
        {
            if (Physics.Raycast(ray, out hitData, Mathf.Infinity))
            {
                Field field = hitData.transform.GetComponent<Field>();
                if (field != null && field.isEnterRange)
                {
                    hitField = field;
                    field.HitColor(true);
                }
                Debug.DrawRay(ray.origin, ray.direction * 30, Color.yellow);
            }
        }

    }


    #region CardSystem

    public void AddCard()
    {
        Item popItem = deckManager.PopItem();
        if (popItem != null)
        {
            var cardObj = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
            var card = cardObj.GetComponent<Card>();
            card.Setup(popItem, true, true);
            myCards.Add(card);

            SetOriginOrder();

            CardAlignment();
        }


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

    public void CardDie(Card card)
    {
        //SelectMovingCardAroundField(false, card);
        PRS prs;
        if (card.isPlayerCard)
        {
            prs = new PRS(CardManager.Instance.cardDeletePoint.position, card.transform.rotation, card.transform.localScale);
        }
        else
        {
            EnemyFieldManager.Instance.enemyCards.Remove(card);
            prs = new PRS(CardManager.Instance.enemy_cardDeletePoint.position, card.transform.rotation, card.transform.localScale);
        }
        if(card.item.isAvatar)
        {
            card.curField.RemoveAvatarCard();
        }
        else if (card.item.isUpperCard)
        {
            card.curField.RemoveUpperCard();
        }
        else
        {
            card.curField.RemoveCurCard();
        }
        card.curField = null;
        card.MoveTransform(prs, true, 0.3f);

        Destroy(card.gameObject, 1);
    }
    #endregion

    #region MyCard

    public virtual void CardMouseOver(Card card)
    {
        if (isCardDrag || card.isOnField) return;

        // if (!isCardDrag)

        EnlargeCard(true, card);
    }

    public virtual void CardMouseExit(Card card)
    {
        if (card.isOnField || isCardDrag) return;

        EnlargeCard(false, card);
    }

    public virtual void CardMouseDown(Card card)
    {
        if (!card.isPlayerCard) return;

        if (card.curField == null)
        {
            //NewFieldManager.Instance.CheckCardDragSpawnRange();
            isCardDrag = true;
            selectCard = card;
            arrowObject.ActiveArrow(true);
            float x = mainCam.WorldToScreenPoint(selectCard.transform.position).x;
            arrowObject.transform.position = new Vector3(x, 540, 0);
        }
        /*else
        {
            if (movingCard != null)
                SelectMovingCardAroundField(false);

            movingCard = card;

            SelectMovingCardAroundField(true);
        }*/
    }
    public void SelectMovingCardAroundField(bool Inbool, Card card = null)
    {
        /*if (card == null)
            card = movingCard;*/

        Vector2Int gridPos = FieldManager.Instance.GetGridPos(card.curField);

        Vector2Int downLeftPos = gridPos - new Vector2Int(1, 1);
        Vector2Int upRightPos = gridPos + new Vector2Int(1, 1);

        for (int x = downLeftPos.x; x < upRightPos.x + 1; x++)
        {
            for (int y = downLeftPos.y; y < upRightPos.y + 1; y++)
            {
                Vector2Int getPos = new Vector2Int(x, y);
                if (getPos != gridPos)
                {
                    Field field = FieldManager.Instance.GetField(getPos);
                    if (field != null)
                    {
                        field.FieldSelect(Inbool);
                    }
                }
            }
        }
    }

    public virtual void CardMouseUp()
    {
        isCardDrag = false;

        UseCard();

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
        if (isEnlarge)
        {
            Vector3 enlarPos = new Vector3(card.originPRS.pos.x, card.originPRS.pos.y, card.originPRS.pos.z + 1.5f);
            card.MoveTransform(new PRS(enlarPos, Quaternion.Euler(75, 0, 0), cardPrefab.transform.localScale), false);
        }
        else
            card.MoveTransform(card.originPRS, false);

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    #endregion


    public virtual void UseCard()
    {

        /*
                if (selectCard != null)
                {
                    if (onMonsterField && !selectCard.item.isMagic && curMonsterField.curCard == null && curMonsterField.isPlayerField)
                    {
                        RemoveCard();
                        curMonsterField.SetUp(selectCard);
                    }
                    else if (onMagicField && selectCard.item.isMagic && curMagicField.curCard == null && curMagicField.isPlayerField)
                    {
                        RemoveCard();
                        curMagicField.SetUp(selectCard);
                    }
                    else if (!onCardArea)
                    {

                        if (!myCards.Contains(selectCard))
                        {
                            myCards.Add(selectCard);
                        }
                        SetOriginOrder();
                        CardAlignment();
                    }
                    else if (onCardArea)
                    {
                        if (!myCards.Contains(selectCard))
                        {
                            myCards.Add(selectCard);
                            SetOriginOrder();
                            CardAlignment();
                        }

                    }

                    selectCard = null;
                }
        */
    }

    private void RemoveCard(bool killTween)
    {
        var a = selectCard;
        myCards.Remove(a);
        if (killTween)
            a.transform.DOKill();
        a?.GetComponent<Order>().SetOriginOrder(0);

        SetOriginOrder();
        CardAlignment();
    }

    //public bool MyCardIsFull()
    //{
    //    if (myCards.Count >= 8)
    //    {
    //        return isFullMyCard;
    //    }
    //    return !isFullMyCard;
    //}

    //public void MyCardMove(bool isSpawn)
    //{
    //    if (isSpawn)
    //    {
    //        for (int i = 0; i < myCards.Count; i++)
    //        {
    //            myCards[i].transform.DOMoveZ(-7.5f, 0.3f);
    //        }

    //    }
    //    else
    //    {
    //        for (int i = 0; i < myCards.Count; i++)
    //        {
    //            myCards[i].transform.DOMoveZ(-7.5f, 0.3f);
    //        }
    //    }
    //}
}
