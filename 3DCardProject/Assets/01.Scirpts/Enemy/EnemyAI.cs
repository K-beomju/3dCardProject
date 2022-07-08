using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyAI : Singleton<EnemyAI>
{
    private Action action;
    private Dictionary<long, Action> _actions = new Dictionary<long, Action>() // // �ƹ�Ÿ ���� �븻 �÷��̾� ����
    {
        //����Ƽ event 
        { 0b100000100000001001000011111111, () => {Debug.Log("1");EnemyManager.Instance.EnemyAction(); } }, // ��


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
            //EnemyManager.Instance.EnemyAction();
        }
    }

    public void BuildingStruct()
    {

    }
}
