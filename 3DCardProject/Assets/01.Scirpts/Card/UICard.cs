using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    [SerializeField] private Image card;
    [SerializeField] private Image cardImage;

    [SerializeField] private TMP_Text costTMP;
    [SerializeField] private TMP_Text atkTMP;
    [SerializeField] private TMP_Text hpTMP;
    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text descriptionTMP;

    [SerializeField] private Button selectButton = null;

    public Item item;
    public Card linkedCard;
    public void Setup(Item item,Card card)
    {
        this.item = item;
        this.linkedCard = card;
        cardImage.sprite = this.item.sprite;
        nameTMP.text = this.item.name;
        costTMP.text = this.item.cost.ToString();
        descriptionTMP.text = this.item.description;
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => { CallOnSelect(); Debug.Log("Select"); });
    }

    public void CallOnSelect()
    {
        ReflectBox.Instance.selectedCard = this;
    }

}
