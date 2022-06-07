using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int cost;            // ī�� ���� 

    public string name;         // ī�� �̸�
    public Sprite sprite;       // ī�� �̹���
    public bool isSpecial;      // Ư�� ī���ΰ�
    public float atk;        // ���ݷ�
    public float hp;        // ü��

    public CardTribeType tribe; // ����

    [TextArea]
    public string description;  // ī�� ����

    public float count;         // ī�� ����
    //��ġ ���� ��
    public CardActionCondition[] OnSpawn;
    //�׾��� ��
    public CardActionCondition[] OnDie;
    //������ ��
    public CardActionCondition[] OnAttack;
    //���ݹ��� ��
    public CardActionCondition[] OnDamage;
    public Item ShallowCopy()
    {
        return (Item)this.MemberwiseClone();
    }
}


public enum CardTribeType
{
    NULL =  0b0000, // ����
    FLY =   0b0001, // ����
    WALK =  0b0010, // ����
    WATER = 0b0100, // ����
}


[CreateAssetMenu(fileName = "ItemArraySO", menuName = "Scriptable Object/ItemArraySO")]
public class ItemArraySO : ScriptableObject
{
    public List<ItemSO> items;
}
