using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Define
{
    public enum Sound
    {
        Bgm,
        Effect,
        NotOneShot,
        MaxCount,  // 아무것도 아님. 그냥 Sound enum의 개수 세기 위해 추가. (0, 1, '2' 이렇게 2개) 
    }

    #region UI Type
    [Flags]
    public enum UIFadeType
    {
        FADE =          1,
        FLOAT =         2,
        //==================================
        OUT =           0,
        IN =            FADE,
        FLOATOUT =      FLOAT,
        FLOATIN =       FADE | FLOAT
    }

    public enum UIEffectType
    {
        SHAKE = 1,
    }
    #endregion

    public enum ItemSlot
    {
        HEAD,
        ARM,
        LEG,
        MAXCOUNT
    }
}
