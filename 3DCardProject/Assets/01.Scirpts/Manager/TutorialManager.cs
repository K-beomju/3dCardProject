using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField]
    private RectTransform panelRTrm;
    [SerializeField]
    private TextMeshProUGUI explainText;
    [SerializeField]
    private CanvasGroup subsPanel;

    public bool isTutorial = false;

    private void Start()
    {
        subsPanel.alpha = 0;
    }

    public void Fade(bool inBool)
    {
        subsPanel.DOFade(inBool ? 1 : 0, .4f);
    }
    public void Explain(string msg, float PosY)
    {
        explainText.text = msg;
        panelRTrm.anchoredPosition = new Vector2(0, PosY) ;
    }
    public IEnumerator ExplainCol(string msg, int posY, float delay = 1.5f)
    {
        yield return new WaitForSeconds(1);
        Explain(msg, posY);
        Fade(true);
        yield return new WaitForSeconds(delay);
        Fade(false);
    }
}
