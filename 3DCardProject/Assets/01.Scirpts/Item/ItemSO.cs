using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int cost;            // ī�� ���� 
    public string name;         // ī�� �̸�
    public Sprite sprite;       // ī�� �̹���
    public bool isMagic;        // ���� ī���ΰ�
    public float figure;        // ��ġ
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
}



[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/ItemSO")]
public class ItemSO : ScriptableObject
{
    public Item[] items;
}
