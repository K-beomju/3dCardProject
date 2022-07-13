using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class Field : MonoBehaviour
{
    [field: SerializeField]
    public Card curCard { get; private set; }
    [field: SerializeField]
    public Card upperCard { get; private set; }
    [field: SerializeField]
    public Card avatarCard { get; private set; }

    private SpriteRenderer sr;
    private Color32 aColor;
    private Color32 cColor;
    private Color32 sColor;
    public bool isHit = false;
    public bool isSelected = false;
    public bool isEnterRange = false;
    public bool isHackField = false;

    public bool isCommon = false;
    

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
    private void Start()
    {
        isHackField = GetComponent<Hack>() != null;
        if (isHackField)
        {
            CardManager.Instance.hackField = this;
        }
    }
    public void HitColor(bool _isHit)
    {
        isHit = true;
        sr.color = _isHit ? aColor : sColor;
    }

    public void FieldSelect(bool inBool)
    {
        isSelected = inBool;
        sr.color = inBool ? aColor : sColor;
    }

    private void Update()
    {
        if (isHit && isEnterRange)
        {
            if (CardManager.Instance.hitField != this || CardManager.Instance.selectCard == null)
            {
                //Debug.Log("Ray 나감");
                isHit = false;
                if (!isSelected)
                    HitColor(isHit);
            }
        }
    }

    public void SetUp(Card card,Action act = null)
    {
        //if ((enableTribe & card.item.tribe) == CardTribeType.NULL) return;
        if (card.item.IsAvatar)
        {
            Debug.Log("SetAvatarCard");
            avatarCard = card;
            avatarCard.curField = this;
            avatarCard.isOnField = true;
        }
        else if (card.item.IsUpperCard)
        {
            Debug.Log("SetUpperCard");
            upperCard = card;
            upperCard.curField = this;
            upperCard.isOnField = true;

        } 
        else
        {
            Debug.Log("SetCard");
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

        Sequence mySequence = DOTween.Sequence();

        Vector3 pos = transform.position;
        pos += new Vector3(0, 1f, 0);

        mySequence.Append(card.transform.DOMove(pos, .3f)).AppendInterval(.3f);
        mySequence.Join(card.transform.DORotateQuaternion(transform.rotation, .1f));
        mySequence.Append(card.transform.DOMove(pos -= new Vector3(0, .45f, 0), .2f)).OnComplete(() => {
            TurnManager.Instance.CanChangeTurn = true;
            if (card.LinkedModel != null && card.item.IsAvatar)
            {
                card.LinkedModel.Move(card.transform.position,act);
                Debug.Log("이동 : " + card.item.name);
            }
            else
            {
                act?.Invoke();
            }
        });
    }
    public void RemoveCurCard()
    {
        curCard.curField = null;
        curCard = null;
    }
    public void RemoveUpperCard()
    {
        upperCard.curField = null;
        upperCard= null;
    }
    public void RemoveAvatarCard()
    {
        avatarCard.curField = null;
        avatarCard = null;
    }


}

public enum FieldType
{
    NORMAL = 0b0000,
    BUSH = 0b0001,
    WALL = 0b0010,
    BUFF = 0b0100,
}