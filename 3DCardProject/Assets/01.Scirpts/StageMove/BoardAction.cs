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

    // ���谡 �Ϸ�Ǹ� ���� 
    public void PlayMethod()
    {

    }

 



}
