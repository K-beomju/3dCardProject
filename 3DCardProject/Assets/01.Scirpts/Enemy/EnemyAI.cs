using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public enum MountState
{
    NULL,
    Prev,
    Next,
    Hack
}

public enum EnemyType
{
    TUTORIAL,
    EASY,
    MEDIUM,
    BOSS
}

public class EnemyAI : Singleton<EnemyAI>
{
    public EnemyType enemyType;
    private MountState mountState;
    private Action action;
    public bool isPhaseOne = true;

    private bool checkOnce = false;
    [SerializeField]
    private bool isReflectOnHand;
    public bool IsReflectOnHand
    {
        get
        {
            bool ishaveItem = EnemyManager.Instance.IsHaveItem(101);
            if(!checkOnce)
            {
                isReflectOnHand = ishaveItem;
                checkOnce = true;
            }
            else
            {
                if(!isReflectOnHand && !ishaveItem)
                {
                    isReflectOnHand = true;
                }
            }
            return isReflectOnHand;
        }
    }
    public Card WaitingCard;
    private Dictionary<long, Action> mediumActions = new Dictionary<long, Action>() // // �ƹ�Ÿ ���� �븻 �÷��̾� ����
    {
        {
            0b100000100000001001000011111111, () => {
                CardManager.Instance.MountCardSupport(107 , MountState.Prev);

        } },

        {
            0b101000000000101001000011111111, () => {
                CardManager.Instance.MountCardSupport(100);

        } },
            {
            0b100010010000000011111111, () => {
                CardManager.Instance.MountCardSupport(102);

        } },
        {
            0b1000000100000001001000011111111, () => {
                CardManager.Instance.MountCardSupport(0,MountState.Prev);
        } },

            {
            0b10100000000000000000100101110010, () => {
                CardManager.Instance.MountCardSupport(102);
        } },


             {
            0b101001101000100000000011101000, () => {
                CardManager.Instance.MountCardSupport(102);
        } },

              {
            0b100000000000100111111111, () => {
                CardManager.Instance.MountCardSupport(103, MountState.Prev);
        } },

                 {
            0b1000100100000000000011101111, () => {
                CardManager.Instance.MountCardSupport(102);
        } },

                        {
            0b10001001000000000000000001101111, () => {
                CardManager.Instance.MountCardSupport(102);
        } },

                                     {
            0b100000100000001001000011110111, () => {
                CardManager.Instance.MountCardSupport(107, MountState.Prev);
        } },

                                                              {
            0b101000000000001001000011110110, () => {
                CardManager.Instance.MountCardSupport(106, MountState.Prev);
        } },
                                                     {
            0b100000001001000011110111, () => {
                CardManager.Instance.MountCardSupport(100);
        } },

                                                       {
            0b1000000010010000001001110111, () => {
                CardManager.Instance.MountCardSupport(107, MountState.Prev);
        } },


                                           {
            0b101000000000101001000011110110, () => {
                CardManager.Instance.MountCardSupport(105);
        } },

                                                            {
            0b10010000100000000000000011110111, () => {
                CardManager.Instance.MountCardSupport(107, MountState.Next);
        } },

                                                            {
            0b10001000001001100001110000, () => {
                CardManager.Instance.MountCardSupport(102);
        } },
                                                              {
            0b101000000000100000100111110110, () => {
                CardManager.Instance.MountCardSupport(103, MountState.Next);
        } },

                                                              {
            0b10010000000010000000000011110110, () => {
                CardManager.Instance.MountCardSupport(105);
        } },

                                                     {
            0b10000000001000001001001001110110, () => {
                CardManager.Instance.MountCardSupport(103, MountState.Prev);
        } },

                                                                                              {
            0b100010010000000011110111, () => {
                CardManager.Instance.MountCardSupport(102);
        } },



                                                         {
            0b101000100100000000000111001000 , () => {
                CardManager.Instance.MountCardSupport(102);
        } },

                                                                                                         {
            0b10000000000000000000100100110111 , () => {
                CardManager.Instance.MountCardSupport(102);
        } },



                                                                                                         {
            0b101000000000001001000111011010 , () => {
                CardManager.Instance.MountCardSupport(106 , MountState.Prev);
        } },




    };

