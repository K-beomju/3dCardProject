using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int cost;            // 카드 가격 
    public string name;         // 카드 이름
    public Sprite sprite;       // 카드 이미지
    public bool isMagic;        // 마법 카드인가
    public float figure;        // 수치
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
}



[CreateAssetMenu(fileName = "ItemArraySO", menuName = "Scriptable Object/ItemArraySO")]
public class ItemArraySO : ScriptableObject
{
    public List<ItemSO> items;
}
