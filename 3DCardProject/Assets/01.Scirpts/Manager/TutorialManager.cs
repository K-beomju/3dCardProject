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

    public IEnumerator Fade(bool inBool, byte nul = 0)
    {
        yield return subsPanel.DOFade(inBool ? 1 : 0, .4f);
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
    public IEnumerator ExplainCol(string msg, int posY, float fadeInDelay = 1f, float fadeOutDelay = 1.5f)
    {
        yield return new WaitForSeconds(fadeInDelay);
        Explain(msg, posY);
        yield return Fade(true,0);
        SoundManager.Instance.PlayFXSound("TutorialExplain", 0.1f);
        yield return new WaitForSeconds(fadeOutDelay);
        Fade(false);
    }
}
