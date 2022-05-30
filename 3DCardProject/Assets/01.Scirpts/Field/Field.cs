using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class Field : MonoBehaviour
{
    [field: SerializeField]
    public Card curCard { get; private set; }
    public bool isPlayerField = true;

    private SpriteRenderer sr;
    private Color32 aColor;
    private Color32 cColor;
    private Color32 sColor;
    public bool isHit = false;
    public bool isSelected = false;
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
        curCard = card;
        curCard.curField = this;
        curCard.isOnField = true;
    
        curCard.transform.DOMove(new Vector3(transform.position.x, transform.position.y + .15f, transform.position.z), .2f).OnComplete(() => {
            curCard.transform.DORotateQuaternion(transform.rotation, .1f);
            curCard.Emphasize(() => {
                foreach (var item in curCard.item.OnSpawn)
                {
                    foreach (var condition in item.condition)
                    {
                        if (!condition.CheckCondition())
                        {
                            return;
                        }
                    }
                    item.action.TakeAction(curCard);
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
