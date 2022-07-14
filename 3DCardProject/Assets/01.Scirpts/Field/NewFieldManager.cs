using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFieldManager : Singleton<NewFieldManager>
{
    public MyLinkedList<Field> fields { get; private set; }

    public List<Field> fieldList = new List<Field>();


    public Card playerCard;
    public Card enemyCard;

    public bool IsClockDir { get; private set; } = true; // 시계방향인가?
    [SerializeField]
    private RectTransform dirImage = null;

    private bool canCheckRange = false;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        fields = new MyLinkedList<Field>(fieldList);

        for (int i = 0; i < fields.GetNodeCount(); i++)
        {
            var node = fields.GetNodeByIndex(i);
            Quaternion q = Quaternion.Euler(new Vector3(90, 0, 0));
            node.Data.transform.rotation = q;
        }
        TurnManager.Instance.CanChangeTurn = false;
        playerCard = CardManager.Instance.CreateCard(PlayerManager.Instance.playerItem.ShallowCopy(), true);
        enemyCard = CardManager.Instance.CreateCard(EnemyManager.Instance.enemyItem.ShallowCopy(), false);
        fields.GetNodeByIndex(5).Data.SetUp(playerCard, () => {
            playerCard.OnSpawn();
            canCheckRange = true;
            TurnManager.Instance.CanChangeTurn = true;
        });
        fields.GetNodeByIndex(2).Data.SetUp(enemyCard, enemyCard.OnSpawn);
        PlayerManager.Instance.playerCards.Add(playerCard);


    }

    private void Update()
    { // 수정 예정 
        if (canCheckRange)
            CheckCardDragSpawnRange();
    }
    public void AvatarMove(Field field, Action act = null)
    {
        var node = fields.GetNodeByData(field);
        Card card = node.Data.avatarCard;
        if (card != null && card.item.IsAvatar)
        {
            Debug.Log("avatarMoveTry");
            //CheckCardDragSpawnRange();

            if (IsClockDir)
            {
                Debug.Log(node.NextNode.Data);
                Move(node.NextNode.Data, card, act);
            }
            else
            {
                Debug.Log(node.PrevNode.Data);
                Move(node.PrevNode.Data, card, act);
            }
        }
    }

    public void ChangeDir()
    {
        IsClockDir = !IsClockDir;
        dirImage.localScale = new Vector3(1, IsClockDir ? 1 : -1, 1);
    }
    public void Spawn(Field field, Card card, Action act = null)
    {
        if (field != null)
        {
            Debug.Log(card.name);
            if (!card.isPlayerCard)
            {
                if (ReflectBox.Instance.CardUIList.Count > 0)
                {
                    ReflectBox.Instance.WaitingCard = card;
                    CardManager.Instance.WaitingActionUntilFinishOnReflect = () => { field.SetUp(card, () => { card.OnSpawn(); }); };
                    CardManager.Instance.OnReflect?.Invoke();
                }
                else
                {
                    if (card.item.IsReflectCard)
                    {
                        ReflectBox.Instance.RemoveCardUI(card);
                    }
                    field.SetUp(card, () => { card.OnSpawn(); act?.Invoke(); });
                }
            }
            else
            {
                if (card.item.IsReflectCard)
                {
                    ReflectBox.Instance.RemoveCardUI(card);
                }
                field.SetUp(card, () => { card.OnSpawn(); act?.Invoke(); });
            }
        }
    }

    /// <summary>
    /// 카드를 필드로이동
    /// </summary>
    /// <param name="field">도착 필드</param>
    /// <param name="card">이동할 카드</param>
    public void Move(Field field, Card card, Action act = null)
    {
        Debug.Log(field);
        TurnManager.Instance.CanChangeTurn = false;
        if (field != null)
        {
            Debug.Log("Field != null");
            // 나중에 새로 프로젝트 팔때 수정필요 !!!!!!!!!!!!!!!!!!!!!
            if (field.upperCard != null)
            {
                Debug.Log("upperCard != null");
                if (field.upperCard.item.CanStandOn)
                {
                    Debug.Log("upperCard CanStandOn");

                    if (card.curField != null)
                    {
                        Debug.Log("curField != null");
                        if (card.item.IsAvatar)
                        {
                            card.curField.RemoveAvatarCard();
                        }
                        else if (card.item.IsUpperCard)
                        {
                            card.curField.RemoveUpperCard();
                        }
                        else
                        {
                            card.curField.RemoveCurCard();
                        }
                    }
                    field.SetUp(card, () => { field.upperCard.OnAttack(); act?.Invoke(); });
                }
                else
                {
                    Debug.Log("upperCard CantStandOn");
                    card.Attack(field, act);
                }

            }
            else if (field.curCard != null)
            {
                if (field.curCard.item.CanStandOn)
                {

                    if (card.item.IsAvatar)
                    {
                        card.curField.RemoveAvatarCard();
                    }
                    else if (card.item.IsUpperCard)
                    {
                        card.curField.RemoveUpperCard();
                    }
                    else
                    {
                        card.curField.RemoveCurCard();
                    }
                    field.SetUp(card, () => { field.curCard.OnAttack(); act?.Invoke(); });
                }
                else
                {
                    card.Attack(field, act);
                }

            }
            else if (field.avatarCard != null)
            {
                card.Attack(field, act);
            }
            else
            {
                if (card.curField != null)
                {
                    if (card.item.IsAvatar)
                    {
                        card.curField.RemoveAvatarCard();
                    }
                    else if (card.item.IsUpperCard)
                    {
                        card.curField.RemoveUpperCard();
                    }
                    else
                    {
                        card.curField.RemoveCurCard();
                    }
                }
                field.SetUp(card, act);

            }


        }
        else
        {
            Debug.Log("Field == null");
        }
    }
    public void CheckCardDragSpawnRange()
    {
        for (int i = 0; i < fieldList.Count; i++)
        {
            fieldList[i].isEnterRange = false;
        }

        var node = fields.GetNodeByData(playerCard.curField);
        Field prevField = node.PrevNode.Data;
        Field nextField = node.NextNode.Data;
        prevField.isEnterRange = true;
        nextField.isEnterRange = true;

    }
    public void CheckCardDragSpawnRange(Field field)
    {
        for (int i = 0; i < fieldList.Count; i++)
        {
            fieldList[i].isEnterRange = false;
        }

        var node = fields.GetNodeByData(field);
        Field prevField = node.PrevNode.Data;
        Field nextField = node.NextNode.Data;
        prevField.isEnterRange = true;
        nextField.isEnterRange = true;

    }

    public MyLinkedList<Field>.Node GetNodeByData(Field field)
    {
        return fields.GetNodeByData(field);
    }

    public MyLinkedList<Field>.Node GetPlayerNodeByData()
    {
        return GetNodeByData(playerCard.curField);
    }   
    public MyLinkedList<Field>.Node GetEnemyNodeByData()
    {
        return GetNodeByData(enemyCard.curField);
    }
}
