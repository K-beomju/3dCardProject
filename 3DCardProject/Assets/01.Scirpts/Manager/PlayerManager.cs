using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public Vector2Int playerPos;
    public Item playerItem;
    public int PlayerHP;
}
