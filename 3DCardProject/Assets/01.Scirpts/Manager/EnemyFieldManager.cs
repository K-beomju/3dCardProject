using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldManager : Singleton<EnemyFieldManager>
{
    private ActionButton actionButton;

    //public List<Item> itemBuffer { get; set; }



    public int spawnCardCount = 0;
    public List<Card> enemyCards = new List<Card>();

    private DeckManager dm;

    /* protected override void Awake()
     {
         base.Awake();
         SetupItemBuffer();
     }
 */
    private void Start()
    {
        dm = GetComponent<DeckManager>();
        actionButton = FindObjectOfType<ActionButton>();
        actionButton.OnMouseDownAct += EnemyAction;
    }
    public void Reseting()
    {
        StageSO stageData = StageManager.Instance.GetCurrentStageData();
        int cardCount = stageData.StageData.Length;
        for (int i = 0; i < cardCount; i++)
        {
            FieldManager.Instance.CheckingSpawn(stageData.StageData[i].vector2Int, CreateCard());
        }
    }


    private Card CreateCard()
    {
        var cardObj = Instantiate(CardManager.Instance.cardPrefab, transform.position, Utils.QI);
        var card = cardObj.GetComponent<Card>();
        card.Setup(dm.PopItem(), true, false);
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

        for (int i = 0; i < enemyCards.Count; i++)
        {
            int randX = Random.Range(-1, 2);
            int randY = Random.Range(-1, 2);
            Vector2Int gridPos = FieldManager.Instance.GetGridPos(enemyCards[i].curField);
            Vector2Int randPos = gridPos + new Vector2Int(randX, randY);
            Field randField = FieldManager.Instance.GetField(randPos);
            if (randField != null)
            {
                FieldManager.Instance.MoveToGrid(randPos, enemyCards[i]);
                // return new WaitUntil(() => enemyCards.TrueForAll(x => x.isMove));
            }
            /*else
            {

            }*/
            yield return new WaitForSeconds(0.5f);

        }


        yield return new WaitForSeconds(1f);
        TurnManager.ChangeTurn(TurnType.Player);

    }
    public void EnemyAction()
    {
        StartCoroutine(StartTurnActionCo());
    }


  /*  public Item PopItem()
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
    }*/
}
