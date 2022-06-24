using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer card;
    [SerializeField] private SpriteRenderer cardImage;

    [SerializeField] private SpriteRenderer crystal;

    [SerializeField] private Sprite crystal_blue;
    [SerializeField] private Sprite crystal_red;

    [SerializeField] private TMP_Text costTMP;
    [SerializeField] private TMP_Text atkTMP;
    [SerializeField] private TMP_Text hpTMP;
    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text descriptionTMP;


    public Item item;
    public PRS originPRS;
    private bool isFront;
    public bool isOnField = false;

    public bool isPlayerCard = true;

    public Field curField;
    public bool isAttack = false;

    public void Setup(Item item, bool isFront, bool isPlayerCard)
    {
        this.isPlayerCard = isPlayerCard;
        this.item = item;
        this.isFront = isFront;

        if (this.isFront)
        {
            RefreshInform();
            
        }
        else
        {
            atkTMP.text = "";
            nameTMP.text = "";
            costTMP.text = "";
            descriptionTMP.text = "";
        }
    }

    void OnMouseOver()
    {
        CardManager.Instance.CardMouseOver(this);
    }

    void OnMouseExit()
    {
        CardManager.Instance.CardMouseExit(this);
    }

    void OnMouseDown()
    {
        if (TurnManager.CurReturnType() != TurnType.Player) return;

        CardManager.Instance.CardMouseDown(this);

        //if (!isOnField)
        //{
        //    if (!CardManager.Instance.MyCardIsFull())
        //    {
        //        CardManager.Instance.DeleteMyCard(this);
        //        CardManager.Instance.CardAlignment();
        //    }
        //}


    }

    void OnMouseUp()
    {
        CardManager.Instance.CardMouseUp();
    }

    public void SetDeleteObject()
    {
        Destroy(gameObject, 1);
    }
    public void RefreshInform()
    {
        cardImage.sprite = this.item.sprite;
        crystal.sprite = isPlayerCard ? crystal_blue : crystal_red;
        nameTMP.text = this.item.name;
        costTMP.text = this.item.cost.ToString();
        descriptionTMP.text = this.item.description;
    }
    public void MoveTransform(PRS prs, bool useDotween, float dotWeenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotWeenTime);
            transform.DORotateQuaternion(prs.rot, dotWeenTime);
            transform.DOScale(prs.scale, dotWeenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
    public void Emphasize(Action act)
    {

        transform.DOScaleZ(transform.localScale.z + .5f, 0.15f).SetLoops(2, LoopType.Yoyo);
        transform.DOScaleX(transform.localScale.x + .5f, 0.15f).SetLoops(2, LoopType.Yoyo);
        transform.DOScaleY(transform.localScale.y + .5f, 0.15f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            act?.Invoke();
        });
    }
   
    public void Attack(Field field)
    {
        if (!isAttack && (field.curCard != this || field.upperCard != this||field.avatarCard != this))
        {
            isAttack = true;
            int originOrder = GetComponent<Order>().GetOriginalOrder();
            GetComponent<Order>().SetOriginOrder(10);
            Vector3 firstPos = transform.position;
            Vector3 fieldPos = field.transform.position;
            fieldPos.y += 1f;
            transform.DOMove(fieldPos, .15f).SetEase(Ease.InElastic).OnComplete(() => {


                transform.DOMove(firstPos, .3f).OnComplete(() => {
                    GetComponent<Order>().SetOriginOrder(originOrder);
                    if (field.curCard != null)
                    {
                        Debug.Log("카드 공격");
                        field.curCard.OnDamage();
                    }
                    else if(field.upperCard != null)
                    {
                        Debug.Log("Upper 카드 공격");
                        field.upperCard.OnDamage();
                    }
                    else if( field.avatarCard != null)
                    {
                        Debug.Log("아바타 카드 공격");
                        field.avatarCard.OnDamage();
                    }
                    isAttack = false;
                    OnAttack();

                });
            });
        }
    }

    // 덫 부분 구현
    public void CommonAction(Field field)
    {
        if (field.curCard != this)
        {
            if (field.curCard != null)
            {
                field.curCard.OnAttack();
            }
        }
    }


    public void OnCreateCard()
    {
        Debug.Log("OnCreate : " + item.name);

        CardAction(item.OnCreate);
    }
    public void OnAttack()
    {
        Debug.Log("ONATTACK : " + item.name);

        CardAction(item.OnAttack);
    }
    public void OnDamage()
    {
        Debug.Log("ONDAMAGE: " + item.name);
        CardAction(item.OnDamage);
    }
    public void OnDie()
    {
        Debug.Log("ONDIE: " + item.name);
        CardAction(item.OnDie);
    }
    public void OnSpawn()
    {
        Debug.Log("ONSPAWN : " + item.name);
        Emphasize(() =>
        {
            CardAction(item.OnSpawn);
            CardManager.Instance.LastUsedCardItem = item.ShallowCopy();
        });
    }

    private void CardAction(CardActionCondition[] act)
    {
        foreach (var item in act)
        {
            if (item == null || item.action == null)
            {
                continue;
            }
            int check = 0;
            foreach (var condition in item.condition)
            {
                if (!condition.CheckCondition())
                {
                    check++;
                }
            }
            if (check == 0)
                item.action.TakeAction(this);
        }
    }

    public Card ShallowCopy()
    {
        return (Card)this.MemberwiseClone();
    }
}
