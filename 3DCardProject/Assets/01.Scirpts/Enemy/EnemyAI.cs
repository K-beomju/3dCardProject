using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyAI : MonoBehaviour
{
    private Action action;
    private Dictionary<long, Action> _actions = new Dictionary<long, Action>() // // 아바타 어퍼 노말 플레이어 구별
    {
        { 0b0000_0000_0000_0000_0000_0000_0000_0000, () => { } },
        { 0b0000_0000_0000_0000_0000_0000_0000_0000, () => { } },
        { 0b0000_0000_0000_0000_0000_0000_0000_0000, () => { } },
        { 0b0000_0000_0000_0000_0000_0000_0000_0000, () => { } },
        { 0b0000_0000_0000_0000_0000_0000_0000_0000, () => { } },
        { 0b0000_0000_0000_0000_0000_0000_0000_0000, () => { } },
        { 0b0000_0000_0000_0000_0000_0000_0000_0000, () => { } },
        { 0b0000_0000_0000_0000_0000_0000_0000_0000, () => { } },
        { 0b0000_0000_0000_0000_0000_0000_0000_0000, () => { } },
        { 0b0000_0000_0000_0000_0000_0000_0000_0000, () => { } },
    };


    private long state = 0b0000_0000_0000_0000_0000_0000_0000_0000;

    [ContextMenu("InitState")]
    public void InitState()
    {
        state = 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;

        for (int i = 0; i < NewFieldManager.Instance.fieldList.Count; i++)
        {
            var field = NewFieldManager.Instance.fieldList[i];

            if (field.avatarCard != null)
            {
                if (field.avatarCard.isPlayerCard)
                {
                    state += 0b0000_0000_0000_0000_0000_0001_0000_0000;
                }
                state += 0b0000_0000_0000_0000_0000_1000_0000_0000;
            }
            if (field.upperCard != null)
                state += 0b0000_0000_0000_0000_0000_0100_0000_0000;
            if (field.curCard != null)
                state += 0b0000_0000_0000_0000_0000_0010_0000_0000;

            if (i != NewFieldManager.Instance.fieldList.Count - 1)
                state <<= 4;
        }


        DeckManager dm = EnemyManager.Instance.dm;

        for (int i = 0; i < dm.itemBuffer.Count; i++)
        {
            switch (dm.itemBuffer[i].uid)
            {
                case 100:
                    state += 0b0000_0000_0000_0000_0000_0000_0000_1000_0000;
                    break;
                case 101:
                    state += 0b0000_0000_0000_0000_0000_0000_0000_0100_0000;
                    break;
                case 102:
                    state += 0b0000_0000_0000_0000_0000_0000_0000_0010_0000;
                    break;
                case 103:
                    state += 0b0000_0000_0000_0000_0000_0000_0000_0001_0000;
                    break;
                case 104:
                    state += 0b0000_0000_0000_0000_0000_0000_0000_0000_1000;
                    break;
                case 105:
                    state += 0b0000_0000_0000_0000_0000_0000_0000_0000_0100;
                    break;
                case 106:
                    state += 0b0000_0000_0000_0000_0000_0000_0000_0000_0010;
                    break;
                case 107:
                    state += 0b0000_0000_0000_0000_0000_0000_0000_0000_0001;
                    break;
            }
        }
        print(Convert.ToString(state, 2));
    }

    public void CheckDrawCard()
    {
       
    }


}
