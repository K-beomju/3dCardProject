using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

[System.Serializable]
public enum TurnType
{
    Player,
    Enemy
}

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField] private TurnType type;
    //[SerializeField] private CanvasGroup turnPanel;
    //[SerializeField] private TMP_Text turnText;

    // 턴 바꿈 
    public static void ChangeTurn(TurnType _type)
    {
        Instance.type = _type;
        //if(_type == TurnType.Player)
        //    Instance.turnText.text = "플레이어 턴";
        //else
        //    Instance.turnText.text = "적 턴";

        //Instance.turnPanel.DOFade(1, 0.5f).OnComplete(() => Instance.turnPanel.DOFade(0, 0.5f));
    }

    // 현재 타입 반환 
    public static TurnType CurReturnType()
    {
        return Instance.type;
    }

 

}
