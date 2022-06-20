using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ReflectBox : Singleton<ReflectBox>
{
    [SerializeField]
    private Button activeBTN;
    [SerializeField]
    private Button confirmBTN;
    [SerializeField]
    private Button cancelBTN;

    [SerializeField]
    private GameObject cardUIPrefab = null;

    [SerializeField]
    private Transform cardUIparent;

    public List<GameObject> CardUIList { get; private set; } = new List<GameObject>();

    private bool isActive = true;

    public UICard selectedCard = null;
    private void Start()
    {
        ReflectBoxActive(false);
        activeBTN.onClick.AddListener(ReflectBoxActive);
        confirmBTN.onClick.AddListener(ConfirmReflect);
        cancelBTN.onClick.AddListener(CancelReflect);
        CardManager.Instance.OnReflect += CallOnReflect;
    }

    public void AddCardUI(Item item,Card card)
    {
        GameObject cardUIGO = Instantiate(cardUIPrefab, cardUIparent);
        cardUIGO.GetComponent<UICard>().Setup(item,card);
        CardUIList.Add(cardUIGO);
    }
    public void RemoveCardUI(GameObject inGO)
    {
        
        Card card = inGO.GetComponent<UICard>().linkedCard;
        
        if (!card.item.IsStructCard)
            card.OnAttack();

        CardUIList.Remove(inGO);
        Destroy(inGO);
    }

    public void CallOnReflect()
    {
        if(CardUIList.Count > 0)
        {
            // �ø���
            ReflectBoxActive(true);
        }
    }

    public void ConfirmReflect()
    {
        if(selectedCard != null)
        {
            RemoveCardUI(selectedCard.gameObject);
            // �ڽ� ������
            ReflectBoxActive(false);
        }
        else
        {
            Debug.LogWarning("ī�带 �����ؾ� �մϴ�");
        }
    }
    public void CancelReflect()
    {
        CardManager.Instance.WaitingActionUntilFinishOnReflect?.Invoke();
        ReflectBoxActive(false);
    }
    public void ReflectBoxActive(bool inBool)
    {
        transform.DOMoveY(250 * (isActive ? -1 : 1), 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            isActive = inBool;
            activeBTN.transform.parent.gameObject.SetActive(inBool);
            confirmBTN.transform.parent.gameObject.SetActive(inBool);
            cancelBTN.transform.parent.gameObject.SetActive(inBool);
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
