using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string btnName;

    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuManager.Instance.SelectButton(btnName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MenuManager.Instance.SelectButton("환경 설정");

    }
}
