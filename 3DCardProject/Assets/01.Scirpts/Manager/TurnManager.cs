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
    private Camera mainCam;
    //[SerializeField] private CanvasGroup turnPanel;
    //[SerializeField] private TMP_Text turnText;

    private void Start()
    {
        mainCam = Camera.main;
    }

    // 턴 바꿈 
    public static void ChangeTurn(TurnType _type)
    {
        Instance.type = _type;
        if (_type == TurnType.Player)
        {
            Instance.mainCam.transform.DOMoveZ(-10.5f, 0.5f).OnComplete(() => PlayerManager.TurnReset());
            Instance.mainCam.transform.DORotate(new Vector3(45, 0, 0), 0.5f);
        }
        else
        {
            Instance.mainCam.transform.DOMoveZ(-5f, 0.5f);
            Instance.mainCam.transform.DORotate(new Vector3(55, 0, 0), 0.5f);
        }
        


        //Instance.turnText.text = "플레이어 턴";
        //Instance.turnText.text = "적 턴";
        //Instance.turnPanel.DOFade(1, 0.5f).OnComplete(() => Instance.turnPanel.DOFade(0, 0.5f));
    }

    // 현재 타입 반환 
    public static TurnType CurReturnType()
    {
        return Instance.type;
    }

 

}
