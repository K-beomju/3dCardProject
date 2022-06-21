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
    [SerializeField]
    private RectTransform dirImage = null;

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
            Quaternion q = Quaternion.Euler(new Vector3(90, 30 * (i + i), 0));
            node.Data.transform.rotation = q;
        }

        playerCard = CardManager.Instance.CreateCard(PlayerManager.Instance.playerItem,true);
        enemyCard = CardManager.Instance.CreateCard(EnemyManager.Instance.enemyItem,false);
        fields.GetNodeByIndex(5).Data.SetUp(playerCard, playerCard.OnSpawn);
        fields.GetNodeByIndex(2).Data.SetUp(enemyCard, enemyCard.OnSpawn);
        PlayerManager.Instance.playerCards.Add(playerCard);

        CheckCardDragSpawnRange();
    }

    private void Update()
    { // ���� ���� 
        CheckCardDragSpawnRange();
    }
    public void AvatarMove(Field field)
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
                Move(node.NextNode.Data, card);
            }
            else
            {
                Debug.Log(node.PrevNode.Data);
                Move(node.PrevNode.Data, card);
            }
        }
    }

    public void ChangeDir()
    {
        IsClockDir = !IsClockDir;
        dirImage.localScale = new Vector3(1, IsClockDir ? 1 : -1, 1);
    }
    public void Spawn(Field field, Card card)
    {
        if (field != null)
        {
            Debug.Log(card.name);
            if(!card.isPlayerCard)
            {
                if (ReflectBox.Instance.CardUIList.Count > 0)
                {
                    CardManager.Instance.OnReflect?.Invoke();
                    CardManager.Instance.WaitingActionUntilFinishOnReflect = () => { field.SetUp(card, () => { card.OnSpawn(); TurnManager.Instance.ChangeTurn(); }); };
                }
                else
                {
                    field.SetUp(card, () => { card.OnSpawn(); TurnManager.Instance.ChangeTurn(); });
                }
            }
            else
            {
                field.SetUp(card, () => { card.OnSpawn(); TurnManager.Instance.ChangeTurn(); });
            }
        }
    }

    /// <summary>
    /// ī�带 �ʵ���̵�
    /// </summary>
    /// <param name="field">���� �ʵ�</param>
    /// <param name="card">�̵��� ī��</param>
    public void Move(Field field, Card card)
    {
        Debug.Log(field);
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
                        if (card.isPlayerCard)
                        {
                            card.curField.RemoveAvatarCard();
                        }
                        else
                        {
                            card.curField.RemoveUpperCard();
                        }
                    }
                    field.SetUp(card, field.upperCard.OnAttack);
                }
                else
                {
                    Debug.Log("upperCard CantStandOn");
                    card.Attack(field);
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
                    field.SetUp(card, field.curCard.OnAttack);
                }
                else
                {
                    card.Attack(field);
                }

            }
            else if (field.avatarCard != null)
            {
                card.Attack(field);
            }
            else
            {
                if (card.curField != null)
                {
                    if (card.item.IsAvatar)
                    {
                        card.curField.RemoveAvatarCard();
                    }
                    else if(card.item.IsUpperCard)
                    {
                        card.curField.RemoveUpperCard();
                    }
                    else
                    {
                        card.curField.RemoveCurCard();
                    }
                }
                field.SetUp(card);

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

    
}
