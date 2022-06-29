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
        int rand = Random.Range(0, 1);

        Field prevField = node.PrevNode.Data;
        Field nextField = node.NextNode.Data;

        bool canCatch = NewFieldManager.Instance.IsClockDir ? node.NextNode.Data.avatarCard != null : node.PrevNode.Data.avatarCard != null;

        Item cardItem = null;
        Field setField = null;

        if (dm.GetTopItem().IsStructCard && canCatch && dm.GetNormalItem() != null)
        {
            cardItem = dm.PopNormalItem();
        }
        else
        {
            cardItem = dm.PopItem();
            if(cardItem == null)
            {
                EnemyDeckManager edm = dm as EnemyDeckManager;
                edm.SetUpEnemyDeckManager();
                cardItem = dm.PopItem();
            }
        }

        Card card = CardManager.Instance.CreateCard(cardItem, false);

        if (card != null)
        {
         bool canPrevField = node.PrevNode.Data.avatarCard == null && ((card.item.IsUpperCard && prevField.upperCard == null) || (!card.item.IsUpperCard && prevField.curCard == null));
            bool canNextField = node.NextNode.Data.avatarCard == null && ((card.item.IsUpperCard && nextField.upperCard == null) || (!card.item.IsUpperCard && nextField.curCard == null));

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

            CardManager.Instance.LastUsedCardItem = card.item.ShallowCopy();

            if (setField == null)
            {
                CardManager.Instance.CardDie(card);
                TurnManager.Instance.ChangeTurn();
            }
            else
            {
                NewFieldManager.Instance.Spawn(setField, card);

            }
        }
    }
}
