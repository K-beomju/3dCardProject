using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ReflectBox : MonoBehaviour
{
    [SerializeField]
    private Button activeBTN;
    [SerializeField]
    private Button confirmBTN;

    private bool isActive = true;
    private void Start()
    {
        ReflectBoxActive(false);
        activeBTN.onClick.AddListener(ReflectBoxActive);
        confirmBTN.onClick.AddListener(ConfirmReflect);
        CardManager.Instance.OnReflect += CallOnReflect;
    }

    public void CallOnReflect()
    {
        // 올리기
        ReflectBoxActive(true);
    }

    public void ConfirmReflect()
    {

        // 박스 내리기
        ReflectBoxActive(false);
    }
    public void ReflectBoxActive(bool inBool)
    {
        isActive = inBool;
        transform.DOMoveY(-250, 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            activeBTN.transform.parent.gameObject.SetActive(inBool);
            confirmBTN.transform.parent.gameObject.SetActive(inBool);
        }
        );
    }
    public void ReflectBoxActive()
    {
        DOTween.Kill(this.gameObject);
        transform.DOMoveY(250 * (isActive ? -1 : 1), 0.2f).SetEase(Ease.OutQuad);
        isActive = !isActive;
    }
}
