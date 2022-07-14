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

    [SerializeField] private Material noneMat;
    [SerializeField] private Material playerMat;
    [SerializeField] private Material enemyMat;

    [SerializeField] private GameObject hackObject;


    public void ChangeHack(Card card)
    {
        if (card.isPlayerCard)
        {
            //sr.color = playerColor;
            state = HackState.Player;
            hackObject.GetComponent<Renderer>().material = playerMat;
        }
        else
        {
            //sr.color = enemyColor;
            state = HackState.Enemy;
            hackObject.GetComponent<Renderer>().material = enemyMat;

        }
    }

  
}
