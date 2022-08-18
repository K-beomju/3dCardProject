using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int cost;            // 카드 가격 

    public uint uid;            // 고유 번호
    public string name;         // 카드 이름
    public Sprite sprite;       // 카드 이미지
    public bool IsAvatar;       // 아바타 카드 인가
    public bool CanStandOn;     // 위에 서있을 수 있는가
    public bool IsReflectCard;  // 받아치기 카드인가
    public bool IsUpperCard;    // 위에 올릴 수 있는 카드인가
    public bool IsStructCard;   // 건물, 설치 가능 카드인가
    public GameObject EffectPrefab;
    public GameObject HitEffectPrefab;
    public float spawnModelYPos;

    public CardTribeType tribe; // 종족

    [TextArea]
    public string description;  // 카드 설명

    public float count;         // 카드 갯수
    //배치 했을 때
    public CardActionCondition[] OnSpawn;
    //죽었을 때
    public CardActionCondition[] OnDie;
    //공격할 때
    public CardActionCondition[] OnAttack;
    //공격받을 때
    public CardActionCondition[] OnDamage;
    //생성되었을 때
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
    NULL =  0b0000, // 없음
    FLY =   0b0001, // 비행
    WALK =  0b0010, // 지상
    WATER = 0b0100, // 수상
}


[CreateAssetMenu(fileName = "ItemArraySO", menuName = "Scriptable Object/ItemArraySO")]
public class ItemArraySO : ScriptableObject
{
    public List<ItemSO> items;

    public ItemSO avatar;
}
