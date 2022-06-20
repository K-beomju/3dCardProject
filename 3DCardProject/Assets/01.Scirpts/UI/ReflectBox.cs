using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ReflectBox : MonoBehaviour
{
    [SerializeField]
    private Button activeBTN;

    private bool isActive = true;
    private void Start()
    {
        activeBTN.onClick.AddListener(ReflectBoxActive);
    }

    public void CallOnReflect()
    {
        isActive = true;
        activeBTN.gameObject.SetActive(true);
        transform.DOMoveY(250, 0.2f).SetEase(Ease.OutQuad);
    }

    public void CallAfterReflect()
    {
        isActive = false;
        transform.DOMoveY(-250, 0.2f).SetEase(Ease.OutQuad).OnComplete(()=> activeBTN.gameObject.SetActive(false));
    }

    public void ReflectBoxActive()
    {
        DOTween.Kill(this.gameObject);
        transform.DOMoveY(250 * (isActive ? -1 : 1), 0.2f).SetEase(Ease.OutQuad);
        isActive = !isActive;
    }
}
