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
    public static bool isClick = true;
    //[SerializeField] private CanvasGroup turnPanel;
    //[SerializeField] private TMP_Text turnText;

    private void Start()
    {
        mainCam = Camera.main;
    }

    // �� �ٲ� 
    public static void ChangeTurn(TurnType _type, ref bool isClick)
    {
        Instance.type = _type;
        if (_type == TurnType.Player)
        {
            Instance.mainCam.transform.DOMoveZ(-10.5f, 0.5f).OnComplete(() => PlayerManager.TurnReset());
            Instance.mainCam.transform.DORotate(new Vector3(45, 0, 0), 0.5f);
            isClick = true;
        }
        else
        {
            isClick = false;
            Instance.mainCam.transform.DOMoveZ(-5f, 0.5f);
            Instance.mainCam.transform.DORotate(new Vector3(55, 0, 0), 0.5f);
        }
        


        //Instance.turnText.text = "�÷��̾� ��";
        //Instance.turnText.text = "�� ��";
        //Instance.turnPanel.DOFade(1, 0.5f).OnComplete(() => Instance.turnPanel.DOFade(0, 0.5f));
    }

    // ���� Ÿ�� ��ȯ 
    public static TurnType CurReturnType()
    {
        return Instance.type;
    }

 

}
