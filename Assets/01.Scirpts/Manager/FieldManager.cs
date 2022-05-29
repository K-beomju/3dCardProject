using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : Singleton<FieldManager>
{
    [SerializeField]
    private Dictionary<Vector2Int, Field> fields = new Dictionary<Vector2Int, Field>();

    public GameObject fieldPrefab;
    [SerializeField]
    private Vector3 originPos;
    [SerializeField]
    private float cellSize;
    [SerializeField]
    public int x, y;
    public Vector2Int DEFINENULLPOS { get; private set; } = new Vector2Int(999, 999);
    protected override void Awake()
    {
        base.Awake();

    }
    private void Start()
    {
        StartCoroutine(CreateFieldCo());
    }

    private IEnumerator CreateFieldCo()
    {
        for (int x = 0; x < this.x; x++)
        {
            for (int y = 0; y < this.y; y++)
            {
                Vector3 worldPos = new Vector3(x, 0, y);

                Field field = Instantiate(fieldPrefab, worldPos * cellSize + originPos, Quaternion.Euler(90, 0, 0)).GetComponent<Field>();
                field.isPlayerField = y == 0 || y == 1;

                fields.Add(new Vector2Int(x, y), field);
            }
        }

        yield return new WaitForSeconds(2f);

        var cardObj = Instantiate(CardManager.Instance.cardPrefab, transform.position, Utils.QI);
        var card = cardObj.GetComponent<Card>();
        card.Setup(PlayerManager.Instance.playerItem, true, true);
        card.GetComponent<Order>().SetOriginOrder(1);
        Spawn(PlayerManager.Instance.playerPos, card);
    }


    public bool CanAssign(Vector2Int gridPos)
    {
        if (gridPos.x >= 0 && gridPos.y >= 0 && gridPos.x < x && gridPos.y < y)
        {
            return true /*fields[gridPos].curCard == null*/;
        }
        else
        {
            return false;
        }
    }
    public void Spawn(Field field, Card card)
    {
        if (GetGridPos(field) != DEFINENULLPOS)
        {
            if (field != null)
            {
                if (field.curCard == null)
                {
                    field.SetUp(card);
                }
            }
        }

    }
    public void Spawn(Vector2Int gridPos, Card card)
    {
        Field field = GetField(gridPos);
        if (field != null)
        {
            if (field.curCard == null)
            {
                field.SetUp(card);
            }
        }
    }
    public void MoveToGrid(Field field, Card card)
    {
        if (GetGridPos(field) != DEFINENULLPOS)
        {
            if (field != null)
            {
                if (field.curCard != null)
                {
                    card.Attack(field);
                }
                else
                {
                    if (card.curField != null)
                    {
                        card.curField.RemoveCard();
                    }
                    field.SetUp(card);
                }
            }
        }
    }
    public void MoveToGrid(Vector2Int gridPos, Card card)
    {
        Field field = GetField(gridPos);
        if (field != null)
        {
            if (field.curCard != null)
            {
                card.Attack(field);
            }
            else
            {
                if (card.curField != null)
                {
                    card.curField.RemoveCard();
                }
                field.SetUp(card);
            }
        }
    }
    public Vector2Int GetGridPos(Field field)
    {
        if (fields.ContainsValue(field))
        {
            foreach (var item in fields)
            {
                if (item.Value == field)
                {
                    return item.Key;
                }
            }
        }
        return DEFINENULLPOS;

    }
    public Field GetField(Vector2Int gridPos)
    {
        if (CanAssign(gridPos))
        {
            Field field = fields[gridPos];
            return field;
        }
        return null;
    }
}
