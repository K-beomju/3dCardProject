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

    public bool IsClockDir { get; private set; } = true; // �ð�����ΰ�?
  

    public bool CanCheckRange = false;
    public bool isFrontJumping = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (StageManager.Instance.SceneState != SceneState.BATTLE) return;

        fields = new MyLinkedList<Field>(fieldList);

        for (int i = 0; i < fields.GetNodeCount(); i++)
        {
            var node = fields.GetNodeByIndex(i);
        }
        TurnManager.Instance.CanChangeTurn = false;
        //Item enemyItem = EnemyManager.Instance.enemyItem.ShallowCopy();
    

    }
    public IEnumerator FieldManagerStartCol()
    {
        uint enemyUid = EnemyManager.Instance.CurEnemyUid;
        if (enemyUid == 0)
        {
            enemyUid = 1001;
        }
        Item enemyItem = CardManager.Instance.FindEnemyData(enemyUid);

        enemyCard = CardManager.Instance.CreateCard(enemyItem, false);
        enemyCard.DetactiveCardView();
        playerCard = CardManager.Instance.CreateCard(PlayerManager.Instance.playerItem.ShallowCopy(), true);
        playerCard.DetactiveCardView();
        fields.GetNodeByIndex(2).Data.SetUp(enemyCard, enemyCard.OnSpawn);
        fields.GetNodeByIndex(5).Data.SetUp(playerCard, () => {
            playerCard.OnSpawn();
            CanCheckRange = true;
            TurnManager.Instance.CanChangeTurn = true;
        });
        //PlayerManager.Instance.playerCards.Add(playerCard);
        EnemyManager.Instance.enemyAvatarCard = enemyCard;
        PlayerManager.Instance.playerAvatarCard = playerCard;

        yield return new WaitForSeconds(2f);
    }

    private void Update()
    { // ���� ���� 
        if (CanCheckRange)
        {
            CheckCardDragSpawnRange();
            CanCheckRange = false;
        }
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

    public void TutorialAvatarMove(Field field, Action act = null)
    {
        StartCoroutine(TutorialAvatarMoveCo(field, act));
    }

    public IEnumerator TutorialAvatarMoveCo(Field field, Action act = null)
    {
        yield return new WaitWhile(() => !BattleTutorial.Instance.isEnemyTurn);
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
        GameManager.Instance.ChangeDirection();
    }
    public void Spawn(Field field, Card card, Action act = null)
    {
        if (field != null)
        {
            Debug.Log(card.name);
            if (!card.isPlayerCard)
            {
                if (ReflectBox.Instance.isCardOnHand)
                {
                    ReflectBox.Instance.WaitingCard = card;
                    CardManager.Instance.WaitingActionUntilFinishOnReflect = () => { field.SetUp(card, () => { card.OnSpawn(); }); };
                    CardManager.Instance.OnReflect?.Invoke();
                }
                else
                {
                    field.SetUp(card, () => { card.OnSpawn(); act?.Invoke(); });
                }
            }
            else
            {

             
                if( EnemyAI.Instance != null && EnemyAI.Instance.isReflectOnHand && (TutorialManager.Instance == null || (TutorialManager.Instance != null && !TutorialManager.Instance.isTutorial)))
                {
                    EnemyAI.Instance.WaitingCard = card;
                    EnemyAI.Instance.CallOnReflect(() => field.SetUp(card, () => { card.OnSpawn(); act?.Invoke(); }));
                }
                else
                {
                    if (card.item.IsReflectCard)
                    {
                        ReflectBox.Instance.isCardOnHand = false;
                    }
                    field.SetUp(card, () => { card.OnSpawn(); act?.Invoke(); });
                }

            }
        }
    }

    /// <summary>
    /// ī�带 �ʵ���̵�
    /// </summary>
    /// <param name="field">���� �ʵ�</param>
    /// <param name="card">�̵��� ī��</param>
    public void Move(Field field, Card card, Action act = null)
    {
        Debug.Log(field);
        act = new Action(() => { TurnManager.Instance.CanChangeTurn = true; } + act);
        TurnManager.Instance.CanChangeTurn = false;
        if (field != null)
        {
            Debug.Log("Field != null");
            // ���߿� ���� ������Ʈ �ȶ� �����ʿ� !!!!!!!!!!!!!!!!!!!!!
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
                    field.SetUp(card, act ,() => { field.upperCard.OnAttack(); });
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
        DisableSpawnRange();

        var node = fields.GetNodeByData(playerCard.curField);
        Field prevField = node.PrevNode.Data;
        Field nextField = node.NextNode.Data;
        prevField.isEnterRange = true;
        nextField.isEnterRange = true;

    }
    public void DisableSpawnRange()
    {
        for (int i = 0; i < fieldList.Count; i++)
        {
            fieldList[i].isEnterRange = false;
        }
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
