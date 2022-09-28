using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CreditManager : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private RectTransform Credit;
    [SerializeField]
    private CanvasGroup cg;

    private void Awake()
    {
        anim.speed = 0;
    }
    private void Start()
    {
        Sequence seq1 = DOTween.Sequence();
        seq1.SetUpdate(true);
        seq1.AppendInterval(2f);
        seq1.Append(cg.DOFade(0, 2f).SetEase(Ease.Linear));
        SoundManager.Instance.PlayFXSound("Kill This Love", 0.1f);
        anim.speed = 1;
        float width = 0;
        for (int i = 0; i < Credit.childCount; i++)
        {
            width += Credit.GetChild(i).GetComponent<RectTransform>().rect.height;
        }
        Sequence seq2 = DOTween.Sequence();
        seq2.SetEase(Ease.Linear);
        seq2.SetUpdate(true);
        seq2.Append(Credit.DOAnchorPosY(1080 + width, SoundManager.Instance.GetFxSound("Kill This Love").length).SetRelative());
        seq2.Append(cg.DOFade(1, 2f));
        seq2.AppendCallback(() => {
            Global.LoadScene.LoadScene("Title");
        });
    }
}
