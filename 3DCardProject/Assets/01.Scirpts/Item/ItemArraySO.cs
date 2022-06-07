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



[CreateAssetMenu(fileName = "ItemArraySO", menuName = "Scriptable Object/ItemArraySO")]
public class ItemArraySO : ScriptableObject
{
    public List<ItemSO> items;
}
