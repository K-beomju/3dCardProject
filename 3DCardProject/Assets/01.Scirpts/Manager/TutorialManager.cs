using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField]
    private RectTransform panelRTrm;
    [SerializeField]
    private TextMeshProUGUI explainText;

    public void Explain(string msg, float PosY)
    {
        explainText.text = msg;
        panelRTrm.anchoredPosition = new Vector2(0, PosY) ;
    }
}
