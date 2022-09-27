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

    private void Awake()
    {
        anim.speed = 0;
    }
    private void Start()
    {
        SoundManager.Instance.PlayFXSound("Kill This Love",1);
        anim.speed = 1;
        float width = 0;
        for (int i = 0; i < Credit.childCount; i++)
        {
            width += Credit.GetChild(i).GetComponent<RectTransform>().rect.height;
        }
        Credit.DOAnchorPosY(1080 + width , SoundManager.Instance.GetFxSound("Kill This Love").length).SetRelative().SetEase(Ease.Linear).OnComplete(()=> {
            Debug.Log("END");
        });
    }
}
