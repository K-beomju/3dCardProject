using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldManager : Singleton<EnemyFieldManager>
{
    public ActionButton actionButton;

    [SerializeField] protected ItemArraySO itemSO;
    public List<Item> itemBuffer { get; set; }

    private WaitForSeconds turnDelay = new WaitForSeconds(1f);

    public int spawnCardCount = 0;

    public List<Card> enemyCards = new List<Card>();

    private void Start()
    {
        actionButton.OnMouseDownAct += EnemyAction;
        FieldManager.Instance.CheckingSpawn(new Vector2Int(2, 3), CreateCard());
    }
    protected override void Awake()
    {
        base.Awake();
        SetupItemBuffer();
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

    private void EnemyCardAction()
    {
        Vector2Int gridPos = FieldManager.Instance.GetGridPos(enemyCards[0].curField);

        int randX = 0;
        int randY = 0;

        while (randX == 0 && randY == 0)
        {
            randX = Random.Range(-1, 2);
            randY = Random.Range(-1, 2);
        }

        var randPos = gridPos + new Vector2Int(randX, randY);
        if (!FieldManager.Instance.CanAssign(randPos))
            return;

        if (FieldManager.Instance.GetField(randPos).curCard == null)
            FieldManager.Instance.MoveToGrid(randPos, enemyCards[0]);

    }

    public IEnumerator CreateCardCo()
    {
        yield return turnDelay;
        EnemyCardAction();

        yield return turnDelay;
        TurnManager.ChangeTurn(TurnType.Player, ref TurnManager.isClick);
    }

    public void EnemyAction()
    {
        StartCoroutine(CreateCardCo());
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
        for (int i = 0; i < itemSO.items.Length; i++)
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
