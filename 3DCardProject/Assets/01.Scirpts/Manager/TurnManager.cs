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

    // �� �ٲ� 
    public static void ChangeTurn(TurnType _type)
    {
        Instance.type = _type;
        //if(_type == TurnType.Player)
        //    Instance.turnText.text = "�÷��̾� ��";
        //else
        //    Instance.turnText.text = "�� ��";

        //Instance.turnPanel.DOFade(1, 0.5f).OnComplete(() => Instance.turnPanel.DOFade(0, 0.5f));
    }

    // ���� Ÿ�� ��ȯ 
    public static TurnType CurReturnType()
    {
        return Instance.type;
    }

 

}
