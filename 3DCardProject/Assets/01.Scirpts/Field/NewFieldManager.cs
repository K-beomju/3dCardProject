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
        fields.GetNodeByIndex(5).Data.SetUp(playerCard, playerCard.OnSpawn);
        fields.GetNodeByIndex(2).Data.SetUp(enemyCard, enemyCard.OnSpawn);
        PlayerManager.Instance.playerCards.Add(playerCard);

        CheckCardDragSpawnRange();
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
        if (card != null && card.item.IsAvatar)
        {
            Debug.Log("avatarMoveTry");
            CheckCardDragSpawnRange();
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

            field.SetUp(card, ()=> { card.OnSpawn(); TurnManager.Instance.ChangeTurn(); });
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
                if (field.curCard.item.CanStandOn)
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
            else if (field.avatarCard != null)
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
