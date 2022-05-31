using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldManager : Singleton<EnemyFieldManager>
{
    private ActionButton actionButton;

    [SerializeField] protected ItemArraySO itemSO;
    public List<Item> itemBuffer { get; set; }


    public int spawnCardCount = 0;

    public List<Card> enemyCards = new List<Card>();
    public bool isDone = false;


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
        // ������, ���ݼ� 
        TurnManager.CurChangeType(TurnType.Change);
        enemyCards.ForEach((x => x.isMove = false));

        yield return new WaitForSeconds(1f);
        TurnManager.CurChangeType(TurnType.Enemy);


        isDone = true;

        Vector2Int gridPos = FieldManager.Instance.GetGridPos(enemyCards[0].curField);

        Vector2Int randPos = default;
        do
        {
            int randX = Random.Range(-1, 2);
            int randY = Random.Range(-1, 2);
            randPos = gridPos + new Vector2Int(randX, randY);
        }
        while (!FieldManager.Instance.CanAssign(randPos) && randPos == gridPos &&
        FieldManager.Instance.GetField(randPos).curCard == null);


        FieldManager.Instance.MoveToGrid(randPos, enemyCards[0]);

        yield return new WaitUntil(() => enemyCards.TrueForAll(x => x.isMove));
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

        // ADD
        for (int i = 0; i < itemSO.items.Count; i++)
        {
            Item item = itemSO.items[i].item;
            for (int j = 0; j < item.count; j++)
                itemBuffer.Add(item);
        }

        // Shuffle
        for (int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = UnityEngine.Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;

        }
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
