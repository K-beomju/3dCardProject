using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardModelBrain : MonoBehaviour
{
    
    public void Move()
    {
        DOTween.Kill(transform);

        transform.DOMove(transform.position,.5f);
    }
}
