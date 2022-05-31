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
    [SerializeField] private TMP_Text hpTMP;
    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text descriptionTMP;


    public Item item;
    public PRS originPRS;
    private bool isFront;
    public bool isOnField = false;

    public bool isPlayerCard = true;
    public bool isMove = false;

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
            RefreshHPText();
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
        if (TurnManager.CurReturnType() != TurnType.Player) return;

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
        transform.DOScaleZ(transform.localScale.z + .5f, 0.15f).SetLoops(2, LoopType.Yoyo);
        transform.DOScaleX(transform.localScale.x + .5f, 0.15f).SetLoops(2, LoopType.Yoyo);
        transform.DOScaleY(transform.localScale.y + .5f, 0.15f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            act?.Invoke();
        });
    }
    public void RefreshHPText()
    {
        if (!item.isMagic)
        {
            hpTMP.text = this.item.hp.ToString();
        }
        else
        {
            hpTMP.text = "";
        }
    }
    public void Attack(Field field)
    {
        if (!isAttack && field.curCard != this)
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
                        print("ī�� ����");
                        if (field.curCard.item.hp < item.hp) // ���� ��󺸴� �۶�
                        {
                            print("��� �ı�");
                            item.hp -= field.curCard.item.hp;
                            RefreshHPText();
                            field.curCard.OnDie();
                        }
                        else if (field.curCard.item.hp == item.hp) // ������
                        {
                            print("�Ѵ� �ı�");
                            field.curCard.OnDie();
                            OnDie();
                        }
                        else if (field.curCard.item.hp > item.hp) // ���ݴ���� �� ����
                        {
                            print("�ڽ� �ı�");
                            field.curCard.item.hp -= item.hp;
                            field.curCard.RefreshHPText();
                            OnDie();
                        }
                    }
                    isAttack = false;
                    OnAttack();

                });
            });


        }

    }
    public void OnAttack()
    {
        foreach (var item in item.OnAttack)
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
}
