using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public Item enemyItem;

    public DeckManager dm { get; private set; }
    private void Start()
    {
        dm = GetComponent<DeckManager>();
    }

    public void EnemyAction()
    {
        StartCoroutine(EnemyProcess());
    }

    public IEnumerator EnemyProcess()
    {
        if(GameManager.Instance.State == GameState.END)
        {
            yield break;
        }

        yield return new WaitForSeconds(1);

        Field field = NewFieldManager.Instance.enemyCard.curField;
        var node = NewFieldManager.Instance.fields.GetNodeByData(field);

        NewFieldManager.Instance.CheckCardDragSpawnRange(field);


        bool canCatch = NewFieldManager.Instance.IsClockDir ? node.NextNode.Data.avatarCard != null : node.PrevNode.Data.avatarCard != null;

        Item cardItem = GetRandItem(canCatch);
        Field setField = null;


        Card card = CardManager.Instance.CreateCard(cardItem, false);

        if (card != null)
        {
            var tempField = SetField(card, node);
            if(tempField != null)
            {
                setField = tempField;
            }


            CardManager.Instance.LastUsedCardItem = card.item.ShallowCopy();

            if (setField == null)
            {
                CardManager.Instance.CardDie(card);
                TurnManager.ChangeTurn(TurnType.Player);
            }
            else
            {
                NewFieldManager.Instance.Spawn(setField, card);

            }
        }
    }
    public Item PopItem(uint num = 0)
    {
        Item cardItem;
        EnemyDeckManager edm = dm as EnemyDeckManager;
        if (num == 0)
        {
            cardItem = dm.PopItem();
            if (cardItem == null)
            {
                edm.SetUpEnemyDeckManager();
                cardItem = dm.PopItem();
            }
        }
        else
        {
            cardItem = dm.PopItem(num);
            if (cardItem == null)
            {
                edm.SetUpEnemyDeckManager();
                cardItem = dm.PopItem(num);
            }
        }
       
        return cardItem;
    }
    public Field SetField(Card card,MyLinkedList<Field>.Node node)
    {
        if (card == null) return null;
        Field setField = null;

        Field prevField = node.PrevNode.Data;
        Field nextField = node.NextNode.Data;

        bool canPrevField = node.PrevNode.Data.avatarCard == null && ((card.item.IsUpperCard && prevField.upperCard == null) || (!card.item.IsUpperCard && prevField.curCard == null));
        bool canNextField = node.NextNode.Data.avatarCard == null && ((card.item.IsUpperCard && nextField.upperCard == null) || (!card.item.IsUpperCard && nextField.curCard == null));
        int rand = Random.Range(0, 1);

        if (rand == 0)
        {
            if (canPrevField)
            {
                setField = node.PrevNode.Data;

            }
            else if (canNextField)
            {
                setField = node.NextNode.Data;
            }
        }
        else
        {
            if (canNextField)
            {
                setField = node.NextNode.Data;

            }
            else if (canPrevField)
            {
                setField = node.PrevNode.Data;
            }
        }

        if (!card.item.IsStructCard)
        {
            Debug.Log("Struct : " + card.item.name);
            setField = CardManager.Instance.hackField;
        }
        return setField;
    }
    public Item GetRandItem(bool canCatch)
    {
        Item cardItem = null;
        if (dm.GetTopItem() != null && dm.GetTopItem().IsStructCard && canCatch && dm.GetNormalItem() != null)
        {
            cardItem = dm.PopNormalItem();
            if (cardItem == null)
            {
                cardItem = PopItem();
            }
        }
        else
        {
            cardItem = PopItem();
        }
        return cardItem;
    }
}
