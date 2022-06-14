using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack : MonoBehaviour
{
    [System.Serializable]
    private enum HackState
    {
        None = 0,
        Player, 
        Enemy
    }
    [SerializeField] private HackState state;

    //Field부분 업데이트 색깔 변경이슈
    //private Color32 playerColor = new Color32(0, 197, 255, 255);
    //private Color32 enemyColor = new Color32(255, 0, 0, 255);
    //private Color32 noneColor = new Color32(255, 255, 255, 255);


    //private SpriteRenderer sr;
    //private void Awake() => sr = GetComponent<SpriteRenderer>();
    //private void Start()
    //{
    //    if (sr != null)
    //        sr.color = noneColor;
    //}


    public void ChangeHack(Card card)
    {
        if (card.isPlayerCard)
        {
            //sr.color = playerColor;
            state = HackState.Player;
        }
        else
        {
            //sr.color = enemyColor;
            state = HackState.Enemy;
        }
    }

  
}
