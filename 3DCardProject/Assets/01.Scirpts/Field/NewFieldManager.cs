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
        fields.GetNodeByIndex(5).Data.SetUp(playerCard,playerCard.OnSpawn);
        fields.GetNodeByIndex(2).Data.SetUp(enemyCard, playerCard.OnSpawn);
        PlayerManager.Instance.playerCards.Add(playerCard);
        
    }
    public Card CreateCard(Item item)
    {
        var cardObj = Instantiate(CardManager.Instance.cardPrefab, transform.position, Utils.QI);
        var card = cardObj.GetComponent<Card>();
        card.Setup(item, true, true);
        card.GetComponent<Order>().SetOriginOrder(1);
        return card;
    }

    public void AvatarMove(Field field)
    {
        var node = fields.GetNodeByData(field);
        Card card = node.Data.avatarCard;
        if (card != null  && card.item.isAvatar)
        {
            Debug.Log(node.NextNode.Data);
            Debug.Log("avatarMoveTry");
            Move(node.NextNode.Data, card);
        }
    }

    public void Spawn(Field field, Card card)
    {
        if (field != null)
        {
            
            field.SetUp(card,card.OnSpawn);
        }
    }

    /// <summary>
    /// 카드를 필드로이동
    /// </summary>
    /// <param name="field">도착 필드</param>
    /// <param name="card">이동할 카드</param>
    public void Move(Field field, Card card)
    {
        Debug.Log(field);
        if (field != null)
        {
            Debug.Log("Field != null");
            // 나중에 새로 프로젝트 팔때 수정필요 !!!!!!!!!!!!!!!!!!!!!
            if (field.upperCard != null)
            {
                Debug.Log("upperCard != null");
                if (field.upperCard.item.canStandOn)
                {
                    Debug.Log("upperCard CanStandOn");

                    if (card.curField != null)
                    {
                        Debug.Log("curField != null");
                        if(card.isPlayerCard)
                        {
                            card.curField.RemoveAvatarCard();
                        }
                        else
                        {
                            card.curField.RemoveUpperCard();
                        }
                    }

                    Debug.Log("AAAAAA");

                    field.SetUp(card, field.upperCard.OnAttack);
                }
                else
                {
                    Debug.Log("upperCard CantStandOn");
                    card.Attack(field);
                    Debug.Log("BBBB");


                }
                Debug.Log("CCCCC");

            }
            else if (field.curCard != null)
            {
                if (field.curCard.item.canStandOn)
                {
                    
                    if (card.curField != null)
                    {
                        if (card.isPlayerCard)
                        {
                            card.curField.RemoveAvatarCard();
                        }
                        else
                        {
                            card.curField.RemoveCurCard();
                        }
                    }

                    field.SetUp(card, field.curCard.OnAttack);
                    Debug.Log("EEEE");

                }
                else
                {
                    card.Attack(field);

                    Debug.Log("FFFFF");

                }

            }
            else if(field.avatarCard != null)
            {
                card.Attack(field);
            }
            else
            {
                if (card.curField != null)
                {
                    if (card.isPlayerCard)
                    {
                        card.curField.RemoveAvatarCard();
                    }
                    else
                    {
                        card.curField.RemoveCurCard();
                    }
                }
                    Debug.Log("GGGGG");
                field.SetUp(card);

            }


        }
        else
        {
            Debug.Log("Field == null");
        }
    }
}
