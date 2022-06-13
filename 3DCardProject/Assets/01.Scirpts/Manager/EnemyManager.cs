using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public Item enemyItem;

    private DeckManager dm;
    private void Start()
    {
        dm = GetComponent<DeckManager>();
    }
}
