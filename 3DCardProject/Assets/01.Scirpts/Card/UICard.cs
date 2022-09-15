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
    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text descriptionTMP;


    public Card linkedCard;
    public void Setup(Card card)
    {
        this.linkedCard = card.ShallowCopy();
        cardImage.sprite = linkedCard.item.sprite;
        nameTMP.text = linkedCard.item.itemName;
        costTMP.text = linkedCard.item.cost.ToString();
        descriptionTMP.text = linkedCard.item.description;
    }


}
