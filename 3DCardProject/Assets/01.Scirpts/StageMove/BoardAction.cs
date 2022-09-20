using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public enum StageType
{
    None,
    Battle,
    Shop,
    GetHP,
    GetGold,
    LossHp,
    LossGold
}




public abstract class BoardAction : MonoBehaviour
{
    public StageType type = StageType.Battle;
    public EnemyType enemyType;
    public bool isClear = false;
    public uint uid;

   
    public virtual void ClearAction()
    {
        if (!isClear)
        {
            isClear = true;
            PlayMethod();
        }
    }

    // 설계가 완료되면 구현 
    public void PlayMethod()
    {
        if(type == StageType.Battle)
        {

        }
    }

 



}
