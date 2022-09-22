using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CrossArrow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rtm;

    private void Awake()
    {
        rtm = this.GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rtm.DOSizeDelta(new Vector2(200, 30), .3f).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DOTween.KillAll(this);
        rtm.sizeDelta = new Vector2(200, 22);
    }

  
}
