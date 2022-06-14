using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class Field : MonoBehaviour
{
    [field: SerializeField]
    public Card curCard { get; private set; }
    public Card upperCard { get; private set; }

    public bool isPlayerField = true;

    private SpriteRenderer sr;
    private Color32 aColor;
    private Color32 cColor;
    private Color32 sColor;
    public bool isHit = false;
    public bool isSelected = false;

    [SerializeField]
    [EnumFlags]
    private FieldType fieldType;
    public FieldType FieldType
    {
        get
        {
            return fieldType;
        }
        set
        {
            fieldType = value;
        }
    }

    [SerializeField]
    [EnumFlags]
    private CardTribeType enableTribe;
    public CardTribeType EnableTribe
    {
        get
        {
            return enableTribe;
        }
        set
        {
            enableTribe = value;
        }
    }
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        aColor = new Color32(100, 100, 255, 255);
        cColor = new Color32(255, 100, 100, 255);
        sColor = new Color32(255, 255, 255, 255);
    }
    public void HitColor(bool _isHit)
    {
        isHit = true;
        sr.color = _isHit ? (isPlayerField ? aColor : cColor) : sColor;
    }

    public void FieldSelect(bool inBool)
    {
        isSelected = inBool;
        sr.color = inBool ? aColor : sColor;
    }

    private void Update()
    {
        if (isHit)
        {
            if (CardManager.Instance.hitField != this || CardManager.Instance.selectCard == null)
            {
                //Debug.Log("Ray ³ª°¨");
                isHit = false;
                if (!isSelected)
                    HitColor(isHit);
            }
        }
    }

    public void SetUp(Card card)
    {
        if ((enableTribe & card.item.tribe) == CardTribeType.NULL) return;
        if (card.item.isUpperCard)
        {
            upperCard = card;
            upperCard.curField = this;
            upperCard.isOnField = true;
        }
        else
        {
            curCard = card;
            curCard.curField = this;
            curCard.isOnField = true;
            if (upperCard != null)
            {

            }
        }
        card.transform.DOScaleZ(transform.localScale.z * .7f, 0.15f);
        card.transform.DOScaleX(transform.localScale.x * .7f, 0.15f);
        card.transform.DOScaleY(transform.localScale.y * .7f, 0.15f);

        card.transform.DOMove(new Vector3(transform.position.x, transform.position.y + .15f, transform.position.z), .2f).OnComplete(() => {


            card.transform.DORotateQuaternion(transform.rotation, .1f);
            card.Emphasize(() => {
                foreach (var item in card.item.OnSpawn)
                {
                    int check = 0;
                    foreach (var condition in item.condition)
                    {
                        if (!condition.CheckCondition())
                        {
                            check++;
                        }
                    }
                    if (check == 0)
                        item.action.TakeAction(card);
                }
            });


        });
    }
    public void RemoveCard()
    {
        curCard.curField = null;
        curCard = null;
    }

}

public enum FieldType
{
    NORMAL = 0b0000,
    BUSH = 0b0001,
    WALL = 0b0010,
    BUFF = 0b0100,
}