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

    public bool isHit = false;
    public bool isEnterRange = false;
    public bool isHackField = false;

    public bool isCommon = false;

    private Outline outline;


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

    private void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

        isHackField = GetComponent<Hack>() != null;
        if (isHackField)
        {
            CardManager.Instance.hackField = this;
        }
    }
    public void HitColor(bool _isHit, bool _isAble = true)
    {
        isHit = _isHit;
        outline.enabled = _isHit;
        if (_isHit)
        {
            outline.OutlineColor = _isAble ? Utils.PlayerColor : Utils.EnemyColor;
        }
        //Debug.Log($"_isHit : {_isHit} , _isAble : {_isAble} ");
    }
    private void Update()
    {
        if (isHit)
        {
            if (CardManager.Instance.hitField != this)
            {
                //Debug.Log("Ray 나감");
                isHit = false;
                HitColor(false);
            }
        }
    }

    public void SetUp(Card card, Action act = null,Action subAct = null)
    {


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

        card.transform.DOScale(.8f, 0.15f);

        Sequence mySequence = DOTween.Sequence();

        Vector3 pos = transform.position;
        bool isTitle = StageManager.Instance.SceneState == SceneState.Title;
        pos += new Vector3(0, isTitle ? 0 : 1.6f, isTitle ? -2f:0);
        mySequence.Append(card.transform.DOMove(pos, .3f)).AppendInterval(.3f);
        mySequence.Join(card.transform.DORotateQuaternion(Quaternion.Euler(new Vector3(isTitle ? 0 : 90, 0, 0)), .1f));
        mySequence.Append(card.transform.DOMove(pos -= new Vector3(0, isTitle ? 0 :.45f, isTitle ? -.8f:0), .2f)).OnComplete(() =>
        {
            TurnManager.Instance.CanChangeTurn = true;
            if (card.LinkedModel != null && card.item.IsAvatar)
            {
                var node = NewFieldManager.Instance.GetNodeByData(this);

                Vector3 cardPos = card.transform.position;
                bool isJump = false;
                if (node.Data.upperCard != null)
                {
                    print("AAAA");
                    isJump = node.Data.upperCard.item.uid == 103;
                }
                if (isJump == true /*!NewFieldManager.Instance.isFrontJumping*/)
                {

                    Vector3 dir = NewFieldManager.Instance.IsClockDir ? node.NextNode.Data.transform.position - node.Data.transform.position : (node.PrevNode.Data.transform.position - node.Data.transform.position);

                    print(dir);
                    dir.y = 0;
                    card.LinkedModel.JumpMove(cardPos, dir.normalized, act, subAct);
                }
                else
                {
                    card.LinkedModel.Move(cardPos, act,subAct);
                }


                Debug.Log("이동 : " + card.item.itemName);
            }
            else
            {
                act?.Invoke();

                if (NewFieldManager.Instance.IsClockDir)
                {
                    if (NewFieldManager.Instance.GetPlayerNodeByData().NextNode.Data.upperCard.item.uid == 103)
                    {
                        NewFieldManager.Instance.isFrontJumping = true;
                    }
                }
                else
                {
                    if (NewFieldManager.Instance.GetPlayerNodeByData().PrevNode.Data.upperCard.item.uid == 103)
                    {
                        NewFieldManager.Instance.isFrontJumping = true;
                    }
                }


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
        upperCard = null;
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