    private Dictionary<long, Action> tutorialActions = new Dictionary<long, Action>()
    {
                  {
            0b100000001001000011110111, () => {
                CardManager.Instance.MountCardSupport(107, MountState.Prev);
        } },
                  {
            0b10011000000000100010000011110010, () => {
                CardManager.Instance.MountCardSupport(100);
        } },

                   {
            0b1000000000100000100111110110, () => {
                CardManager.Instance.MountCardSupport(105);
        } },

                  

    };

    private Dictionary<long, Action> bossPhaseTwoAction = new Dictionary<long, Action>()
    {
                  {
            0b100000001001000011110111, () => {
                 if (!Instance.isRedFloor)
                    {
                        Instance.StartCoroutine( Instance.BossActionRedFloor());
                    }
                    else
                    {
                        Instance.isRedFloor = false;
                    }
        } },
                  {
            0b10011000000000100010000011110010, () => {
                Instance.StartCoroutine( Instance.BossActionMountBomb(UnityEngine.Random.Range(0,6)));
        } },


    };

    private bool isRedFloor = false;
    private bool isTurn2Phase2 = false;
    private bool isMountBomb = false;
    private Action OnBombCount;

    private void Start()
    {
        enemyType = EnemyManager.Instance.curEnemyType;
    }

    private long currentState = 0b0000_0000_0000_0000_0000_0000_0000_0000;

    [ContextMenu("InitState")]
    public void InitState()
    {

        currentState = 0b0000_0000_0000_0000_0000_0000_0000_0000;

        for (int i = 0; i < NewFieldManager.Instance.fieldList.Count; i++)
        {
            var field = NewFieldManager.Instance.fieldList[i];

            if (field.avatarCard != null)
            {
                if (field.avatarCard.isPlayerCard)
                {
                    currentState += 0b0000_0000_0000_0000_0000_0001_0000_0000;
                }
                currentState += 0b0000_0000_0000_0000_0000_1000_0000_0000;
            }
            if (field.upperCard != null)
                currentState += 0b0000_0000_0000_0000_0000_0100_0000_0000;
            if (field.curCard != null)
                currentState += 0b0000_0000_0000_0000_0000_0010_0000_0000;

            if (i != NewFieldManager.Instance.fieldList.Count - 1)
                currentState <<= 4;
        }


        DeckManager dm = EnemyManager.Instance.dm;

        for (int i = 0; i < dm.itemBuffer.Count; i++)
        {
            switch (dm.itemBuffer[i].uid)
            {
                case 100:
                    currentState += 0b0000_0000_0000_0000_0000_0000_1000_0000;
                    break;
                case 101:
                    currentState += 0b0000_0000_0000_0000_0000_0000_0100_0000;
                    break;
                case 102:
                    currentState += 0b0000_0000_0000_0000_0000_0000_0010_0000;
                    break;
                case 103:
                    currentState += 0b0000_0000_0000_0000_0000_0000_0001_0000;
                    break;
                case 104:
                    currentState += 0b0000_0000_0000_0000_0000_0000_0000_1000;
                    break;
                case 105:
                    currentState += 0b0000_0000_0000_0000_0000_0000_0000_0100;
                    break;
                case 106:
                    currentState += 0b0000_0000_0000_0000_0000_0000_0000_0010;
                    break;
                case 107:
                    currentState += 0b0000_0000_0000_0000_0000_0000_0000_0001;
                    break;
            }
        }
        for (int i = 0; i < EnemyManager.Instance.enemyItems.Count; i++)
        {
            switch (EnemyManager.Instance.enemyItems[i].uid)
            {
                case 100:
                    currentState += 0b0000_0000_0000_0000_0000_0000_1000_0000;
                    break;
                case 101:
                    currentState += 0b0000_0000_0000_0000_0000_0000_0100_0000;
                    break;
                case 102:
                    currentState += 0b0000_0000_0000_0000_0000_0000_0010_0000;
                    break;
                case 103:
                    currentState += 0b0000_0000_0000_0000_0000_0000_0001_0000;
                    break;
                case 104:
                    currentState += 0b0000_0000_0000_0000_0000_0000_0000_1000;
                    break;
                case 105:
                    currentState += 0b0000_0000_0000_0000_0000_0000_0000_0100;
                    break;
                case 106:
                    currentState += 0b0000_0000_0000_0000_0000_0000_0000_0010;
                    break;
                case 107:
                    currentState += 0b0000_0000_0000_0000_0000_0000_0000_0001;
                    break;
            }
        }
        print(Convert.ToString(currentState, 2));
    }

