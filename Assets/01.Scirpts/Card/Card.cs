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

    [SerializeField] private TMP_Text costTMP;
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
        if (item.isMagic)
        {
            card.color = new Color(0.5137255f, 0.6470588f, 0.9137256f);
        }
        else
        {
            card.color = new Color(0.9137255f, 0.5137255f, 0.5853466f);
        }
        if (this.isFront)
        {
            cardImage.sprite = this.item.sprite;
            nameTMP.text = this.item.name;
            costTMP.text = this.item.cost.ToString();
            descriptionTMP.text = this.item.description;
        }
        else
        {
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
        CardManager.Instance.CardMouseDown(this);

        if (!isOnField)
        {
            if (!CardManager.Instance.MyCardIsFull())
            {
                CardManager.Instance.DeleteMyCard(this);
                CardManager.Instance.CardAlignment();
            }
        }


    }

    void OnMouseUp()
    {
        CardManager.Instance.CardMouseUp();
    }

    public void SetDeleteObject()
    {
        Destroy(gameObject, 1);
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
        transform.DOScaleX(transform.localScale.x + .5f, 0.15f).SetLoops(2, LoopType.Yoyo);
        transform.DOScaleY(transform.localScale.y + .5f, 0.15f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            act?.Invoke();
        });
    }
    public void Attack(Field field)
    {
        if (!isAttack)
        {
            isAttack = true;
            int originOrder = GetComponent<Order>().GetOriginalOrder();
            GetComponent<Order>().SetOriginOrder(10);
            Vector3 firstPos = transform.position;
            Vector3 fieldPos = field.transform.position;
            fieldPos.y += 1f;
            transform.DOMove(fieldPos, .15f).SetEase(Ease.InElastic).OnComplete(() => {
                OnAttack();


                transform.DOMove(firstPos, .3f).OnComplete(() => {
                    GetComponent<Order>().SetOriginOrder(originOrder);
                    if (field.curCard == null) // 명치 공격
                    {
                        print("명치 공격");
                        if (field.isPlayerField)
                        {
                            // 플레이어 피격
                        }
                        else
                        {
                            // 적 피격

                        }
                    }
                    else // 카드 공격
                    {
                        print("카드 공격");
                        if (field.curCard.item.figure < item.figure) // 공격 대상보다 쌜때
                        {
                            print("대상 파괴");
                            field.curCard.OnDie();
                        }
                        else if (field.curCard.item.figure == item.figure) // 같을때
                        {
                            print("둘다 파괴");
                            field.curCard.OnDie();
                            OnDie();
                        }
                        else if (field.curCard.item.figure > item.figure) // 공격대상이 더 썔때
                        {
                            print("자신 파괴");
                            OnDie();
                        }
                    }
                    isAttack = false;
                });
            });


        }

    }
    public void OnAttack()
    {
        foreach (var item in item.OnAttack)
        {
            foreach (var condition in item.condition)
            {
                if (!condition.CheckCondition())
                {
                    return;
                }
            }
            item.action.TakeAction(this);
        }
    }
    /*  public void OnDamage()
      {
          foreach (var item in item.OnDamage)
          {
              foreach (var condition in item.condition)
              {
                  if (!condition.CheckCondition())
                  {
                      return;
                  }
              }
              item.action.TakeAction();
          }
      }*/
    public void OnDie()
    {
        foreach (var item in item.OnDie)
        {
            foreach (var condition in item.condition)
            {
                if (!condition.CheckCondition())
                {
                    return;
                }
            }
            item.action.TakeAction(this);
        }
    }
}
