using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldManager : Singleton<EnemyFieldManager>
{
    private ActionButton actionButton;

    public List<Item> itemBuffer { get; set; }



    public int spawnCardCount = 0;
    public List<Card> enemyCards = new List<Card>();


    protected override void Awake()
    {
        base.Awake();
        SetupItemBuffer();
    }

    public void Reseting()
    {
        actionButton = FindObjectOfType<ActionButton>();
        actionButton.OnMouseDownAct += EnemyAction;
        FieldManager.Instance.CheckingSpawn(new Vector2Int(2, 3), CreateCard());
    }

    private Card CreateCard()
    {
        var cardObj = Instantiate(CardManager.Instance.cardPrefab, transform.position, Utils.QI);
        var card = cardObj.GetComponent<Card>();
        card.Setup(PopItem(), true, false);
        card.GetComponent<Order>().SetOriginOrder(1);
        spawnCardCount++;
        enemyCards.Add(card);
        return card;
    }


    private IEnumerator StartTurnActionCo()
    {
        // 움직임, 공격순 
        TurnManager.CurChangeType(TurnType.Change);
        enemyCards.ForEach((x => x.isMove = false));

        yield return new WaitForSeconds(1f);
        TurnManager.CurChangeType(TurnType.Enemy);


            int randX = Random.Range(-1, 2);
            int randY = Random.Range(-1, 2);
        Vector2Int gridPos = FieldManager.Instance.GetGridPos(enemyCards[0].curField);
        Vector2Int randPos = gridPos + new Vector2Int(randX, randY);
        if(FieldManager.Instance.GetField(randPos) != null)
        {
            FieldManager.Instance.MoveToGrid(randPos, enemyCards[0]);
            yield return new WaitUntil(() => enemyCards.TrueForAll(x => x.isMove));
        }
        else
        {
            yield return new WaitForSeconds(0.5f);

        }

        yield return new WaitForSeconds(1f);
        TurnManager.ChangeTurn(TurnType.Player);

    }
    public void EnemyAction()
    {
        StartCoroutine(StartTurnActionCo());
    }


    public Item PopItem()
    {
        if (itemBuffer.Count == 0)
            SetupItemBuffer();

        Item item = itemBuffer[0];
        itemBuffer.RemoveAt(0);
        return item;
    }

    private void SetupItemBuffer()
    {
        itemBuffer = new List<Item>();

        itemBuffer = StageManager.SetStageBuffer(itemBuffer);

        List<Item> tempBuffer = new List<Item>();

        foreach (var item in itemBuffer)
        {
            if (item.isMagic)
            {
                tempBuffer.Add(item);
            }
        }
        foreach (var temp in tempBuffer)
        {
            itemBuffer.Remove(temp);
        }
    }
}
