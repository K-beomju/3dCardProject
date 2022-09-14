using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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
    MEDIUM
}

public class EnemyAI : Singleton<EnemyAI>
{
    public EnemyType enemyType;
    private MountState mountState;
    private Action action;

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

        if (TutorialManager.Instance.isTutorial)
        {
            enemyType = EnemyType.TUTORIAL;

            if (tutorialActions.ContainsKey(currentState)) // "DO PRESET"
            {
                tutorialActions[currentState]?.Invoke();
            }
            else
            {
                EnemyManager.Instance.EnemyAction();
                print(Convert.ToString(currentState, 2));

            }
        }
        else
        {

            if (enemyType == EnemyType.EASY)
            {
                EnemyManager.Instance.EnemyAction();
            }
            if (enemyType == EnemyType.MEDIUM)
            {

                if (mediumActions.ContainsKey(currentState)) // "DO PRESET"
                {
                    mediumActions[currentState]?.Invoke();
                }
                else                                    // "DO DEFAULT"
                {
                    print(Convert.ToString(currentState, 2));
                    EnemyManager.Instance.EnemyAction();
                }
            }
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
}
