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

public class EnemyAI : Singleton<EnemyAI>
{

    private MountState mountState;
    private Action action;
    private Dictionary<long, Action> _actions = new Dictionary<long, Action>() // // �ƹ�Ÿ ���� �븻 �÷��̾� ����
    {
        //����Ƽ event 
        { 
            0b100000100000001001000011111111, () => {
                CardManager.Instance.MountCardSupport(107 , MountState.Prev);

        } }, // �Ϲ�ī�� 

        {
            0b101000000000101001000011111111, () => {
                CardManager.Instance.MountCardSupport(100);

        } }, // �Ϲ�ī�� -> ���� ī��  
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
        //print(Convert.ToString(currentState, 2));
    }

    [ContextMenu("JudgementCard")]
    public void JudgementCard()
    {
        InitState();

        if (_actions.ContainsKey(currentState)) // �̸� �����ص� �ൿ
        {
            Debug.Log("DO PRESET");
            _actions[currentState]?.Invoke();
        }
        else // �⺻ �ൿ
        {
            print("����� ���� ����");
            print(Convert.ToString(currentState, 2));
            //Debug.Log("DO DEFAULT");
            EnemyManager.Instance.EnemyAction();
        }
    }
    //// �տ� ��ġ (ī��)
    //public void MountingPrev(Card card)
    //{
    //    var fieldNode = NewFieldManager.Instance.fields.GetNodeByData(NewFieldManager.Instance.enemyCard.curField);
    //    Mounting(card, fieldNode.PrevNode.Data);
    //}
    //// �ڿ� ��ġ (ī��)
    //public void MountingNext(Card card)
    //{
    //    var fieldNode = NewFieldManager.Instance.fields.GetNodeByData(NewFieldManager.Instance.enemyCard.curField);
    //    Mounting(card, fieldNode.NextNode.Data);
    //}
    //// �ٿ� ��ġ (ī��)
    //public void MountingOnHack(Card card)
    //{
    //    Mounting(card,CardManager.Instance.hackField);
    //}

    public void MountingCard(Card card, MountState mount)
    {
        var fieldNode = NewFieldManager.Instance.fields.GetNodeByData(NewFieldManager.Instance.enemyCard.curField);

        switch (mount)
        {
            case MountState.Prev:  //�ڿ� ��ġ (ī��)
                Mounting(card, fieldNode.PrevNode.Data);
                break;
            case MountState.Next:  //�տ� ��ġ(ī��)
                Mounting(card, fieldNode.NextNode.Data);
                break;
            case MountState.Hack: //�ٿ� ��ġ(ī��)
                Mounting(card, CardManager.Instance.hackField);
                break;
        }
    }



    // ��ġ (��ġ , ī��)
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
