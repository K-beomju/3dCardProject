using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICard : MonoBehaviour
{
    [SerializeField] private SpriteRenderer card;
    [SerializeField] private SpriteRenderer cardImage;

    [SerializeField] private TMP_Text costTMP;
    [SerializeField] private TMP_Text atkTMP;
    [SerializeField] private TMP_Text hpTMP;
    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text descriptionTMP;
    public Item item;
    public void Setup(Item item, bool isPlayerCard)
    {
        this.item = item;

        cardImage.sprite = this.item.sprite;
        nameTMP.text = this.item.name;
        costTMP.text = this.item.cost.ToString();
        descriptionTMP.text = this.item.description;
    }

    private void OnMouseOver()
    {
        
    }

    private void OnMouseUp()
    {
        
    }
}
