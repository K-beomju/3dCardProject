using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int cost;            // ī�� ���� 

    public uint uid;            // ���� ��ȣ
    public string name;         // ī�� �̸�
    public Sprite sprite;       // ī�� �̹���
    public bool IsAvatar;       // �ƹ�Ÿ ī�� �ΰ�
    public bool CanStandOn;     // ���� ������ �� �ִ°�
    public bool IsReflectCard;  // �޾�ġ�� ī���ΰ�
    public bool IsUpperCard;    // ���� �ø� �� �ִ� ī���ΰ�
    public bool IsStructCard;   // �ǹ�, ��ġ ���� ī���ΰ�
    public GameObject EffectPrefab;
    public float spawnModelYPos;

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
    //�����Ǿ��� ��
    public CardActionCondition[] OnCreate;

    private List<int> aa = new List<int>()
    {
        0b0000,
        0b0000,
    };
    private int a;

    public void laa()
    {
        
    }
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

    public ItemSO avatar;
}
