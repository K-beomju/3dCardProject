using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public enum StageType
{
    Battle,
    Shop,
    Upgrade
}




public abstract class BoardAction : MonoBehaviour
{
    public StageType type = StageType.Battle;
    public EnemyType enemyType;
    public bool isClear = false;
    public ItemArraySO stageData;

   
    public virtual void ClearAction()
    {
        if (!isClear)
        {
            isClear = true;
            PlayMethod();
        }
    }

    // ���谡 �Ϸ�Ǹ� ���� 
    public void PlayMethod()
    {
        if(type == StageType.Battle)
        {

        }
    }

 



}
