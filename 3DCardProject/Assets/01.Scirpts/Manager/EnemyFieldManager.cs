using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldManager : Singleton<EnemyFieldManager>
{
    private ActionButton actionButton;

    //public List<Item> itemBuffer { get; set; }



    public int spawnCardCount = 0;
    public List<Card> enemyCards = new List<Card>();

    public Item enemyItem;

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
        Card card = CreateCard(enemyItem);
        Vector3 rot = card.transform.eulerAngles;
        card.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z - 180);
        FieldManager.Instance.CheckingSpawn(new Vector2Int(2, 3), card);
    }


    private Card CreateCard(Item item)
    {
        var cardObj = Instantiate(CardManager.Instance.cardPrefab, transform.position, Utils.QI);
        var card = cardObj.GetComponent<Card>();
        card.Setup(item, true, false);
        card.GetComponent<Order>().SetOriginOrder(1);
        card.isMove = true;
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

        // 스폰
        Vector2Int pos = new Vector2Int(Random.Range(0, FieldManager.Instance.x), FieldManager.Instance.y - 1);
        Field field = FieldManager.Instance.GetField(pos);
        if (field != null && field.curCard == null)
            FieldManager.Instance.CheckingSpawn(pos, CreateCard(dm.PopItem()));

        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < enemyCards.Count; i++)
        {
            int randX = Random.Range(-1, 2);
            int randY = Random.Range(-1, 2);
            Vector2Int gridPos = FieldManager.Instance.GetGridPos(enemyCards[i].curField);
            Vector2Int randPos = gridPos + new Vector2Int(randX, randY);
            Field randField = FieldManager.Instance.GetField(randPos);
            if (randField != null )
            {
                if (!enemyCards[i].isMove)
                {
                    FieldManager.Instance.MoveToGrid(randPos, enemyCards[i]);
                }
            }
            else
            {
                enemyCards[i].isMove = true;
            }
            yield return new WaitForSeconds(0.5f);

        }
        yield return new WaitUntil(() => enemyCards.TrueForAll(x => x.isMove));

        //yield return new WaitForSeconds(1f);

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