    [ContextMenu("JudgementCard")]
    public void JudgementCard()
    {
        InitState();
        switch (enemyType)
        {
            case EnemyType.TUTORIAL:
                DoAction(tutorialActions);
                break;
            case EnemyType.EASY:
                EnemyManager.Instance.EnemyAction();
                break;
            case EnemyType.MEDIUM:
                DoAction(mediumActions);
                break;
            case EnemyType.BOSS:
                Debug.Log(isPhaseOne);
                if (isPhaseOne)
                {
                    DoAction(mediumActions);
                }
                else
                {
                    if (isTurn2Phase2)
                        return;
                    if(isMountBomb)
                    {
                        OnBombCount?.Invoke();
                    }
                    else
                    {
                        DoAction(bossPhaseTwoAction);
                    }
                }
                break;
            default:
                break;
        }
    }

    public IEnumerator ActiveOutline(int idx , bool isActive,Color32 color,float term)
    {
        yield return new WaitForSeconds(term);
        NewFieldManager.Instance.fieldList[idx].outline.enabled = isActive;
        NewFieldManager.Instance.fieldList[idx].outline.OutlineColor = color;
    }
    public IEnumerator ActiveOutline(bool isOdd,bool isActive,Color32 color,float term)
    {
        for (int i = 0; i < NewFieldManager.Instance.fieldList.Count; i++)
        {
            if (isOdd ? i % 2 != 0 : i % 2 == 0)
                continue;
            yield return ActiveOutline(i,isActive,color,term);
        }
        yield break;
    }
    //다음턴에 특정 위치에 있으면 HP--;
    public IEnumerator BossActionRedFloor()
    {
        bool isOdd = UnityEngine.Random.Range(0, 1) == 0;
        yield return ActiveOutline(isOdd,true,Utils.EnemyColor,.6f);
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < 6; i++)
        {
            yield return ActiveOutline(isOdd, false, Utils.EnemyColor, 0f);
            yield return new WaitForSeconds(.2f);
            yield return ActiveOutline(isOdd, true, Utils.EnemyColor, 0f);
            yield return new WaitForSeconds(.2f);
        }
        yield return ActiveOutline(isOdd,false,Utils.EnemyColor,0f);

