using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : Singleton<PlayerManager>
{
    public Vector2Int playerPos;
    public Item playerItem;
    public int PlayerHP;

    public List<Card> playerCards = new List<Card>();
    public Card playerAvatarCard;
    public ParticleSystem deadPt;


    public static void TurnReset()
    {
        //Instance.playerCards.ForEach(x => x.isMove = false);
    }

    public static void RemoveCard(Card card)
    {
        Instance.playerCards.Remove(card);
    }

    public void PlayerDie()
    {
        playerAvatarCard.avtarAnim.SetTrigger("Dead");
    }

    public void DeadParticle()
    {
        Instantiate(deadPt, playerAvatarCard.transform.position, Utils.QI);
    }

    public void LookEnemyAvatar()
    {
        playerAvatarCard.LinkedModel.ModelObject.transform.DOLookAt(EnemyManager.Instance.enemyAvatarCard.LinkedModel.ModelObject.transform.position, 0f);
    }
}
