using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class BoardAction : MonoBehaviour
{
    public bool isClear = false;

   
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

    }

 



}
