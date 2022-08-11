using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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

    [SerializeField] private GameObject hackColorObject;
    private Renderer rend;
    [SerializeField] private Light hackLight;
    [SerializeField] private Transform hackTrm;
    private void Start()
    {
        rend = hackColorObject.GetComponent<Renderer>();

        noneMat.color = rend.material.GetColor("Color_AD284DAE");
        hackTrm.DOMoveY(hackTrm.position.y - .5f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
    public void ChangeHack(Card card)
    {
        if (card.isPlayerCard)
        {
            //sr.color = playerColor;
            state = HackState.Player;
            noneMat.DOColor(playerMat.color, .4f).OnUpdate(()=> { hackLight.color = noneMat.color; rend.material.SetColor("Color_AD284DAE", noneMat.color); } ).SetEase(Ease.InQuad);
            //rend.material.SetColor("Color_AD284DAE", noneMat.color);
        }
        else
        {
            //sr.color = enemyColor;
            state = HackState.Enemy;
            noneMat.DOColor(enemyMat.color, .4f).OnUpdate(()=> { hackLight.color = noneMat.color; rend.material.SetColor("Color_AD284DAE", noneMat.color); } ).SetEase(Ease.InQuad);
        }
    }

}
