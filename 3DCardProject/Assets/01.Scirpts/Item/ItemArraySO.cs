using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int cost;            // ī�� ���� 

    public uint uid;            // ���� ��ȣ
    public string itemName;         // ī�� �̸�
    public Sprite sprite;       // ī�� �̹���
    public bool IsAvatar;       // �ƹ�Ÿ ī�� �ΰ�
    public bool CanStandOn;     // ���� ������ �� �ִ°�
    public bool IsReflectCard;  // �޾�ġ�� ī���ΰ�
    public bool IsUpperCard;    // ���� �ø� �� �ִ� ī���ΰ�
    public bool IsStructCard;   // �ǹ�, ��ġ ���� ī���ΰ�
    public bool IsTitleCard;    // Ÿ��Ʋ ī���ΰ�
    public GameObject EffectPrefab;
    public GameObject HitEffectPrefab;
    public float SpawnModelYPos;
    public int Price;         // ī�� ����

    public CardType cardType; // ����

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

    public Vector3 cardImageSize; // ī�� �̹��� ������ ���� 
    public Vector3 cardImagePos;  // ī�� �̹��� ��ġ ���� 
    public Color cardImageColor;  // ī�� �̹��� ���� ���� 

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


public enum CardType
{
    Attack, // ����
    Avoid,  // ��ȿ
    Change, // ������ȯ
    Jump,   // ��Ʋ
    Stop,   // ��ž
    Trap,   // ��
    Wall    // ��
}


[CreateAssetMenu(fileName = "ItemArraySO", menuName = "Scriptable Object/ItemArraySO")]
public class ItemArraySO : ScriptableObject
{
    public List<ItemSO> items;
}