        TurnManager.Instance.OnTurnChange2Enemy += () => {
            Debug.Log("Bombb1");
            StartCoroutine(BossActionRedFloorAttack(isOdd));
        };
        isRedFloor = true;
        TurnManager.ChangeTurn();
    }
    public IEnumerator BossActionRedFloorAttack(bool isOdd)
    {
        for (int i = 0; i < NewFieldManager.Instance.fieldList.Count; i++)
        {
            if (isOdd ? i % 2 != 0 : i % 2 == 0)
                continue;

            NewFieldManager.Instance.fieldList[i].outline.enabled = true;
            NewFieldManager.Instance.fieldList[i].outline.OutlineColor = Utils.EnemyColor;
        }

        yield return new WaitForSeconds(.5f);
        Item item = CardManager.Instance.FindEnemyItem(1100);
        if (item == null)
        {
            Debug.LogError("잘못된 아이템 UID");
            yield break;
        }
        for (int i = 0; i < NewFieldManager.Instance.fieldList.Count; i++)
        {
            if (isOdd ? i % 2 != 0 : i % 2 == 0)
                continue;
            Debug.Log("Bombb2");
            Field field = NewFieldManager.Instance.fieldList[i];
            Card card = Global.Pool.GetItem<Card>();
            card.Setup(item, true, false);
            card.OnSpawn();
            card.transform.position = field.transform.position + new Vector3(0, 15, 0);
            card.transform.DOMove(field.transform.position + new Vector3(0, 1, 0),.6f).OnComplete(()=> {

                Global.Pool.GetItem<Effect_Spawn>().transform.position = field.transform.position + new Vector3(0, 1, 0);
                CardManager.Instance.CardDie(card);
            });

            Card aCard = field.avatarCard;
            if (aCard != null)
            {
                if (aCard?.item.uid == NewFieldManager.Instance.playerCard.item.uid)
                {
                    // 체력 감소
                    Debug.Log("Bombb3");
                    SaveManager.Instance.gameData.Hp--;
                }
            }
        }
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < NewFieldManager.Instance.fieldList.Count; i++)
        {
            if (isOdd ? i % 2 != 0 : i % 2 == 0)
                continue;

            NewFieldManager.Instance.fieldList[i].outline.enabled = false;
        }
        yield return new WaitForSeconds(.5f);

        TurnManager.Instance.OnTurnChange2Enemy = null;
        TurnManager.ChangeTurn();
    }

    public IEnumerator BossActionMountBomb(int idx)
    {
        yield return new WaitForSeconds(1f);
        Card card = MountCard(idx,1100);
        if (card == null) yield break;
        card.gameObject.AddComponent<BombCard>();
        BombCard bCard = card.GetComponent<BombCard>();
        OnBombCount = bCard.Counting;
        bCard.OnUnderZero = ()=> { StartCoroutine( BossActionBomb(card)); };
        int fieldCount = NewFieldManager.Instance.fieldList.Count;
        for (int j = 0; j < 6; j++)
        {
            for (int i = 0; i < fieldCount; i++)
            {
                yield return ActiveOutline(i, j%2 != 0, Utils.EnemyColor, 0);
            }
            yield return new WaitForSeconds(.2f);
        }
        TurnManager.ChangeTurn();
    }
    public IEnumerator BossActionBomb(Card inBombCard)
    {
      
        int fieldCount = NewFieldManager.Instance.fieldList.Count;
        yield return new WaitForSeconds(1f);
        for (int j = 0; j < 6; j++)
        {
            for (int i = 0; i < fieldCount; i++)
            {
                yield return ActiveOutline(i, j % 2 != 0, Utils.EnemyColor, 0);
            }
            yield return new WaitForSeconds(.2f);
        }
            
        BombCard bCard = inBombCard.GetComponent<BombCard>();
        Destroy(bCard);
        CardManager.Instance.CardDie(inBombCard);

        for (int i = 0; i < fieldCount; i++)
        {
            Field field = NewFieldManager.Instance.fieldList[i];

            Global.Pool.GetItem<Effect_Spawn>().transform.position = field.transform.position + new Vector3(0, 1, 0);
        }
        // 게임오버
        isMountBomb = false;
    }
    public Card MountCard(int idx,uint itemUid)
    {
        if (NewFieldManager.Instance.fieldList.Count <= idx)
        {
            Debug.LogError("잘못된 필드 위치");
            return null;

        }

        Field field = NewFieldManager.Instance.fieldList[idx];
        if (field.avatarCard == null)
        {
            Item item = CardManager.Instance.FindEnemyItem(itemUid);
            if(item == null)
            {
                Debug.LogError("잘못된 아이템 UID");
                return null;
            }
            Card card = Global.Pool.GetItem<Card>();
            card.Setup(item, true, false);
            field.SetUp(card);
            isMountBomb = true;

        }
        return null;

    }
    public void DoAction(Dictionary<long,Action> actionDict)
    {

        if (actionDict.ContainsKey(currentState)) // "DO PRESET"
        {
            actionDict[currentState]?.Invoke();
        }
        else                                    // "DO DEFAULT"
        {
            print(Convert.ToString(currentState, 2));
            EnemyManager.Instance.EnemyAction();
        }
    }

    public void MountingCard(Card card, MountState mount)
    {
        var fieldNode = NewFieldManager.Instance.fields.GetNodeByData(NewFieldManager.Instance.enemyCard.curField);

        switch (mount)
        {
            case MountState.Prev:
                Mounting(card, fieldNode.PrevNode.Data);
                break;
            case MountState.Next:
                Mounting(card, fieldNode.NextNode.Data);
                break;
            case MountState.Hack:
                Mounting(card, CardManager.Instance.hackField);
                break;
        }
    }


    public void Mounting(Card card, Field setField)
    {
        if (GameManager.Instance.State == GameState.END) return;

        CardManager.Instance.LastUsedCardItem = card.item.ShallowCopy();

        if (setField == null)
        {
            CardManager.Instance.CardDie(card);
        }
        else
        {
            NewFieldManager.Instance.Spawn(setField, card);
        }
    }
    private Dictionary<long, Action<Action>> reflectAction = new Dictionary<long, Action<Action>>()
    {
        {  0b10001010011000, (a) => {
             if(Instance.IsWaitingCard(101))
            {
                Instance.Reflect();
            }
             else
            {
                a?.Invoke();
            }
        } },
        {  0b10000010011000, (a) => {
              if(Instance.IsWaitingCard(101))
            {
                Instance.Reflect();
            }
             else
            {
                a?.Invoke();
            }
        } },
        {  0b1000000100100110000011, (a) => {
              if(Instance.IsWaitingCard(101))
            {
                Instance.Reflect();
            }
             else
            {
                a?.Invoke();
            }
        } },
        {  0b10010011000, (a) => {
              if(Instance.IsWaitingCard(101))
            {
                Instance.Reflect();
            }
             else
            {
                a?.Invoke();
            }
        } },
        {  0b10011001, (a) => {
              if(Instance.IsWaitingCard(101))
            {
                Instance.Reflect();
            }
             else
            {
                a?.Invoke();
            }
        } },
        {  0b1010011000, (a) => {
              if(Instance.IsWaitingCard(101))
            {
                Instance.Reflect();
            }
             else
            {
                a?.Invoke();
            }
        } },
        {  0b100001010011000, (a) => {
              if(Instance.IsWaitingCard(101))
            {
                Instance.Reflect();
            }
             else
            {
                a?.Invoke();
            }
        } },
        {  0b10000000000100010010000, (a) => {
              if(Instance.IsWaitingCard(101))
            {
                Instance.Reflect();
            }
             else
            {
                a?.Invoke();
            }
        } },
        {  0b10011000, (a) => {
              if(Instance.IsWaitingCard(101))
            {
                Instance.Reflect();
            }
             else
            {
                a?.Invoke();
            }
        } },

    };
    public bool IsWaitingCard(uint uid)
    {
        return Instance.WaitingCard != null && Instance.WaitingCard.item.uid == uid;
    }
    public void CallOnReflect(Action act)
    {
        StartCoroutine(ReflectJudge(act));
    }
    private IEnumerator ReflectJudge(Action act)
    {
        InitState();
        currentState = currentState >> 8;
        Debug.LogError("CurrentState : " + Convert.ToString(currentState, 2));
        long state = currentState;

        if (reflectAction.ContainsKey(state))
        {
            Vector3 pos = CardManager.Instance.hackField.transform.position;
            pos += new Vector3(0, 5f, 0);
            WaitingCard.transform.DOScale(WaitingCard.transform.localScale * 2f, .2f);
            WaitingCard.transform.DOMove(pos, .4f);
            WaitingCard.transform.DORotate(new Vector3(40, 0, 0), .3f);
            yield return new WaitForSeconds(2);

            reflectAction[state].Invoke(act);
        }
        else
        {
            act?.Invoke();
        }


    }
    public void Reflect()
    {
        EnemyManager.Instance.PopItem(101);
        EnemyManager.Instance.RemoveItem(EnemyManager.Instance.PopItem(101));
        if (WaitingCard != null)
        {
            CardManager.Instance.CardDie(WaitingCard);
        }
        NewFieldManager.Instance.AvatarMove(NewFieldManager.Instance.playerCard.curField, () =>
        {
            TurnManager.ChangeTurn();
            if (TutorialManager.Instance != null && TutorialManager.Instance.isTutorial)
            {
                if (BattleTutorial.Instance != null)
                    BattleTutorial.Instance.isDoneNullity = true;
            }
        });
    }

    [ContextMenu("Turn2Phase2")]
    public void Turn2Phase2()
    {
        StartCoroutine(Turn2Phase2Col());
    }
    private IEnumerator Turn2Phase2Col()
    {
        isTurn2Phase2 = true;
        yield return BattleCameraController.ZoomInEnemy();
        yield return BattleCameraController.LetterBoxActive(true);
        yield return BattleCameraController.SubText("제법이군");
        yield return BattleCameraController.SubText("하지만 2 페이즈를 버틸수 있을까?");
        yield return BattleCameraController.LetterBoxActive(false);
        yield return BattleCameraController.ZoomOut();

        CardModelBrain CMB = NewFieldManager.Instance.enemyCard.LinkedModel;
        CMB.ModelObject.transform.DOMoveY(CMB.ModelObject.transform.position.y + 15f, 1.5f).SetEase(Ease.InElastic);
        yield return new WaitForSeconds(2f);
        CardManager.Instance.CardDie(NewFieldManager.Instance.enemyCard);
        NewFieldManager.Instance.enemyCard = null;
        isTurn2Phase2 = false;
        JudgementCard();
    }
}
