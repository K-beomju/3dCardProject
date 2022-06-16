using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFieldManager : Singleton<NewFieldManager>
{
    private MyLinkedList<Field> fields;

    public List<Field> fieldList = new List<Field>();
    public Card playerCard;
    public Card enemyCard;


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

        playerCard = CreateCard(PlayerManager.Instance.playerItem);
        enemyCard = CreateCard(EnemyManager.Instance.enemyItem);
        fields.GetNodeByIndex(5).Data.SetUp(playerCard);
        fields.GetNodeByIndex(2).Data.SetUp(enemyCard);
        PlayerManager.Instance.playerCards.Add(playerCard);
        CheckCardDragSpawnRange();


    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            RefreshMove();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Advance();
        }

    }
    public Card CreateCard(Item item)
    {
        var cardObj = Instantiate(CardManager.Instance.cardPrefab, transform.position, Utils.QI);
        var card = cardObj.GetComponent<Card>();
        card.Setup(item, true, true);
        card.GetComponent<Order>().SetOriginOrder(1);
        return card;
    }

    public void RefreshMove()
    {
        for (int i = 0; i < fields.GetNodeCount(); i++)
        {
            var node = fields.GetNodeByIndex(i);
            Card card = node.Data.curCard;
            if (card != null)
                card.isMove = false;
        }
    }
    public void Advance()
    {
        for (int i = 0; i < fields.GetNodeCount(); i++)
        {
            var node = fields.GetNodeByIndex(i);
            Card card = node.Data.curCard;
            if (card != null && !card.isMove && card.item.isAvatar)
            {
                card.isMove = true;
                Move(node.NextNode.Data, card);

            }
        }
    }
    public void Move(Field field)
    {
        var node = fields.GetNodeByData(field);
        Card card = node.Data.curCard;
        if (card != null && !card.isMove && card.item.isAvatar)
        {
            card.isMove = true;
            Move(node.NextNode.Data, card);
            CheckCardDragSpawnRange();

        }
    }

    public void Spawn(Field field, Card card)
    {
        if (field != null)
        {
            card.isMove = true;
            field.SetUp(card);
        }
    }

    public void Move(Field field, Card card)
    {
        if (field != null)
        {
            if (field.curCard != null)
            {
                if(field.curCard.item.name == "µ£")
                {
                    card.CommonAction(field);
                }


                if (field.curCard.item.canStandOn)
                {
                    if (card.curField != null)
                    {
                        card.curField.RemoveCard();
                    }
                    field.SetUp(card);
                    card.isMove = true;
                }
                else
                {
                    card.Attack(field);
                    card.isMove = true;

                }
            }
            else
            {
                if ((field.EnableTribe & card.item.tribe) != CardTribeType.NULL)
                {
                    if (card.curField != null)
                    {
                        card.curField.RemoveCard();
                    }
                    field.SetUp(card);
                    card.isMove = true;

                }
            }


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
}
