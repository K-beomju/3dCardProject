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
    public bool isReflectOnHand
    {
        get
        {
            return UnityEngine.Random.Range(0,2) == 0 &&  EnemyManager.Instance.IsHaveItem(101);
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
                if (isPhaseOne)
                {
                    DoAction(mediumActions);
                }
                else
                {
                    // 다음턴에 특정 위치에 있으면 HP--;
                    DoAction(bossPhaseTwoAction);
                }
                break;
            default:
                break;
        }
    }
    public void BossAciton(int actNum)
    {
        switch (actNum)
        {
            case 0:
                break;
            case 1:
                StartCoroutine(BossActionRedFloor());
                break;
            default:
                break;
        }
    }
    public IEnumerator BossActionRedFloor()
    {
        bool isOdd = UnityEngine.Random.Range(0, 1) == 0;
        for (int i = 0; i < NewFieldManager.Instance.fieldList.Count; i++)
        {
            if (isOdd ? i % 2 != 0 : i % 2 == 0)
                continue;

            yield return new WaitForSeconds(.6f);
            NewFieldManager.Instance.fieldList[i].outline.enabled = true;
            NewFieldManager.Instance.fieldList[i].outline.OutlineColor = Utils.EnemyColor;
        }
        
        for (int i = 0; i < NewFieldManager.Instance.fieldList.Count; i++)
        {
            if (isOdd ? i % 2 != 0 : i % 2 == 0)
                continue;

            yield return new WaitForSeconds(.6f);
            NewFieldManager.Instance.fieldList[i].outline.enabled =false;
        }

        yield return new WaitForSeconds(.6f);
        TurnManager.Instance.OnTurnChange2Enemy += () => {
            for (int i = 0; i < NewFieldManager.Instance.fieldList.Count; i++)
            {
                if (isOdd ? i % 2 != 0 : i % 2 == 0)
                    continue;
                if(NewFieldManager.Instance.fieldList[i].avatarCard.item.uid == NewFieldManager.Instance.playerCard.item.uid)
                {
                    // 체력 감소
                    SaveManager.Instance.gameData.Hp--;
                }
            }
            TurnManager.Instance.OnTurnChange2Enemy = null;
        };

        TurnManager.ChangeTurn();
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
    private Dictionary<long, Action> reflectAction = new Dictionary<long, Action>()
    {
        {  0b111111111111, () => {

        } },
    };
    public void CallOnReflect(Action act)
    {
        StartCoroutine(ReflectJudge(act));
    }
    private IEnumerator ReflectJudge(Action act)
    {
        long state = GetCurrentStateForReflect();

        if (reflectAction.ContainsKey(state))
        {
            Vector3 pos = CardManager.Instance.hackField.transform.position;
            pos += new Vector3(0, 5f, 0);
            WaitingCard.transform.DOScale(WaitingCard.transform.localScale * 2f, .2f);
            WaitingCard.transform.DOMove(pos, .4f);
            WaitingCard.transform.DORotate(new Vector3(40, 0, 0), .3f);
            yield return new WaitForSeconds(3);

            EnemyManager.Instance.PopItem(101);
            reflectAction[state].Invoke();
            if (WaitingCard != null)
            {
                CardManager.Instance.CardDie(WaitingCard);
            }
        }
        else
        {
            act?.Invoke();
        }


    }
    public long GetCurrentStateForReflect()
    {

        long curState = 0b0_00_00_00_00_00_00_00000000;

        for (int i = 0; i < NewFieldManager.Instance.fieldList.Count; i++)
        {
            var field = NewFieldManager.Instance.fieldList[i];

            if (field.avatarCard != null)
            {
                if (field.avatarCard.isPlayerCard)
                {
                    curState += 0b0_00_00_00_00_00_00_00000000;
                }
                curState += 0b0_00_00_00_00_00_00_00000000;
            }
            if (field.upperCard != null)
                curState += 0b0_00_00_00_00_00_00_00000000;
            else if (field.curCard != null)
                curState += 0b0_00_00_00_00_00_00_00000000;

            if (i != NewFieldManager.Instance.fieldList.Count - 1)
                curState <<= 4;
        }

        switch (WaitingCard.item.uid)
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
        return curState;
    }

    public void Turn2Phase2()
    {
        StartCoroutine(Turn2Phase2Col());
    }
    private IEnumerator Turn2Phase2Col()
    {
        yield return new WaitForSeconds(1);
        CardManager.Instance.CardDie(NewFieldManager.Instance.enemyCard);
    }
}
