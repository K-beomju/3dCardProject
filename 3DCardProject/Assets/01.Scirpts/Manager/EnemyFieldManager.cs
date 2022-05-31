using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldManager : Singleton<EnemyFieldManager>
{
    public ActionButton actionButton;

    [SerializeField] protected ItemSO itemSO;
    public List<Item> itemBuffer { get; set; }

    private WaitForSeconds turnDelay = new WaitForSeconds(1f);

    public int spawnCardCount = 0;

    public List<Card> enemyCards = new List<Card>();

    private void Start()
    {
        actionButton.OnMouseDownAct += CreateCardCoMethod;

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
    private void CallOnActionButtonClick()
    {
        

        int x = FieldManager.Instance.x;
        int y = FieldManager.Instance.y;
        Vector2Int pos = new Vector2Int(2, 3);

        if (FieldManager.Instance.GetField(pos).curCard != null)
        {
            int index = enemyCards.IndexOf(FieldManager.Instance.GetField(pos).curCard);
            Vector2Int gridPos = FieldManager.Instance.GetGridPos(enemyCards[index].curField);
            var randPos = gridPos + new Vector2Int(Random.Range(-1, 1), Random.Range(-1, 1));
            FieldManager.Instance.MoveToGrid(randPos, enemyCards[index]);
            return;
        }
        
            


        //if (FieldManager.Instance.CanAssign(pos))
        //{
        //    FieldManager.Instance.CheckingSpawn(pos, CreateCard());
        //}
    }

    public IEnumerator CreateCardCo()
    {
        yield return turnDelay;
        CallOnActionButtonClick();

        yield return turnDelay;
        TurnManager.ChangeTurn(TurnType.Player, ref TurnManager.isClick);

        //if (CardManager.Instance.MyCardIsFull())
        //    CardManager.Instance.AddCard();

    }

    public void CreateCardCoMethod()
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
            Item item = itemSO.items[i];
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
